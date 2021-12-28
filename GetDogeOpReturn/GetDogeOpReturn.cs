using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using GetDogeOpReturn.Entities;
using GetDogeOpReturn.Repos;
using System.Threading.Tasks;
using GetDogeOpReturn.Utils;
using Newtonsoft.Json;
using GetDogeOpReturn.Entities.Transactions;
using GetDogeOpReturn.Entities.Blocks;
using GetDogeOpReturn.Entities.Web;
using GetDogeOpReturn.Entities.Commands;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.Configuration;
using GetDogeOpReturn.Entities.Network;
using System.Linq;

namespace GetDogeOpReturn
{
    public static class GetDogeOpReturn
    {

        public const long MinBlockId = 4030000;
        public const int MaxBlocks = 30;
        public const long BlocksToEndBuffer = 3;
        public const string OP_RETURN = "OP_RETURN";
        public const long BlockIdDiv = 1000; // Should be multiple of 10. 
        public const string CoinName = "DOGE";
        public const string blockPrefixURL = "https://chain.so/api/v2/get_block/" + CoinName + "/";
        public const string transPrefixURL = "https://chain.so/api/v2/get_tx/" + CoinName + "/";
        public const string networkInfoURL = "https://chain.so/api/v2/get_info/" + CoinName;

        public const string LastSavedFileName = "LastSaved.txt";

        public static string ConnectionString = "";


        public static readonly List<string> BadAddresses = new List<string>()
        {
            "coinbase",
            "nonstandard",
        };

        [FunctionName("GetDogeOpReturn")]
        public static void Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            RunAsync().Wait();


        }


        public async static Task RunAsync()
        {
            var dict = Environment.GetEnvironmentVariables();
            ConnectionString = Environment.GetEnvironmentVariable("StorageAccount");
            //ConnectionString = ConfigurationManager.AppSettings["StorageAccount"];
            BlobRepository<BlockData> blockRepo = new BlobRepository<BlockData>(ConnectionString);

            BlobRepository<LastBlockSaved> savedRepo = new BlobRepository<LastBlockSaved>(ConnectionString);


            BlobRepository<BlockList> blockListRepo = new BlobRepository<BlockList>(ConnectionString);


            FullNetwork fullNetwork = await SendWebRequest.GetObject<FullNetwork>(networkInfoURL);

            if (fullNetwork == null || fullNetwork.data == null || fullNetwork.data.blocks < 1)
            {
                Console.Write("Failed to deserialize network data");
                return;
            }

            long maxAcceptableBlockId = fullNetwork.data.blocks - BlocksToEndBuffer;

            for (int b = 0; b < MaxBlocks; b++)
            {
                LastBlockSaved lastBlock = await savedRepo.Load(LastSavedFileName);

                if (lastBlock == null)
                {
                    lastBlock = new LastBlockSaved()
                    {
                        Id = LastSavedFileName,
                        LastBlockId = MinBlockId,
                    };

                    await savedRepo.Save(lastBlock);
                }

                if (lastBlock.LastBlockId > maxAcceptableBlockId)
                {
                    return;
                }

                string blockBlobId = BlockData.GetFileNameFromBlockId(lastBlock.LastBlockId);

                BlockData oldBlock = await blockRepo.Load(blockBlobId);

                if (oldBlock != null)
                {
                    lastBlock.LastBlockId++;
                    await savedRepo.Save(lastBlock);
                    continue;
                }

                FullBlock block = await SendWebRequest.GetObject<FullBlock>(blockPrefixURL + lastBlock.LastBlockId);

                if (block == null || block.data == null)
                {
                    Console.WriteLine("Missing inner block data");
                    return;
                }

                if (block.data.block_no != lastBlock.LastBlockId)
                {
                    Console.WriteLine("Block Height read did not match desired BlockId");
                    return;
                }


                BlockData currentData = new BlockData()
                {
                    Id = BlockData.GetFileNameFromBlockId(lastBlock.LastBlockId),
                    BlockId = lastBlock.LastBlockId,
                };

                block.data.txs = block.data.txs.OrderBy(x => x).ToList();

                for (int t = 0; t < block.data.txs.Count; t++)
                {
                    string txid = block.data.txs[t];

                    FullTransaction fullTrans = await SendWebRequest.GetObject<FullTransaction>(transPrefixURL + txid);
                    if (fullTrans == null || fullTrans.data == null || fullTrans.data.inputs.Count < 1)
                    {
                        continue;
                    }

                    string fromWallet = "";
                    
                    for (int ii = 0; ii < fullTrans.data.inputs.Count; ii++)
                    {
                        if (!BadAddresses.Contains(fullTrans.data.inputs[ii].address))
                        {
                            fromWallet = fullTrans.data.inputs[ii].address;
                            break;
                        }
                    }
                    
                    if (fullTrans.data.outputs.Count < 1)
                    {
                        continue;
                    }


                    string toWallet = "";
                    double outputSum = 0;


                    for (int oo = 0; oo < fullTrans.data.outputs.Count; oo++)
                    {
                        if (!BadAddresses.Contains(fullTrans.data.outputs[oo].address))
                        {
                            toWallet = fullTrans.data.outputs[oo].address;
                            outputSum = fullTrans.data.outputs[oo].value;
                            break;
                        }
                    }


                    if (string.IsNullOrEmpty(toWallet) || string.IsNullOrEmpty(fromWallet))
                    {
                        continue;
                    }


                    string raw_op_return = "";
                    foreach (TransactionIO tio in fullTrans.data.outputs)
                    {
                        if (!string.IsNullOrEmpty(tio.script) &&
                            tio.script.IndexOf(OP_RETURN) == 0)
                        {
                            raw_op_return = tio.script;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(raw_op_return))
                    {
                        continue;
                    }

                    string[] words = raw_op_return.Split(' ');
                    if (words.Length != 2 || words[0] != OP_RETURN)
                    {
                        continue;
                    }

                    string decodedCommand = StrUtils.HexToString(words[1]);


                    if (!string.IsNullOrEmpty(decodedCommand))
                    {
                        Command comm = new Command()
                        {
                            FromWallet = fromWallet,
                            ToWallet = toWallet,
                            Quantity = outputSum,
                            RawCommand = words[1],
                            DecodedCommand = decodedCommand,

                        };

                        currentData.Commands.Add(comm);
                    }

                }

                if (currentData.Commands.Count > 0)
                {
                    await blockRepo.Save(currentData);
                    string currentBlockListId = (currentData.BlockId / BlockIdDiv).ToString();

                    BlockList currentList = await blockListRepo.Load(currentBlockListId);

                    if (currentList == null)
                    {
                        currentList = new BlockList()
                        {
                            Id = currentBlockListId,
                        };
                    }


                    long idToSave = currentData.BlockId % BlockIdDiv;

                    if (!currentList.BlockIds.Contains(idToSave))
                    {
                        currentList.BlockIds.Add(idToSave);
                    }

                    await blockListRepo.Save(currentList);

                }
                lastBlock.LastBlockId++;
                await savedRepo.Save(lastBlock);
            }

        }
    }
}

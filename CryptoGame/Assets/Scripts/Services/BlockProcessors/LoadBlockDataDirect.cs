using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// opreturn.net
public class LoadBlockDataDirect : IBlockProcessor
{

    protected const string OP_RETURN = "OP_RETURN";
    protected const string blockPrefixURL = "https://chain.so/api/v2/get_block/DOGE/";
    protected const string transPrefixURL = "https://chain.so/api/v2/get_tx/DOGE/";  


    public static readonly List<string> BadAddresses = new List<string>()
        {
            "coinbase",
            "nonstandard",
        };
    public virtual IEnumerator Process(GameState gs, PlayerState ps)
    {
        long blockId = gs.processing.BlockId;

        BlockData currentData = gs.repo.Load<BlockData>(blockId.ToString());

        if (currentData == null)
        {
            WebResult blockResult = new WebResult();
            yield return SendWebRequest.GetRequest(blockPrefixURL + blockId,
                blockResult);

            if (!blockResult.Success)
            {
                gs.processing.BlockError = "Failed to download block " + blockId;
                yield break;
            }
            FullBlock block = null;
            try
            {
                block = JsonConvert.DeserializeObject<FullBlock>(blockResult.Text);
            }
            catch (Exception e)
            {
                gs.processing.BlockError = e.Message;
                yield break;
            }

            if (block == null || block.data == null)
            {
                gs.processing.BlockError = "Missing block at " + blockId;
                yield break;
            }

            if (block.data.block_no != blockId)
            {
                gs.processing.BlockError = "Block Height read did not match desired BlockId";
                yield break;
            }

            currentData = new BlockData()
            {
                BlockId = block.data.block_no,
            };

            for (int i = 0; i < block.data.txs.Count; i++)
            {
                string txid = block.data.txs[i];
                WebResult transResult = new WebResult();
                yield return SendWebRequest.GetRequest(transPrefixURL + txid,
              transResult);

                if (!transResult.Success)
                {
                    gs.processing.BlockError =
                    "Failed to download transaction: " + txid;
                    yield break;
                }

                FullTransaction fullTrans = null;
                try
                {
                    fullTrans = JsonConvert.DeserializeObject<FullTransaction>(transResult.Text);
                }
                catch (Exception e)
                {
                    gs.processing.BlockError = e.Message;
                    yield break;
                }

                if (fullTrans.data.inputs.Count < 1)
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
                
                Command comm = new Command()
                {
                    FromWallet = fromWallet,
                    ToWallet = toWallet,
                    Quantity = outputSum,
                    RawCommand = words[1],
                    DecodedCommand =decodedCommand,
                    
                };

                currentData.Commands.Add(comm);     

            }

            gs.repo.Save(currentData);
        }


        yield break;
    }
}
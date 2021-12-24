using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// opreturn.net
public class DownloadNewBlock : IBlockProcessor
{
    protected const string blockPrefixURL = "https://dogechain.info/api/v1/block/";
    //protected const string transPrefixURL = "https://dogechain.info/api/v1/transaction/";
    //protected const string transPrefixURL = "https://dogechain.info/rawblock/";
    protected const string transPrefixURL = "https://chain.so/api/v2/get_tx/DOGE/";
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
                throw new Exception("Failed to download block: " + blockId);
            }
            FullBlock block = null;
            try
            {
                block = JsonUtility.FromJson<FullBlock>(blockResult.Text);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (block == null || block.block == null)
            {
                throw new Exception("Failed to download block");
            }

            if (block.block.height != blockId)
            {
                throw new Exception("Block Height read did not match desired BlockId");
            }

            currentData = new BlockData()
            {
                BlockId = block.block.height,
            };

            for (int i = 0; i < block.block.txs.Count; i++)
            {
                string txid = block.block.txs[i];
                txid = "2bc3a2fadc9605f51957e61b6e3238e987031103e1ec6ddec9590751d6283026";
                WebResult transResult = new WebResult();
                yield return SendWebRequest.GetRequest(transPrefixURL + txid,
              transResult);

                if (!transResult.Success)
                {
                    throw new Exception("Failed to download transaction: " + txid);
                }

                FullTransaction fullTrans = null;
                try
                {
                    fullTrans = JsonUtility.FromJson<FullTransaction>(transResult.Text);
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (fullTrans.data.inputs.Count < 1)
                {
                    continue;
                }

                string fromWallet = fullTrans.data.inputs[0].address;

                if (fullTrans.data.outputs.Count < 1)
                {
                    continue;
                }

                string toWallet = fullTrans.data.outputs[0].address;
                double outputSum = fullTrans.data.outputs[0].value;

                string raw_op_return = "";
                foreach (TransactionIO tio in fullTrans.data.outputs)
                {
                    if (!string.IsNullOrEmpty(tio.script) &&
                        tio.script.IndexOf("OP_RETURN") == 0)
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
                if (words.Length != 2 || words[0] != "OP_RETURN")
                {
                    continue;
                }

                string decodedCommand = StrUtils.HexToString(words[1]);
                
                Command comm = new Command()
                {
                    FromWallet = fromWallet,
                    ToWallet = toWallet,
                    Quantity = outputSum,
                    FullCommand =decodedCommand,
                    
                };

                currentData.Commands.Add(comm);     

                Debug.Log("FullTrans is: " + fullTrans);
                break;
            }
        }


        yield break;
    }
}
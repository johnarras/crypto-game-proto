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
    protected const string transPrefixURL = "https://dogechain.info/api/v1/transaction/";

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
                txid = "e8294ebac3c8c9b6aed4b2fab08393e9035eb451f356e3cb88a119d55932ceda";
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

                double outputSum = 0;
                foreach (TransactionIO tio in fullTrans.transaction.outputs)
                {
                    if (tio.address == gs.processing.ToWallet)
                    {
                        outputSum += tio.value;
                    }
                }

                if (outputSum > 0)
                {
                    Command comm = new Command()
                    {
                        FromWallet = fullTrans.transaction.inputs[0].address,
                        ToWallet = fullTrans.transaction.outputs[0].address,
                        Quantity = outputSum,

                    };
                }

                Debug.Log("FullTrans is: " + fullTrans);
                break;
            }
        }


        yield break;
    }
}
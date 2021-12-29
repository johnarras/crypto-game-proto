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
public class LoadBlockData : IBlockProcessor
{

    protected string LastBlockSavedFilename = "LastSaved.txt";
    protected string BlockStatusFilename = "BlockStatus.txt";


    public static readonly List<string> BadAddresses = new List<string>()
    {
        "coinbase",
        "nonstandard",
    };

    public virtual IEnumerator Process(GameState gs)
    {
        if (gs.didDownloadBlocks)
        {
            yield break;
        }

        gs.didDownloadBlocks = true;

        CurrentBlockStatus blockStatus = gs.repo.Load<CurrentBlockStatus>(BlockStatusFilename);

        if (blockStatus == null)
        {
            blockStatus = new CurrentBlockStatus()
            {
                Id = BlockStatusFilename,
                CurrDownloadBlock = BlockConstants.MinBlock,
            };
            gs.repo.Save(blockStatus);
        }

        if (blockStatus.CurrDownloadBlock < BlockConstants.MinBlock)
        {
            blockStatus.CurrDownloadBlock = BlockConstants.MinBlock;
        }

        TypedResult<LastBlockSaved> saveResult = new TypedResult<LastBlockSaved>();

        yield return SendWebRequest.LoadFromBlob<LastBlockSaved>(LastBlockSavedFilename, saveResult);

        if (!saveResult.IsOk())
        {
            gs.processMessage = "No Last Saved Block";
            yield break;
        }

        gs.maxProcessBlock = saveResult.Data.LastBlockId;

        gs.repo.Save(blockStatus);

        long minId = blockStatus.CurrDownloadBlock;
        long maxId = saveResult.Data.LastBlockId;

        long minBlockListId = minId / BlockConstants.BlockIdDiv;
        long maxBlockListId = maxId / BlockConstants.BlockIdDiv;

        List<long> blockListsToDownload = new List<long>();

        for (long bid = minBlockListId; bid <= maxBlockListId; bid++)
        {
            blockListsToDownload.Add(bid);
        }


        foreach (long blockListId in blockListsToDownload)
        {
            TypedResult<BlockList> blockListResult = new TypedResult<BlockList>();

            yield return SendWebRequest.LoadFromBlob<BlockList>(blockListId.ToString(), blockListResult);

            if (!blockListResult.IsOk())
            {
                break;
            }

            BlockList blockList = blockListResult.Data;

            if (Int64.TryParse(blockList.Id, out long fullBlockListId))
            {
                long startBlockId = fullBlockListId * BlockConstants.BlockIdDiv;
                long nextStartBlockId = startBlockId + BlockConstants.BlockIdDiv;
                for (int bidx = 0; bidx < blockListResult.Data.BlockIds.Count; bidx++)
                {
                    long blockId = blockListResult.Data.BlockIds[bidx];
                    long fullBlockId = startBlockId + blockId;

                    if (fullBlockId < blockStatus.CurrDownloadBlock)
                    {
                        continue;
                    }
                    TypedResult<BlockData> blockResult = new TypedResult<BlockData>();

                    yield return SendWebRequest.LoadFromBlob<BlockData>(fullBlockId.ToString(), blockResult);

                    if (blockResult.IsOk())
                    {
                        gs.repo.Save(blockResult.Data);
                        blockStatus.CurrDownloadBlock = fullBlockId + 1;
                        if (bidx == blockListResult.Data.BlockIds.Count-1 &&
                            blockListId < maxBlockListId)
                        {
                            blockStatus.CurrDownloadBlock = nextStartBlockId;
                        }
                        gs.repo.Save(blockStatus);
                    }
                }
            }
            Debug.Log("Downloaded blocklist " + blockListId);
        }
        if (blockStatus.CurrDownloadBlock < gs.maxProcessBlock)
        {
            blockStatus.CurrDownloadBlock = gs.maxProcessBlock;
            gs.repo.Save(blockStatus);
        }
    }
}

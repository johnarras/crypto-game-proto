using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IBlockProcessor
{
    // These have no GetMinBlockId() because the version of the ProcessBlockService
    // we want to use will make a new list of these objects if we need to override them.


    IEnumerator Process(GameState gs, PlayerState ps);
}
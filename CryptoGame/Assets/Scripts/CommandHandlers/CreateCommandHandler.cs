﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHelpers.CommandHandlers
{
    public class CreateCommandHandler : ICommandHandler
    {
        public long GetMinBlockId() { return BlockConstants.V1; }

        public string GetKey() { return CommandList.Create; }

        public IEnumerator Process(GameState gs, Command comm)
        {
            IPlayerService playerService = gs.fact.Get<IPlayerService>();

            Player player = playerService.AddPlayer(gs, comm.FromWallet, comm.Args);

            ILandService landService = gs.fact.Get<ILandService>();

            landService.AddLand(gs, player.Id);

            yield break;
        }
    }
}
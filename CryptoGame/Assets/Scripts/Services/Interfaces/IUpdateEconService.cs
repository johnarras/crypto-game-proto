﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IUpdateEconService : IService
{
    public void Process(GameState gs, PlayerState ps);
}

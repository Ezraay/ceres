﻿using System;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class ClientBattle : Battle
    {
        [JsonConstructor]
        public ClientBattle(TeamManager teamManager) : base(teamManager) { }

        public void Execute(IServerAction action)
        {
            action.Apply(this);
        }
    }
}
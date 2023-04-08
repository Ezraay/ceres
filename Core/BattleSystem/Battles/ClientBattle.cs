﻿using System;
using System.Collections.Generic;
using Ceres.Core.BattleSystem.Battles;
using Newtonsoft.Json;

namespace Ceres.Core.BattleSystem
{
    public class ClientBattle : Battle
    {
        [JsonConstructor]
        public ClientBattle(TeamManager teamManager, PhaseManager phaseManager) : base(teamManager, phaseManager) { }

        public void Execute(IServerAction action)
        {
            action.Apply(this);
        }
    }
}
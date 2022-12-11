﻿using CardGame.Client.Display;
using CardGame.Slots;
using UnityEngine;

namespace CardGame.Client
{
    public class DisplayManager : MonoBehaviour
    {
        [SerializeField] private PlayerDisplay player1Display;
        [SerializeField] private PlayerDisplay player2Display;
        
        public void Setup(Battle battle)
        {
            player1Display.Setup(battle.Player1);
            player2Display.Setup(battle.Player2);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ceres.Core.BattleSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class MultiCardSlotDisplay : SlotDisplay
    {
        [SerializeField] private Transform content;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float cardLookOffset;
        private BoxCollider2D boxCollider2D;
        public List<CardDisplay> Displays { get; private set; } = new();


        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        public void AddCard(CardDisplay display)
        {
            Displays.Add(display);
            display.SetParent(this);
            display.transform.parent = content;
        }

        public void Setup(IMultiCardSlot slot, CardDisplayFactory factory)
        {
            if (slot is MultiCardSlot multiCardSlot)
            {
                foreach (Card card in multiCardSlot.Cards)
                {
                    AddCard(factory.Create(card));
                }
            }
            else
            {
                for (int i = 0; i < slot.Count; i++)
                {
                    AddCard(factory.CreateHidden());
                }
            }

            StartCoroutine(UpdatePositions());
        }

        public void RemoveCard(CardDisplay display)
        {
            Displays.Remove(display);
            display.SetParent(null);
            display.transform.parent = null;
        }

        [Button]
        private void ForceUpdatePositions()
        {
            Displays = GetComponentsInChildren<CardDisplay>().ToList();
            StartCoroutine(UpdatePositions());
        }

        public IEnumerator UpdatePositions()
        {
            // float leftRotation = cardLookOffset * (displays.Count - 1) / 2;
            float halfCount = (Displays.Count - 1) / 2f;

            for (int i = 0; i < Displays.Count; i++)
            {
                CardDisplay display = Displays[i];
                float angle = Mathf.Atan2((halfCount - i) * positionOffset.x, cardLookOffset);
                Vector3 position = new Vector3((i - halfCount) * positionOffset.x,
                    -Mathf.Pow(positionOffset.y * (i - halfCount), 2));

                Quaternion rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                StartCoroutine(display.MoveTo(position));
                display.transform.localRotation = rotation;
                display.SetSortingOrder(i + sortingOrderOffset);
            }

            yield return new WaitUntil(CardFinishedMoving);
        }

        private bool CardFinishedMoving()
        {
            bool moving = true;

            foreach (CardDisplay display in Displays)
                if (display.IsMoving)
                    moving = false;

            return moving;
        }

        public override bool CanDrag(ClientBattle battle, PlayerDisplay myPlayer)
        {
            if (battle.PhaseManager.Phase == BattlePhase.Main && myPlayer.Hand == this) return true;
            if (battle.PhaseManager.Phase == BattlePhase.Ascend && myPlayer.Hand == this) return true;
            if (battle.PhaseManager.Phase == BattlePhase.Defend && myPlayer.Hand == this &&
                battle.PhaseManager.CurrentTurnPlayer.Id != myPlayer.PlayerId) return true;
            return false;
        }
    }
}
using Ceres.Client.BattleSystem.Display.CardDisplays;
using Ceres.Core.BattleSystem.Actions.PlayerActions;
using Ceres.Core.BattleSystem.Battles;
using Ceres.Core.BattleSystem.Slots;
using UnityEngine;

namespace Ceres.Client.BattleSystem.Display
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private LayerMask cardMask;
        [SerializeField] private LayerMask slotMask;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CardDisplay card = RaycastCard();
                ICardSlotDisplay display = RaycastSlot();
                if (display == null) return;
                
                // if (display?.Slot == battleManager.battle.PriorityPlayer.Hand)
                //     battleManager.battle.Execute(new AscendFromHand(card.Card), battleManager.battle.PriorityPlayer);

                CardSlot cardSlot = display.Slot as CardSlot;
                MultiCardSlot multiCardSlot = display.Slot as MultiCardSlot;
                if (cardSlot != null && battleManager.Battle.BattlePhaseManager.Phase == BattlePhase.Attack && display.Owner == battleManager.Battle.AttackingPlayer)
                {
                    battleManager.Battle.Execute(new DeclareAttack(cardSlot), display.Owner);
                } else if (multiCardSlot == battleManager.Battle.DefendingPlayer.Hand && battleManager.Battle.BattlePhaseManager.Phase == BattlePhase.Defend)
                {
                    battleManager.Battle.Execute(new DefendFromHand(card.Card));
                } else if (multiCardSlot == battleManager.Battle.AttackingPlayer.Hand &&
                           battleManager.Battle.BattlePhaseManager.Phase == BattlePhase.Ascend)
                {
                    battleManager.Battle.Execute(new AscendFromHand(card.Card));
                }
            }
        }

        private CardDisplay RaycastCard()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardMask))
                return hit.collider.GetComponent<CardDisplay>();
            return null;
        }

        private ICardSlotDisplay RaycastSlot()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, slotMask))
                return hit.collider.GetComponent<ICardSlotDisplay>();
            return null;
        }
    }
}
using UnityEngine;

namespace CardGame
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
                if (cardSlot != null && battleManager.battle.Phase == BattlePhase.Attack && display.Owner == battleManager.battle.AttackingPlayer)
                {
                    battleManager.battle.Execute(new DeclareAttack(cardSlot), display.Owner);
                } else if (multiCardSlot == battleManager.battle.DefendingPlayer.Hand && battleManager.battle.Phase == BattlePhase.Defend)
                {
                    battleManager.battle.Execute(new DefendFromHand(card.Card));
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
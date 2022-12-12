using CardGame.Actions;
using CardGame.Client.Display;
using UnityEngine;

namespace CardGame.Client
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
                if (display == null && card == null) return;
                
                // if (display?.Slot == battleManager.battle.PriorityPlayer.Hand)
                //     battleManager.battle.Execute(new AscendFromHand(card.Card), battleManager.battle.PriorityPlayer);
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
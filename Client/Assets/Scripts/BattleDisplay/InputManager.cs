using CardGame.BattleDisplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask cardMask;
        [SerializeField] private CardPreviewDisplay cardPreviewDisplay;
        [SerializeField] [ReadOnly] private CardDisplay draggedCard;
        private Vector2 draggedCardStartPosition;

        
        
        private void Update()
        {
            CardDisplay display = RaycastCard();

            if (display != null && display.Card != null && draggedCard == null)
                cardPreviewDisplay.Show(display);
            else
                cardPreviewDisplay.Hide();

            if (Input.GetMouseButtonDown(0) && display != null)
            {
                // Start dragging
                draggedCard = display;
                draggedCardStartPosition = display.transform.position;
            }

            if (Input.GetMouseButtonUp(0) && draggedCard != null)
            {
                draggedCard.transform.position = draggedCardStartPosition;
                cardPreviewDisplay.Hide();
                draggedCard = null;
                return;
            }

            if (draggedCard != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                draggedCard.transform.position = mousePosition;
            }
        }


        private CardDisplay RaycastCard()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 1000, cardMask);
            CardDisplay card = null;

            foreach (var hit in hits)
            {
                CardDisplay other = hit.collider.GetComponent<CardDisplay>();
                if ((card == null || other.SortingOrder > card.SortingOrder) && other.Card != null)
                    card = other;
            }

            return card;
        }
    }
}
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardPreviewDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay display;
        [SerializeField] private LayerMask cardMask;

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000, cardMask);

            if (hit.collider != null)
            {
                CardDisplay card = hit.collider.GetComponent<CardDisplay>();
                if (card.Card != null)
                {
                    display.ShowFront(card.Card);
                    display.gameObject.SetActive(true);
                    return;
                }
            }

            display.gameObject.SetActive(false);
        }
    }
}
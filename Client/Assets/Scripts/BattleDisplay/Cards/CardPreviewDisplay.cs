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
                display.SetCard(card.Card);
                display.gameObject.SetActive(true);
            }
            else
            {
                display.gameObject.SetActive(false);
            }
        }
    }
}
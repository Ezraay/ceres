using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CardPreviewDisplay : MonoBehaviour
    {
        [SerializeField] private CardDisplay display;
        [SerializeField] private LayerMask cardMask;
        private Vector2 cardColliderSize;

        private void Start()
        {
            cardColliderSize = display.GetComponent<BoxCollider2D>().size;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 1000, cardMask);
            CardDisplay card = null;

            foreach (var hit in hits)
            {
                CardDisplay other = hit.collider.GetComponent<CardDisplay>();
                if (card == null || other.SortingOrder > card.SortingOrder)
                    card = other;
            }


            if (card != null && card.Card != null)
            {
                // CardDisplay card = hit.collider.GetComponent<CardDisplay>();
                // if (card.Card != null)
                // {
                    display.ShowFront(card.Card);
                    display.gameObject.SetActive(true);
                    display.transform.position = GetSafePosition(card.transform.position);
                    return;
                // }
            }

            display.gameObject.SetActive(false);
        }

        private Vector2 GetSafePosition(Vector2 input)
        {
            Vector2 cardSize = cardColliderSize * display.transform.localScale;
            float orthographicSize = Camera.main.orthographicSize;
            Vector2 cameraSize = new Vector2(orthographicSize * Camera.main.aspect, orthographicSize);
            Vector2 topRight = cameraSize - cardSize / 2;
            Vector2 bottomLeft = cardSize / 2 - cameraSize;
            Vector2 output = input;

            output.x = Mathf.Clamp(output.x, bottomLeft.x, topRight.x);
            output.y = Mathf.Clamp(output.y, bottomLeft.y, topRight.y);
            
            return output;
        }
    }
}
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

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

        public void Show(CardDisplay cardDisplay)
        {
            display.ShowFront(cardDisplay.Card);
            display.transform.position = GetSafePosition(cardDisplay.transform.position);
            display.gameObject.SetActive(true);
        }

        public void Hide()
        {
            display.gameObject.SetActive(false);
        }
    }
}
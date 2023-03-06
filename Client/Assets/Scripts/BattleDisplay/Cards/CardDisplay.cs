using System;
using System.Collections;
using Ceres.Core.BattleSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class CardDisplay : MonoBehaviour
    {
        private const float StoppingDistance = 0.05f;
        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private Canvas canvas;

        [SerializeField] private GameObject content;
        [SerializeField] private CardSpriteManager cardSpriteManager;
        [SerializeField] private Image sprite;
        [SerializeField] private TMP_Text attack;
        [SerializeField] private TMP_Text defense;
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text tier;
        public bool IsMoving { get; private set; }
        public bool IsHidden { get; private set; }
        public Card Card { get; private set; }
        public int SortingOrder => canvas.sortingOrder;
        private IEnumerator currentMove;

        private void Start()
        {
            if (Card == null)
                ShowBack();
        }

        public void ShowBack()
        {
            content.SetActive(false);
            IsHidden = true;
        }
        
        public void ShowFront(Card card)
        {
            content.SetActive(true);
            Card = card;
            name.text = card.Data.Name;
            attack.text = card.Data.Attack.ToString();
            defense.text = card.Data.Defense.ToString();
            tier.text = card.Data.Tier.ToString();
            sprite.sprite = cardSpriteManager.GetSprite(card.Data.ID);
            IsHidden = false;
        }

        public void SetSortingOrder(int order)
        {
            canvas.sortingOrder = order;
        }
        
        public IEnumerator MoveTo(Vector3 position)
        {
            if (currentMove != null)
                StopCoroutine(currentMove);
            
            currentMove = StartMove(position);
            yield return currentMove;
        }

        private IEnumerator StartMove(Vector3 position)
        {
            IsMoving = true;
            
            float distance;
            do
            {
                distance = Vector3.Distance(transform.localPosition, position);
                Vector3 direction = position - transform.localPosition;
                Vector3 velocity = direction.normalized * movementSpeed;
                Vector3 delta = velocity * Time.deltaTime * Mathf.Min(distance, 1f);
                
                transform.localPosition += delta;
                yield return null;
            } while (distance > StoppingDistance);

            IsMoving = false;
        }
    }
}
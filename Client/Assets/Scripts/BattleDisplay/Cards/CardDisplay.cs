using System;
using System.Collections;
using Ceres.Core.BattleSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class CardDisplay : SerializedMonoBehaviour
    {
        private const float StoppingDistance = 0.05f;
        [SerializeField] private float movementSpeed = 20;
        [SerializeField] private Canvas canvas;

        [SerializeField] private GameObject content;

        public bool IsMoving { get; private set; }
        public bool IsHidden { get; private set; }
        public Card Card { get; private set; }
        public int SortingOrder => canvas.sortingOrder;
        private IEnumerator currentMove;
        public SlotDisplay Parent { get; private set; }
        public event Action OnDestroyed;
        public event Action<Card> OnShowFront;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

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
        
        public virtual void ShowFront(Card card)
        {
            content.SetActive(true);
            content.SetActive(true);
            Card = card;

            IsHidden = false;
            OnShowFront?.Invoke(card);
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

        public void SetParent(SlotDisplay slotDisplay)
        {
            Parent = slotDisplay;
        }
    }
}
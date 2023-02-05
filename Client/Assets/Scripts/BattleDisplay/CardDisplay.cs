using System.Collections;
using Ceres.Core.BattleSystem;
using UnityEngine;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame.BattleDisplay
{
    public class CardDisplay : MonoBehaviour
    {
        private const float StoppingDistance = 0.05f;
        [SerializeField] private float movementSpeed = 5;
        public bool IsMoving { get; private set; }

        public void SetCard(Card card)
        {
            // TODO
        }
        
        public IEnumerator MoveTo(Vector3 position)
        {
            if (IsMoving)
            {
                Logger.LogWarning("Tried to move card when already moving.");   
                yield break;
            }
            
            IsMoving = true;
            float distance;
            do
            {
                distance = Vector3.Distance(transform.localPosition, position);
                Vector3 direction = position - transform.localPosition;
                Vector3 velocity = direction.normalized * movementSpeed;
                transform.localPosition += velocity * Time.deltaTime * Mathf.Min(distance, 1f);
                yield return null;
            } while (distance > StoppingDistance);

            IsMoving = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class MultiCardSlotDisplay : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private float cardLookOffset;
        private List<CardDisplay> displays = new();

        public IEnumerator AddCard(CardDisplay display)
        {
            displays.Add(display);
            display.transform.parent = content;
            yield return UpdatePositions();
        }

        [Button]
        private void ForceUpdatePositions()
        {
            displays = GetComponentsInChildren<CardDisplay>().ToList();
            StartCoroutine(UpdatePositions());
        }
        
        private IEnumerator UpdatePositions()
        {
            // float leftRotation = cardLookOffset * (displays.Count - 1) / 2;
            float halfCount = (displays.Count - 1) / 2f;

            for (int i = 0; i < displays.Count; i++)
            {
                CardDisplay display = displays[i];
                // Vector3 position = new Vector3((i - halfCount) * positionOffset.x,
                //     Mathf.Sqrt(Mathf.Abs(i - halfCount)) * positionOffset.y);
                // Vector3 position = new Vector3((i - halfCount) * positionOffset.x,
                //     Mathf.Sqrt(Mathf.Abs(i - halfCount)) * positionOffset.y);
                // float angle = Mathf.Atan2(-position.x, cardLookOffset);
                float angle = Mathf.Atan2((halfCount - i) * positionOffset.x, cardLookOffset);
                Vector3 position = new Vector3((i - halfCount) * positionOffset.x, -Mathf.Pow(positionOffset.y * (i - halfCount), 2));

                // Debug.Log(angle);
                // position.y = Mathf.Cos(angle);
                // float rotation = i * rotationOffset;

                // float rotationAngle = displays.Count > 1 ? (float) i / (displays.Count - 1) : 0.5f;

                display.transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
                //     Mathf.Lerp(-cardLookOffset, cardLookOffset, rotationAngle));
                // display.transform.localRotation = Quaternion.Euler(0, 0, (rotation - halfRotation) * (i) / displays.Count);
                StartCoroutine(display.MoveTo(position));
            }

            yield return new WaitUntil(CardFinishedMoving);
        }

        private bool CardFinishedMoving()
        {
            bool moving = true;

            foreach (CardDisplay display in displays)
                if (display.IsMoving)
                    moving = false;

            return moving;
        }
    }
}
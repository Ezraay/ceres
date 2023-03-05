using System.Collections;
using UnityEngine;

namespace CardGame.BattleDisplay
{
    public class CameraController : MonoBehaviour
    {
        public IEnumerator Shake(float duration, float amount)
        {
            Vector3 originalPosition = transform.localPosition;
            float remaining = duration;

            while (remaining > 0)
            {
                float x = Random.Range(-amount, amount);
                float y = Random.Range(-amount, amount);

                Vector3 offset = new Vector3(x, y, 0) * remaining / duration;
                remaining -= Time.deltaTime;
                transform.localPosition = originalPosition + offset;
                yield return null;
            }

            transform.localPosition = originalPosition;
        }
    }
}
using CardGame.Actions;
using UnityEngine;

namespace CardGame.Client
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private BattleManager battleManager;
        [SerializeField] private LayerMask cardMask;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardMask))
                {
                    CardDisplay display = hit.collider.GetComponent<CardDisplay>();
                    battleManager.battle.Execute(new AscendAction(display.Card), battleManager.battle.Player1);
                }
            }
                
        }
    }
}
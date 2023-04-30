using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.BattleDisplay;
using CardGame.BattleDisplay.Commands;
using Ceres.Client.BattleSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using Logger = Ceres.Client.Utility.Logger;

namespace CardGame
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask cardMask;
        [SerializeField] private LayerMask slotMask;
        [SerializeField] private CardPreviewDisplay cardPreviewDisplay;
        [SerializeField] [ReadOnly] private CardDisplay draggedCard;
        [SerializeField] [ReadOnly] private SlotDisplay draggedSlot;
        private readonly List<IInputCommand> commands = new();        
        private BattleDisplayManager battleDisplayManager;
        private BattleManager battleManager;
        private Vector2 draggedCardStartPosition;
        private Guid myPlayerId;
        private int draggedCardOrder;


        private void Awake()
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IInputCommand).IsAssignableFrom(p) && !p.IsInterface);
            foreach (Type type in types)
            {
                IInputCommand command = (IInputCommand) Activator.CreateInstance(type);
                commands.Add(command);
            }
            
            battleManager.OnStart += conditions =>
            {
                if (conditions.MyPlayerId != Guid.Empty)
                    myPlayerId = conditions.MyPlayerId;
                else
                    Destroy(gameObject);
            };
        }

        private void Update()
        {
            if (!this.battleManager.IsBattleOngoing) return;
            
            CardDisplay display = RaycastCard();

            if (display != null && display.Card != null && draggedCard == null)
                cardPreviewDisplay.Show(display);
            else
                cardPreviewDisplay.Hide();

            if (Input.GetMouseButtonDown(0) && display != null && battleDisplayManager.CanInteract)
            {
                // Start dragging
                draggedCard = display;
                draggedSlot = display.Parent;
                draggedCardOrder = draggedCard.SortingOrder;
                draggedCard.transform.localRotation = Quaternion.identity;
                draggedCardStartPosition = display.transform.position;
                draggedCard.SetSortingOrder(10);
            }

            if (Input.GetMouseButtonUp(0) && draggedCard != null)
            {
                SlotDisplay endSlot = RaycastSlot();
                draggedCard.SetSortingOrder(draggedCardOrder);
                if (endSlot != null)
                {
                    InputCommandData data = new InputCommandData(draggedSlot, endSlot, draggedCard,
                        battleManager.Battle, battleDisplayManager.GetPlayerDisplay(myPlayerId), battleManager.Battle.GetPlayerById(myPlayerId));
                    IInputCommand command = GetInputCommand(data);
                    if (command != null)
                    {
                        battleManager.Execute(command.GetCommand(data));
                    }
                    else
                    {
                        draggedCard.transform.position = draggedCardStartPosition;
                    }
                }
                else
                {
                    draggedCard.transform.position = draggedCardStartPosition;
                }

                cardPreviewDisplay.Hide();
                draggedSlot = null;
                draggedCard = null;
                return;
            }

            if (draggedCard != null && this.draggedSlot.CanDrag(this.battleManager.Battle, this.battleDisplayManager.GetPlayerDisplay(this.battleManager.MyPlayer.Id)))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                draggedCard.transform.position = mousePosition;
            }
        }


        [Inject]
        public void Construct(BattleManager battle, BattleDisplayManager battleDisplay)
        {
            battleManager = battle;
            battleDisplayManager = battleDisplay;
        }

        private CardDisplay RaycastCard()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 1000, cardMask);
            CardDisplay card = null;

            foreach (var hit in hits)
            {
                CardDisplay other = hit.collider.GetComponent<CardDisplay>();
                if ((card == null || other.SortingOrder > card.SortingOrder) && other.Card != null)
                    card = other;
            }

            return card;
        }

        private SlotDisplay RaycastSlot()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 1000, slotMask);
            SlotDisplay other = hit.collider?.GetComponent<SlotDisplay>();

            return other;
        }

        private IInputCommand GetInputCommand(InputCommandData data)
        {
            if (!battleDisplayManager.CanInteract)
                return null;

            IInputCommand result = null;
            // IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            //     .SelectMany(s => s.GetTypes())
            //     .Where(p => typeof(IInputCommand).IsAssignableFrom(p) && !p.IsInterface);

            foreach (IInputCommand command in commands)
                // IInputCommand command = (IInputCommand)Activator.CreateInstance(type);
                if (command.CanExecute(data))
                {
                    if (result != null)
                        Logger.LogError($"Commands {result} and {command} executed at the same time");
                    result = command;
                }

            return result;
        }
    }
}
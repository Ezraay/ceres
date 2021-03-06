using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemPickup : Interactable {
    private static int itemDespawnTime = 300;
    private static Dictionary<int, ItemPickup> pickups = new Dictionary<int, ItemPickup> ();

    public Item item { get; private set; }
    public int id { get; private set; }

    void Start () {
        Destroy (gameObject, itemDespawnTime);
    }

    void CheckSurroundingPickups () {
        Collider[] hitColliders = Physics.OverlapSphere (transform.position, interactRadius, GameManager.interactableMask);
        foreach (var hitCollider in hitColliders) {
            ItemPickup pickup = hitCollider.GetComponent<ItemPickup> ();
            if (pickup != null && pickup != this && pickup.item.id == item.id) {
                pickup.item.count += item.count;
                Destroy (gameObject);
                return;
            }
        }
        pickups.Add (id, this);
    }

    public static ItemPickup Get (int pickupID) {
        return pickups[pickupID];
    }

    public static List<ItemPickup> GetAllPickups () {
        return pickups.Values.ToList();
    }

    public void TakeItem (int count) {
        item.count -= count;
        if (item.count <= 0) {
            pickups.Remove (id);
            Destroy (gameObject);
        }
    }

    public void SetItem (Item item, int pickupID = -1) {
        if (pickupID == -1) pickupID = pickups.Count;
        id = pickupID;

        this.item = item;
        CheckSurroundingPickups ();
    }

    public override void Interact (Entity entity) {
        int count = item.count;
        item = entity.AddItem (item);
        if (entity == GameManager.mainPlayer)
            PacketSender.ItemPickedUp (this, item == null ? count : count - item.count);

        if (item == null) {
            pickups.Remove (id);
            Destroy (gameObject);
        }
    }

    protected override void OnMouseOver () {
        base.OnMouseOver ();
        TooltipUI.ShowTooltip ($"{item.count}x {item.name}", item.category, item.description);
        TooltipUI.SetTooltipRarity (ItemDatabase.rarityColours[item.rarity]);
    }
}
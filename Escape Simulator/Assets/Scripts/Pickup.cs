using UnityEngine;

public class Pickup : Interactable
{
    public InventoryItemData referenceItem;

    public override string GetDescription()
    {
        return "Press [E] to pickup or [Space] to examine.";
    }

    public override void Interact()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(gameObject);
    }
}
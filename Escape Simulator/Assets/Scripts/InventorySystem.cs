using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{
    [HideInInspector]
    public int currentSelection = -1;
    [HideInInspector]
    public GameObject currentHeldObject;

    public float throwForce;
    public Transform holdParent;

    public GameObject slotPrefab;
    public Color unHighlightedColour;
    public Color highlightedColour;
    public List<UIInventoryItemSlot> slots;

    public static InventorySystem current;

    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    void Update()
    {
        for (int i = 1; i <= slots.Count + 1; i++)
        {
            if (Input.GetKeyDown((i - 1).ToString()))
            {
                currentSelection = i - 2;
                for (int x = 0; x < slots.Count; x++)
                {
                    slots[x].bg.color = unHighlightedColour;
                }

                slots[currentSelection].bg.color = highlightedColour;
                if (holdParent.childCount > 0)
                {
                    Destroy(holdParent.GetChild(0).gameObject);
                }

                currentHeldObject = Instantiate(inventory[currentSelection].data.prefab, holdParent);
                currentHeldObject.GetComponent<Interactable>().canBeInteractedWith = true;
                currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
                currentHeldObject.layer = LayerMask.NameToLayer("NoCollision");
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentHeldObject != null)
            ThrowObject();

        if (Input.GetKeyDown(KeyCode.P) && currentHeldObject != null)
            MouseController.current.ActivatePlaceMode(inventory[currentSelection].data);
    }

    private void OnUpdateInventory()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        slots = new List<UIInventoryItemSlot>();
        foreach (InventoryItem item in inventory)
        {
            AddInventorySlot(item);
        }

        if (slots.Count == 0)
            currentSelection = -1;

        if (currentSelection > -1)
        {
            for (int x = 0; x < slots.Count; x++)
            {
                slots[x].bg.color = unHighlightedColour;
            }

            slots[currentSelection].bg.color = highlightedColour;
            if (holdParent.childCount > 0)
            {
                Destroy(holdParent.GetChild(0).gameObject);
            }

            currentHeldObject = Instantiate(inventory[currentSelection].data.prefab, holdParent);
            currentHeldObject.GetComponent<Interactable>().canBeInteractedWith = true;
            currentHeldObject.GetComponent<Rigidbody>().isKinematic = true;
            currentHeldObject.layer = LayerMask.NameToLayer("NoCollision");
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(slotPrefab);
        obj.transform.SetParent(transform, false);

        UIInventoryItemSlot itemSlot = obj.GetComponent<UIInventoryItemSlot>();
        slots.Add(itemSlot);
        itemSlot.Set(item);
    }

    void ThrowObject()
    {
        currentHeldObject.GetComponent<Interactable>().canBeInteractedWith = true;
        currentHeldObject.transform.SetParent(null);
        currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
        currentHeldObject.GetComponent<Rigidbody>().AddForce(holdParent.forward * throwForce);
        currentHeldObject.layer = LayerMask.NameToLayer("Default");
        currentHeldObject = null;

        Remove(inventory[currentSelection].data);
    }

    public void Add(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
            value.AddToStack();
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            itemDictionary.Add(referenceData, newItem);
        }

        OnUpdateInventory();
    }

    public void Remove(InventoryItemData referenceData)
    {
        if(itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(referenceData);
            }

            OnUpdateInventory();
        }
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (itemDictionary.TryGetValue(referenceData, out InventoryItem value))
            return value;

        return null;
    }
}

[System.Serializable]
public class InventoryItem
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItem (InventoryItemData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}

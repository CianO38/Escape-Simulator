using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;

    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    public UnityEvent onInventoryChanged;

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
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

        onInventoryChanged.Invoke();
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

            onInventoryChanged.Invoke();
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

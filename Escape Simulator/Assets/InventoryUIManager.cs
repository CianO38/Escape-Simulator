using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject slotPrefab;

    void Start()
    {
        InventorySystem.current.onInventoryChanged.AddListener(OnUpdateInventory);
    }

    private void OnUpdateInventory()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach(InventoryItem item in InventorySystem.current.inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(slotPrefab);
        obj.transform.SetParent(transform, false);

        UIInventoryItemSlot itemSlot = obj.GetComponent<UIInventoryItemSlot>();
        itemSlot.Set(item);
    }
}

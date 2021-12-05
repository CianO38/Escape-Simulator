using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
}

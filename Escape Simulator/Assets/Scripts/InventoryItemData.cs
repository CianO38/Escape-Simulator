using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item data")]
public class InventoryItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
    public GameObject highlightPrefab;
}

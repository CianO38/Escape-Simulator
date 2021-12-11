using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryItemSlot : MonoBehaviour
{
    public Image bg;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI label;

    [SerializeField]
    private GameObject stackObj;

    [SerializeField]
    private TextMeshProUGUI stackLabel;

    public void Set(InventoryItem item)
    {
        icon.sprite = item.data.icon;
        label.text = item.data.displayName;
        if(item.stackSize <= 1)
        {
            stackObj.SetActive(false);
            return;
        }

        stackLabel.text = item.stackSize.ToString();
    }
}

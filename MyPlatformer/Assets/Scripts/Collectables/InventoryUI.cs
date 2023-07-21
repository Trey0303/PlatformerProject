using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI standardPickupText;

    // Start is called before the first frame update
    void Start()
    {
        standardPickupText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateStandardPickupText(PlayerInventory playerInventory)
    {
        standardPickupText.text = playerInventory.NumberOfStandardPickups.ToString();
    }
}

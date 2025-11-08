using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageDisplay : MonoBehaviour
{
    [SerializeField] private string Name;
    [SerializeField] private List<Inventory> Storages;


    [SerializeField] private TMP_Text Text;

    private void OnValidate()
    {
        UpdateAmount();
    }

    private void Awake()
    {
        foreach (Inventory inventory in Storages)
        {
            inventory.OnAmountUpdate.AddListener(UpdateAmount);
        }
    }

    private void UpdateAmount()
    {
        string fullDisplay = Name;

        foreach (Inventory storage in Storages)
        {
            fullDisplay += "\n" + storage.Type + ": " + storage.Amount + (storage.MaxAmount > 0 ? "/" + storage.MaxAmount : "");
        }

        Text.text = fullDisplay;
    }
}

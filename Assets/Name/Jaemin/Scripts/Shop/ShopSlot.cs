using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public ShopItem item;
    public GameObject lockUi;
    private ShopManager shopManager;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
        
        if (IsPurchased())
        {
            lockUi.SetActive(false);
        }

        GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            shopManager.ChangeSelectedItem(item);
        });
    }

    private void Update()
    {
        if (IsPurchased())
        {
            lockUi.SetActive(false);
        }
    }

    private bool IsPurchased()
    {
        string saveKey = "Saved_" + item.name;
        return PlayerPrefs.GetInt(saveKey, 0) == 1;
    }
}
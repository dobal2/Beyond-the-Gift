using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI selectedItemNameText;
    public TextMeshProUGUI selectedItemPriceText;
    public GameObject buyButton;

    public ShopItem currentItem;
    private string saveKey;

    private void Start()
    {
        UpdateUI();
        saveKey = "Saved_" + currentItem.name;
        if (IsPurchaseState())
        {
            buyButton.GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        UpdateBuyButtonState();
    }

    public void ChangeSelectedItem(ShopItem item)
    {
        
        currentItem = item;
        saveKey = "Saved_" + currentItem.name;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentItem == null) return;
        
        selectedItemNameText.text = currentItem.itemName;
        selectedItemPriceText.text = currentItem.price.ToString("N0");

        if (IsPurchaseState())
        {
            UpdateUIForPurchasedState();
        }
        else
        {
            buyButton.GetComponent<Button>().interactable = true;
        }
    }

    private void UpdateBuyButtonState()
    {
        if (currentItem == null)
            return;
        if (IsPurchaseState())
        {
            buyButton.GetComponent<Button>().interactable = false;
            return;
        }

        buyButton.GetComponent<Button>().interactable = GameManager.Instance.GetCookieCount() >= currentItem.price;
    }

    public void BuyItem()
    {
        if (currentItem == null || IsPurchaseState())
            return;

        if (GameManager.Instance.GetCookieCount() >= currentItem.price)
        {
            GameManager.Instance.MinusCookie(currentItem.price);
            PlayerPrefs.SetInt(saveKey, 1);
            PlayerPrefs.Save();

            UpdateUIForPurchasedState();
        }
    }

    private void UpdateUIForPurchasedState()
    {
        buyButton.GetComponent<Button>().interactable = false;
    }

    private bool IsPurchaseState()
    {
        return PlayerPrefs.GetInt(saveKey, 0) == 1;
    }
}

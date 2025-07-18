using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopItensPanel;
    [SerializeField] GameObject confirmPanel;

    [SerializeField] TextMeshProUGUI confirmItemNameTxt;
    [SerializeField] TextMeshProUGUI confirmItemValueTxt;

    [SerializeField] GameObject buttonPrefab;
    [SerializeField] List<ShopItemSO> shopItems;

    private PlayerSkin playerSkin;

    private void Start()
    {
        GenerateItens();
    }

    public void GenerateItens()
    {
        foreach (ShopItemSO item in shopItems)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.GetComponent<ShopItemButton>().Initialize(item, this);
            button.transform.SetParent(shopItensPanel.transform, false);
        }
    }

    public void ToggleShopUI(bool toggle)
    {
        shopUI.gameObject.SetActive(toggle);
    }

    public void ToggleShopPanel(bool toggle)
    {
        ToggleShopUI(toggle);
        shopItensPanel.gameObject.SetActive(toggle);
    }

    public void ToggleConfirmation(bool toggle)
    {
        confirmPanel.gameObject.SetActive(toggle);
    }

    public void CloseShop()
    {
        ToggleShopPanel(true);
        ToggleShopUI(false);
        if (confirmPanel.activeSelf)
            ToggleConfirmation(false);
    }

    public bool BuyItem(ShopItemSO item)
    {
        if (playerSkin == null)
            playerSkin = GameObject.FindFirstObjectByType<PlayerSkin>();

        int value = item.cost;
        if (MoneyManager.Instance.ChargeMoney(value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EquipItem(ShopItemSO item)
    {
        playerSkin.ChangeSkin(item.skinColor, item.necktieColor, item.clothColor);
    }
}

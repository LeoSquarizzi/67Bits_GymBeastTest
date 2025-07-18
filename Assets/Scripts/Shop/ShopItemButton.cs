using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public bool bought;

    [SerializeField] private ShopItemSO itemData;
    [SerializeField] private Image cover;
    [SerializeField] private TextMeshProUGUI costTxt;
    private ShopUI shopUI;

    public void Initialize(ShopItemSO Item, ShopUI _shopUI)
    {
        itemData = Item;
        shopUI = _shopUI;
        name = itemData.name;
        cover.sprite = itemData.cover;

        if (itemData.cost > 0)
        {
            costTxt.text = $"$ {itemData.cost}";
        }
        else
        {
            
            costTxt.text = $"Comprado";
        }
        //transform.localScale = Vector3.one;
    }

    public void OnClick()
    {
        if (!bought)
        {
            bought = shopUI.BuyItem(itemData);
            if(bought)
                costTxt.text = $"Comprado";
        }
        else
            shopUI.EquipItem(itemData);
    }
}

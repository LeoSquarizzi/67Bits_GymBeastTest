using UnityEngine;

public class ShopZone : MonoBehaviour
{
    public ShopUI shopUI;

    private void OnTriggerEnter(Collider other)
    {
        if (shopUI == null)
            shopUI = GameObject.FindGameObjectWithTag("ShopUI").GetComponent<ShopUI>();

        if(other.tag == "Player")
            shopUI.ToggleShopPanel(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            shopUI.CloseShop();
    }
}

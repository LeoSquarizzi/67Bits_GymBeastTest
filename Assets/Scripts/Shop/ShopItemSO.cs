using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject
{
    [Header("Shop Settings")]
    public int cost;
    public Sprite cover;

    [Header("Skin Materials")]
    public Material skinColor;
    public Material clothColor;
    public Material necktieColor;
}

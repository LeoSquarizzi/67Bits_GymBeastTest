using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Renderer skin;
    public Renderer necktie;
    public Renderer clothTop;
    public Renderer clothBot;

    public void ChangeSkin(Material _skin, Material _necktie, Material _cloth)
    {
        skin.material = _skin;
        necktie.material = _necktie;
        clothTop.material = _cloth;
        clothBot.material = _cloth;
    }
}

using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public int life = 1;
    public bool isknockedOut
;
    public float moveSpeed;
    public int coinReward = 1;

    private int timeUntilDisappear = 10;

    public virtual void Move() { }
    public virtual void Punched(int damage) 
    {
        life -= damage;
        if (life <= 0 && !isknockedOut)
        {
            KnockedOut();
        }
    }
    public virtual void KnockedOut() 
    { 
        isknockedOut = true;
    } 
}

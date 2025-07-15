using UnityEngine;

public abstract class NpcBase : MonoBehaviour
{
    public int life = 1;
    public bool isknockedOut
;
    public float moveSpeed;
    public int coinReward = 1;

    private int timeUntilDisappear = 10;

    public abstract void Move();
    public abstract void Punched(int damage);
    public abstract void KnockedOut(); 
}

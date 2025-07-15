using UnityEngine;

public class NpcIdle : NpcBase
{
    public override void KnockedOut()
    {
        Debug.Log("Knocked");
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void Punched(int damage)
    {
        life -= damage;
        if(life <= 0)
        {
            KnockedOut();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

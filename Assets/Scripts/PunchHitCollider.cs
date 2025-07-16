using UnityEngine;

public class PunchHitCollider : MonoBehaviour
{
    [SerializeField] private int punchPower;

    public void ChangePunchPower(int _power)
    {
        punchPower = _power;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NPC")
        {
            other.GetComponent<NpcBase>().Punched(punchPower);
        }
    }
}

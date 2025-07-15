using System.Collections;
using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [Header("Punch Settings")]
    public int punchPower = 1;
    public bool canPunch = true;
    [SerializeField] float punchDelay;

    [Header("Setup")]
    [SerializeField] string requiredTag = "NPC";
    [SerializeField] LayerMask punchableLayer;

    private BoxCollider punchCollider;

    private void Awake()
    {
        if (punchCollider == null)
            punchCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canPunch
            && ((1 << other.gameObject.layer) & punchableLayer) != 0
            && other.CompareTag(requiredTag)
            && !other.GetComponent<NpcBase>().isknockedOut)
        {
            other.gameObject.GetComponent<NpcBase>().Punched(punchPower);
            canPunch = false;
            SetPunchCollider(false);
            StartCoroutine(PunchDelayRoutine());
        }
    }

    IEnumerator PunchDelayRoutine()
    {
        yield return new WaitForSeconds(punchDelay);
        SetPunchCollider(true);
        canPunch = true;
    }

    public void SetPunchCollider(bool toggle)
    {
        punchCollider.enabled = toggle;
    }

    public void ChangePunchPower(int _power)
    {
        punchPower = _power;
    }
}

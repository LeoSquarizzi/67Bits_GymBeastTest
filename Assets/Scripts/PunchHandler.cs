using System.Collections;
using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [Header("Punch Settings")]
    public bool canPunch = true;
    public int punchDamage = 1;
    [SerializeField] float punchPushForce = 1;
    [SerializeField] float punchDelay;


    [Header("Setup")]
    [SerializeField] string requiredTag = "NPC";
    [SerializeField] LayerMask punchableLayer;
    [SerializeField] AnimationClip punchAnimation;

    private BoxCollider punchCollider;
    private PlayerController playerController;

    private void Awake()
    {
        if (punchCollider == null)
            punchCollider = GetComponent<BoxCollider>();
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canPunch
            && ((1 << other.gameObject.layer) & punchableLayer) != 0
            && other.CompareTag(requiredTag)
            && !other.GetComponent<NpcBase>().isknockedOut)
        {

            StartCoroutine(PunchDelayRoutine(other));
        }
    }

    IEnumerator PunchDelayRoutine(Collider target)
    {
        playerController.IsPunching(true);
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        Vector3 punchDirection = (target.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(punchAnimation.length / 5);

        targetRb.isKinematic = false;
        targetRb.useGravity = true;
        targetRb.AddForce(punchDirection * punchPushForce, ForceMode.Impulse);

        yield return new WaitForSeconds(punchAnimation.length / 4);
        playerController.IsPunching(false);
        target.gameObject.GetComponent<NpcBase>().Punched(punchDamage);
        canPunch = false;
        SetPunchCollider(false);

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
        punchDamage = _power;
    }
}

using System.Collections;
using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [Header("Punch Settings")]
    public bool canPunch = true;
    public int punchDamage = 1;
    [SerializeField] float punchPushForce;
    [SerializeField] float punchDelay;

    [Header("Setup")]
    [SerializeField] string requiredTag = "NPC";
    [SerializeField] LayerMask punchableLayer;
    [SerializeField] AnimationClip punchAnimation;

    private BoxCollider punchCollider;
    private PlayerController playerController;
    private AudioSource punchAudio;

    private void Awake()
    {
        if (punchCollider == null)
            punchCollider = GetComponent<BoxCollider>();
        playerController = FindFirstObjectByType<PlayerController>();
        punchAudio = GetComponent<AudioSource>();
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

    public Rigidbody corpo;
    IEnumerator PunchDelayRoutine(Collider target)
    {
        playerController.IsPunching(true);
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        NpcBase npcBase = target.GetComponent<NpcBase>();
        GameObject root = target.gameObject;

        Vector3 punchDirection = (target.transform.position - transform.position).normalized;
        canPunch = false;
        SetPunchCollider(false);
        yield return new WaitForSeconds(punchAnimation.length / 5);
        punchAudio.Play();

        npcBase.ToogleInteractions(false);
        npcBase.Punched(punchDamage, punchDirection, punchPushForce);
        npcBase.UpdateBaseObjectPositionDelayed(2f);
        yield return new WaitForSeconds(punchDelay);

        playerController.IsPunching(false);
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

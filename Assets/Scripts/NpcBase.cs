using System;
using System.Collections;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    [Header("Npc Settings")]
    public int life = 1;
    public float moveSpeed;
    public int coinReward = 1;
    public bool isknockedOut;
    private int timeUntilDisappear = 10;
    [SerializeField] float maxWalkDistance;

    [Header("Animation Settings")]
    public Animator animator;
    public Transform npcRoot;
    public string walkStateName = "Walk";
    public string idleStateName = "Idle";

    private Collider baseCollider;
    CharacterJoint[] joints;
    Rigidbody[] rbodies;
    Collider[] colliders;

    public static event Action<NpcBase> OnKnockedOut;

    private void Start()
    {
        rbodies = npcRoot.GetComponentsInChildren<Rigidbody>();
        joints = npcRoot.GetComponentsInChildren<CharacterJoint>();
        colliders = npcRoot.GetComponentsInChildren<Collider>();
        baseCollider = GetComponent<Collider>();

        foreach (Rigidbody rb in rbodies)
        {
            rb.detectCollisions = false;
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        StartCoroutine(MoveRoutine());
    }

    public void Punched(int damage, Vector3 direction, float force) 
    {
        life -= damage;
        if (life <= 0 && !isknockedOut)
        {
            baseCollider.isTrigger = true;
            ActivateRagdoll();
            npcRoot.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
            KnockedOut();
        }
    }
    public void KnockedOut() 
    { 
        isknockedOut = true;
        OnKnockedOut?.Invoke(this);
        Invoke("DisappearAfterKnockOut", timeUntilDisappear);
    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rb in rbodies)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.useGravity = true;
        }

        ToggleRagdollColliders(true);
    }

    public void DisappearAfterKnockOut()
    {
        Destroy(gameObject);
    }

    public void ToggleRagdollColliders(bool toggle)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = toggle;
        }
    }

    public void ToogleInteractions(bool toggle)
    {
        if (baseCollider == null)
            baseCollider = GetComponent<Collider>();
        baseCollider.enabled = toggle;
    }

    public void UpdateBaseObjectPositionDelayed(float delay)
    {
        Invoke("UpdateBaseObjectPosition", delay);
    }

    public void UpdateBaseObjectPosition()
    {
        npcRoot.transform.parent = null;
        transform.position = npcRoot.transform.position;
        npcRoot.transform.parent = transform;
        ToogleInteractions(true);
    }

    public void OnCollect()
    {
        CancelInvoke("DisappearAfterKnockOut");
        ToggleRagdollColliders(false);
        foreach (Rigidbody rb in rbodies)
        {
            rb.detectCollisions = false;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    IEnumerator MoveRoutine()
    {
        while (!isknockedOut)
        {
            Vector3 randomDistance = UnityEngine.Random.insideUnitSphere * maxWalkDistance;
            randomDistance.y = 0f;
            Vector3 newPos = new Vector3(transform.position.x + randomDistance.x, transform.position.y, transform.position.z + randomDistance.z);

            Quaternion rotateTo = Quaternion.LookRotation(newPos - transform.position);
            while (Quaternion.Angle(transform.rotation, rotateTo) > 1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, 5 * Time.deltaTime);
            }

            animator.Play(walkStateName);
            while (Vector3.Distance(transform.position, newPos) > 0.1f)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                yield return null;
            }

            animator.Play(idleStateName);
            yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 10f));
        }
    }
}

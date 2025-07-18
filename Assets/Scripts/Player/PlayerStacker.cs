using System.Collections.Generic;
using UnityEngine;

public class PlayerStacker : MonoBehaviour
{
    [Header("Stack Settings")]
    public int maxStack;
    [SerializeField] float bodiesGap = 1;
    [SerializeField] float stackSpeed = 5;

    [Header("Setup")]
    [SerializeField] string requiredTag = "NPC";
    [SerializeField] LayerMask punchableLayer;

    [Header("Run-time")]
    [SerializeField] private List<GameObject> stackedNpcs;
    Transform stackHolder;

    private void Start()
    {
        stackedNpcs = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & punchableLayer) != 0
            && other.CompareTag(requiredTag)
            && other.GetComponent<NpcBase>().isknockedOut)
        {
            if(stackedNpcs.Count < maxStack)
            {
                AddBodyToStack(other.gameObject);
            }
        }

        if (other.CompareTag("DeliveryArea"))
        {
            if (stackedNpcs.Count > 0)
            {
                RemoveAllFromList(other.GetComponent<DeliveryZone>());
            }
        }
    }

    public void AddBodyToStack(GameObject body)
    {
        if (stackedNpcs.Count < maxStack)
        {
            if (stackHolder == null)
                stackHolder = new GameObject("StackList Holder").transform;

            Rigidbody rb = body.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            body.GetComponent<Collider>().enabled = false;
            Vector3 lastBody = GetLastBodyPosition();
            Vector3 stackPos = new Vector3(transform.position.x, lastBody.y + bodiesGap, transform.position.z); 
            body.transform.position = stackPos;
            body.GetComponent<NpcBase>().OnCollect();
            body.transform.parent = stackHolder;
            stackedNpcs.Add(body);
        }
        else
        {
            Debug.Log("Você não consegue carregar mais corpos");
        }
    }

    public Vector3 GetLastBodyPosition()
    {
        if (stackedNpcs.Count > 0)
            return stackedNpcs[stackedNpcs.Count - 1].transform.position;
        else 
            return transform.position;
    }

    private void Update()
    {
        StackPositionUpdate();
    }

    private void StackPositionUpdate()
    {
        for (int i = 0; i < stackedNpcs.Count; i++)
        {
            Vector3 targetPos;
            float bodySpeed = stackSpeed;

            if (i == 0)
            {
                targetPos = new Vector3(transform.position.x, stackedNpcs[0].transform.position.y, transform.position.z);
                stackedNpcs[i].transform.position = targetPos;
                continue;
            }
            else
            {
                bodySpeed = bodySpeed / i;
                targetPos = new Vector3(stackedNpcs[i - 1].transform.position.x, stackedNpcs[i].transform.position.y, transform.position.z);
            }
            stackedNpcs[i].transform.position = Vector3.Lerp(stackedNpcs[i].transform.position, targetPos, Time.deltaTime * bodySpeed);
        }
    }

    public void RemoveAllFromList(DeliveryZone _zone)
    {
        _zone.ReceiveBodyStack(stackedNpcs, transform);
        stackedNpcs = new List<GameObject>();
    }

    public void IncreaseStackListMax(int slotsToAdd)
    {
        maxStack += slotsToAdd;
    }
}

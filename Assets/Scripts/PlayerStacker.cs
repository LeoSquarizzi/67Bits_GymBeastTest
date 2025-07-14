using NUnit.Framework;
using System.Collections.Generic;

using System.ComponentModel;
using UnityEngine;

public class PlayerStacker : MonoBehaviour
{
    public int maxStack;
    //public int currentStack;
    [SerializeField] private List<GameObject> stackedNpcs;

    float bodiesGap;

    public void AddBodyToStack(GameObject body)
    {
        if (stackedNpcs.Count < maxStack)
        {
            Vector3 lastBody = GetLastBodyPosition();
            Vector3 stackPos = new Vector3(transform.position.x, lastBody.y + bodiesGap, transform.position.z); 
            body.transform.position = stackPos;
            transform.parent = transform;
            stackedNpcs.Add(body);
        }
        else
        {
            Debug.Log("Você não consegue carregar mais corpos");
        }
    }

    public Vector3 GetLastBodyPosition()
    {
        return stackedNpcs[stackedNpcs.Count - 1].transform.position;
    }

    public void RemoveAllFromList(DeliveryZone _zone)
    {

    }
}

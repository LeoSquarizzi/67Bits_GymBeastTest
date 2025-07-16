using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [SerializeField] float deliveryDuration = 0.3f;
    [SerializeField] float timeBeforeDestroy = 3f;

    private GameObject stackedContainer;
    private PlayerController player;

    public void ReceiveBodyStack(List<GameObject> bodyStack, Transform _deliveryBy)
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        player.TogglePlayerController(false);
        StartCoroutine(BodiesToZone(bodyStack, _deliveryBy));
    }

    private IEnumerator BodiesToZone(List<GameObject> bodyStack, Transform _deliveryBy)
    {
        Renderer rend = GetComponent<Renderer>();
        stackedContainer = new GameObject("Stack Container");
        bodyStack.Reverse();

        foreach (GameObject body in bodyStack)
        {
            Vector3 randomPos = new Vector3(Random.Range(rend.bounds.min.x + 0.3f, rend.bounds.max.x - 0.3f), rend.bounds.center.y, Random.Range(rend.bounds.min.z + 0.3f, rend.bounds.max.z - 0.3f));
            body.transform.parent = stackedContainer.transform;
            float time = 0f;

            while (time < deliveryDuration)
            {
                float t = time / deliveryDuration;
                body.transform.position = Vector3.Lerp(body.transform.position, randomPos, t);
                time += Time.deltaTime;
                yield return null;
            }

            body.transform.position = randomPos;
        } 
        
        yield return null;
        player.TogglePlayerController(true);
        Destroy(stackedContainer, timeBeforeDestroy);
    }
}

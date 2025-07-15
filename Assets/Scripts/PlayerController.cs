using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private bool canMove = true;
    private Transform currentTarget;

    private Vector2 stickInputValue;
    private Transform camTransform;
    private CharacterController characController;
    //private Animator animator;

    private void Awake()
    {
        characController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>(); 
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (canMove)
        {
            RotateCharacterFromCameraView();
            characController.Move(transform.forward * stickInputValue.magnitude * moveSpeed * Time.deltaTime);
        }

        //characController.Move(Vector3.down * 9.81f * Time.deltaTime);
        //animator.SetBool("andar", stickInputValue != Vector2.zero);
    }

    public void Move(InputAction.CallbackContext value)
    {
        stickInputValue = value.ReadValue<Vector2>();
    }

    private void RotateCharacterFromCameraView()
    {
        Vector3 camForward = camTransform.TransformDirection(Vector3.forward);
        Vector3 camRight = camTransform.TransformDirection(Vector3.right);
        Vector3 targetDirection = stickInputValue.x * camRight + stickInputValue.y * camForward;

        if (stickInputValue != Vector2.zero && targetDirection.magnitude > 0.1f) 
        {
            Quaternion freeRotation = Quaternion.LookRotation(targetDirection.normalized);
            Quaternion targetRotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, freeRotation.eulerAngles.y, transform.eulerAngles.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    public void TogglePlayerController(bool toggle)
    {
        canMove = toggle;
    }
}

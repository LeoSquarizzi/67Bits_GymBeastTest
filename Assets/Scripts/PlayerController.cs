using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isPunching;
    private Transform currentTarget;

    private Vector2 stickInputValue;
    private Transform camTransform;
    private CharacterController characController;
    public Animator animator;

    private string walkState = "Walk";
    private string idleState = "Idle";
    private string punchState = "Punch";

    private void Awake()
    {
        if(characController == null)
            characController = GetComponent<CharacterController>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (canMove)
        {
            if (isPunching)
                animator.Play(punchState);
            else if (!animator.GetCurrentAnimatorStateInfo(0).IsName(walkState) && stickInputValue != Vector2.zero)
                animator.Play(walkState);           
            else if(stickInputValue == Vector2.zero)
                animator.Play(idleState);

            RotateCharacterFromCameraView();
            characController.Move(transform.forward * stickInputValue.magnitude * moveSpeed * Time.deltaTime);
        }
        else
            animator.Play(idleState);
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

    public void IsPunching(bool toggle)
    {
        isPunching = toggle;
    }
}

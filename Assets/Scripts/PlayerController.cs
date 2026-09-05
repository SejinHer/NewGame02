using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputActions inputActions;
    private Vector2 moveInput;

    public float moveSpeed;

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }


    void Awake()
    {
        inputActions = new InputActions();
    }

    void Start()
    {
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
    }

    void Update()
    {
        Vector3 dir = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
    }
}

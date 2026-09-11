using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    public Rigidbody rb;
    public SpriteRenderer sr;
    private Vector3 moveDir;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        moveAction = inputActions != null
            ? inputActions.FindActionMap("Player")?.FindAction("Move")
            : null;
    }

    void OnEnable()
    {
        moveAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
    }

    void Update()
    {
        if (moveAction == null)
            return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        float moveHorizontal = input.x;
        float moveVertical = input.y;
        moveDir = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (moveHorizontal < 0f)
        {
            sr.flipX = true;
        }
        else if (moveHorizontal > 0f)
        {
            sr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = moveDir * speed;
    }
}

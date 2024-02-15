using UnityEngine;

public class Player : MonoBehaviour
{
    public bool freezePlayer;

    KeyboardInput playerInput;

    [Header("Camera")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform cameraPivot;
    private Camera playerCamera;

    [SerializeField] private float XSensitivity;
    [SerializeField] private float YSensitivity;

    [SerializeField] private float VerticalTiltTopLimit = 40f;
    [SerializeField] private float VerticalTiltBottomLimit = -60f;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5;
    private float currentSpeed;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] PlayerState playerState;
    public enum PlayerState
    {
        Idle,
        Walking,
        Running,
        Falling,
        Dashing,
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerInput = new KeyboardInput();
        playerInput.Player.Enable();
    }

    private void Start()
    {
        SetupCamera();
    }

    void SetupCamera()
    {
        playerCamera = Camera.main;
        playerCamera.transform.parent = cameraPivot;
        playerCamera.transform.position = cameraTarget.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        Cursor.lockState = CursorLockMode.Locked;

        if (!freezePlayer)
        {
            if (playerInput.Player.Walk != null)
            {
                MovePlayer(GetMoveDirection());
            }

            MoveCamera();

        }
    }

    #region camera
    public float xRot = 0f;
    private void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * XSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * YSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, VerticalTiltBottomLimit, VerticalTiltTopLimit);

        cameraPivot.transform.position = Vector3.MoveTowards(cameraPivot.transform.position, cameraTarget.position, 50 * Time.deltaTime);

        cameraTarget.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        cameraPivot.transform.localRotation = cameraTarget.localRotation;
        transform.Rotate(Vector3.up * mouseX);
    }

    #endregion

    #region movement
    private Vector3 GetMoveDirection()
    {
        Vector2 input = playerInput.Player.Walk.ReadValue<Vector2>();
        input.Normalize();

        Vector3 rawDirection = new()
        {
            x = input.x,
            y = 0,
            z = input.y
        };

        if (rawDirection == Vector3.zero)
        {
            playerState = PlayerState.Idle;
        }
        else
        {
            playerState = PlayerState.Walking;
        }

        Vector3 adjustedDirection = transform.right * rawDirection.x + transform.forward * rawDirection.z;
        return adjustedDirection;
    }

    // checks if the player is on solid ground
    private bool CheckGrounded()
    {
        //RaycastHit hit;

        if (Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundMask))
        {
            Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.green);

            return true;
        }
        else
        {
            Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance, Color.red);

            return false;
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        HandleSpeed();
        Debug.Log(direction);
        transform.position += (direction * currentSpeed * Time.deltaTime);
    }

    private void HandleSpeed()
    {
        float targetSpeed = 0;
        float accelerationSpeed = 10;
        float decelerationSpeed = 15;

        switch (playerState)
        {
            case PlayerState.Walking:
                targetSpeed = walkSpeed;
                break;
            case PlayerState.Falling:
                targetSpeed = walkSpeed;
                break;
            case PlayerState.Idle:
                targetSpeed = 0;
                break;
        }

        if (currentSpeed < targetSpeed)
        {
            currentSpeed += Time.deltaTime * accelerationSpeed;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= Time.deltaTime * decelerationSpeed;
        }
    }
    #endregion
}

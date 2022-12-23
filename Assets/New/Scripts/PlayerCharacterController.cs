using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;
    private static readonly int IsWalkingForward = Animator.StringToHash("IsWalkingForward");
    private static readonly int IsWalkingBackward = Animator.StringToHash("IsWalkingBackward");    
    private static readonly int IsWalkingLeft = Animator.StringToHash("IsWalkingLeft");
    private static readonly int IsWalkingRight = Animator.StringToHash("IsWalkingRight");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");

    [Header("Movement")]
    [SerializeField] private Transform toFollow;
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 8;
    [SerializeField] private float acceleration = 0.5f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField]private float dampingRotation = 10f;
    [ReadOnly][SerializeField]private float velocity;
    private float _actualSpeed;
    private Rigidbody _rb;
    
    [Header("Camera Rotation")]
    [SerializeField] private float dampingCamera = 10f;
    [SerializeField] private float speedX = 80f;
    private Vector2 _cameraRotate;
    
    private bool _isRunning;
    private bool _inMotion;
    private bool _isJumping;
    private float _verticalInput;
    private float _horizontalInput;
    //private Vector2 _inputDirection;
    private float _xRotation;
    private Vector3 _moveDirection;

    [Header("Ground Check")]
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private float groundDrag;
    [SerializeField] private float offsetGround = 0.15f;
    [ReadOnly][SerializeField]private float playerHeight;
    [ReadOnly][SerializeField]private bool grounded;
    
    [Header("Jump")]
    [SerializeField] private float impulse = 10f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        playerHeight = GetComponent<CapsuleCollider>().height;
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayer();
        CheckGrounded();
        
        RotateTargetForCamera();
        AnimationBehavior();
    }

    private void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position+(playerHeight/2)*Vector3.up, Vector3.down, playerHeight * 0.5f + offsetGround, groundLayer);


        if (grounded)
        {
            _rb.drag = groundDrag;
        }
        else
            _rb.drag = 0;
        
    }
    /*
    
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    */
    
    private void FixedUpdate()
    {
        UpdateVelocity();
        MovePlayer();
        RotatePlayer();
    }

    private void UpdateVelocity()
    {
        if ((_inMotion && velocity < walkSpeed / runSpeed)|| (_isRunning && velocity < 1.0f))
        {
            velocity += Time.deltaTime * acceleration;
            velocity = Input.GetButton("Run") ? Mathf.Min(velocity, 1) : Mathf.Min(velocity, walkSpeed / runSpeed);
        }
        else if((_inMotion == false && velocity > 0.0f) || (_isRunning == false && _actualSpeed > walkSpeed))
        {
            velocity -= Time.deltaTime * deceleration;
            velocity = Mathf.Max(velocity, 0);
        }

        _actualSpeed = velocity*runSpeed;
    }

    private void RotateTargetForCamera()
    {
        toFollow.rotation = Quaternion.Slerp(toFollow.rotation,Quaternion.Euler(0, _xRotation, 0),dampingCamera);
    }
    
    void InputPlayer()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        
        _inMotion = _horizontalInput != 0 || _verticalInput != 0;

        _isRunning = Input.GetButton("Run");
        
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * speedX;
        _xRotation += mouseX;

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Jump()
    {
        /*
        _isJumping = true;
        if(grounded)
        {
            print("ui");
            //characterAnimator.SetBool(IsJumping, true);
            _rb.AddForce(new Vector3(0,impulse,0), ForceMode.Impulse);
        }
        */
    }
    void MovePlayer()
    {
        if (_inMotion)
        {
            _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
        }

        if (grounded)
            _rb.AddForce(_actualSpeed * _moveDirection.normalized,ForceMode.VelocityChange);
    }

    void RotatePlayer()
    {
        if (_moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * dampingRotation);
        }
    }

    private void AnimationBehavior()
    {
        characterAnimator.SetBool(IsWalkingForward, _verticalInput > 0 && _horizontalInput == 0);
        characterAnimator.SetBool(IsWalkingBackward, _verticalInput < 0 && _horizontalInput == 0);
        
        characterAnimator.SetBool(IsWalkingRight, _horizontalInput > 0);
        characterAnimator.SetBool(IsWalkingLeft, _horizontalInput < 0);

        characterAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        
        characterAnimator.SetFloat(VelocityHash,velocity);
    }
}


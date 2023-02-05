using System;
using System;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerCharacterController : CharacterControllerBase
{
    [Header("Animation")]
    [ReadOnly][SerializeField] private Animator characterAnimator;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

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
    [SerializeField] private float lockCamY = 35f;
    private Vector2 _cameraRotate;
    
    [Header("Debug movement")]
    [ReadOnly][SerializeField]private bool _isRunning;
    private bool _inMotion;
    [ReadOnly][SerializeField]private bool _isJumping;
    [ReadOnly][SerializeField]private float _verticalInput;
    [ReadOnly][SerializeField]private float _horizontalInput;
    //private Vector2 _inputDirection;
    private float _xRotation;
    private float _yRotation;
    private Vector3 _moveDirection;
    private CapsuleCollider _capsuleCollider;

    [Header("Ground Check")]
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private float groundDrag;
    [SerializeField] private float groundDistanceMax = 0.2f;
    [ReadOnly][SerializeField]private float playerHeight;
    [ReadOnly][SerializeField]private bool grounded;
    [SerializeField] private Transform foot;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airDrag;
    [ReadOnly][SerializeField] private bool readyToJump;
    [SerializeField] private float updateColliderSpeedUp;
    [SerializeField] private float updateColliderSpeedDown;
    [SerializeField] private float centerJumpCollider;
    [ReadOnly] [SerializeField] private float originCenterCollider;
    [SerializeField] private float jumpStartTime = 0.3f;
    [ReadOnly][SerializeField] private float jumpStartCounter = 0f;

    [Header("Step Climb")]
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private float distanceFirstStep = 0.3f;
    [SerializeField] private float distanceSecondStep = 0.45f;

    [Header("Collisions")] [ReadOnly] [SerializeField]
    private int collisions;
    public bool bague = false;
    public int orb = 0;

    public bool Grounded
    {
        get => grounded;
        set => grounded = value;
    }

    private Vector3 _rigidbodyDrag;
    void Start()
    {
        characterAnimator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        _capsuleCollider = GetComponent<CapsuleCollider>();
        playerHeight = _capsuleCollider.height;
        originCenterCollider = _capsuleCollider.center.y;
        ResetJump();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepRayUpper.transform.position.y+stepHeight, stepRayUpper.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayer();
        CheckGrounded();
        RotateTargetForCamera(_xRotation,_yRotation);
        AnimationBehavior();
        //UpdateSizeCapsuleCollision();
    }

    private void CheckGrounded()
    {
        
        Vector3 position = transform.position;
        //grounded = Physics.Raycast(position, Vector3.down, groundDistanceMax);
        //grounded = Physics.CheckSphere(position, groundDistanceMax);
        Collider[] hitColliders = Physics.OverlapBox(position, new Vector3(groundDistanceMax,groundDistanceMax,groundDistanceMax));
        //Collider[] hitColliders = Physics.OverlapSphere(position, groundDistanceMax);
        grounded = hitColliders.Length > 1 ;
        //grounded = Physics.CheckSphere(position, groundDistanceMax);
        Gizmos.color = Color.red;
        
        
        //Vector3 position = transform.position + (playerHeight / 2) * Vector3.up;
        //float groundDistanceMax = playerHeight * 0.5f + offsetGround;
        //grounded = Physics.Raycast(position, Vector3.down, groundDistanceMax, groundLayer);

        if (jumpStartTime < jumpStartCounter)
        {
            _isJumping = false;
        }
        else if(jumpStartTime > jumpStartCounter)
        {
            jumpStartCounter += Time.deltaTime;
        }
        //_isJumping = !grounded;


    }
    
    void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(position,groundDistanceMax);
        Gizmos.DrawCube(position,new Vector3(groundDistanceMax,groundDistanceMax,groundDistanceMax));
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
        if(_inMotion)
            RotatePlayer(_moveDirection);
        StepClimb();
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

    public void RotateTargetForCamera(float xRot,float yRot)
    {
        toFollow.rotation = Quaternion.Slerp(toFollow.rotation,Quaternion.Euler(yRot, xRot,0),dampingCamera);
    }
    
    void InputPlayer()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        
        _inMotion = _horizontalInput != 0 || _verticalInput != 0;

        _isRunning = Input.GetButton("Run");
        
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * speedX;
        _xRotation += mouseX;
        
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * speedX;
        _yRotation = MathF.Abs(_yRotation) < lockCamY ? _yRotation - mouseY : _yRotation;
        _yRotation = _yRotation > lockCamY && mouseY > 0? _yRotation - mouseY : _yRotation; 
        _yRotation = _yRotation < - lockCamY && mouseY < 0? _yRotation - mouseY : _yRotation;
        //_yRotation = MathF.Abs(_yRotation - mouseY) < lockCamY ? _yRotation - mouseY : _yRotation;

        /*
        if (Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
        */
    }

    public void Jump()
    {
        //_rb.velocity = new Vector3(_rb.velocity.x, 0f,_rb.velocity.z);
        _isJumping = true;
        jumpStartCounter = 0;
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(transform.up*jumpForce, ForceMode.VelocityChange);

        /*
        if(grounded)
        {
            print("ui");
            //characterAnimator.SetBool(IsJumping, true);
            _rb.AddForce(new Vector3(0,impulse,0), ForceMode.Impulse);
        }
        */
    }

    void ResetJump()
    {
        readyToJump = true;
    }
    /*
    void MovePlayer()
    {
        _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;

        if (_inMotion)
        {
            _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
            _moveDirection.y = 0;
        }

        RaycastHit hit;
        // Check if the body's current velocity will result in a collision
        if (_rb.SweepTest(_moveDirection, out hit, 0.1f))
        {
            // Nuke horizontal velocity
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            return;
        }

        _rb.AddForce(_actualSpeed * _moveDirection.normalized,ForceMode.VelocityChange);
        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);
    }
    */
    
    void MovePlayer()
    {
        if (_inMotion)
        {
            _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
            _moveDirection.y = 0;
        }
        
        _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;

        if (!grounded)
        {
            _rigidbodyDrag += -_rb.velocity*airDrag;

        }
        if(collisions == 0 || grounded)
            _rb.AddForce(_actualSpeed * _moveDirection.normalized,ForceMode.VelocityChange);
        
        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);

    }

    void OnCollisionEnter(Collision collision)
    {
        collisions++;
    }

    void OnCollisionExit(Collision collision)
    {
        collisions--;
    }
    
    public void RotatePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingRotation);
        }
    }
    
    public void RotatePlayer(Quaternion angle)
    {
        if (angle.eulerAngles != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * dampingRotation);
        }
    }

    private void AnimationBehavior()
    {
        if (!characterAnimator)
        {
            Debug.LogWarning("No Animator Character on "+name);
            return;  
        }
        
        characterAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
        characterAnimator.SetFloat(VelocityHash,velocity);
        characterAnimator.SetBool(IsGrounded, grounded);
        characterAnimator.SetBool(IsJumping, _isJumping);
        characterAnimator.SetBool(IsFalling, !grounded);
    }

    private void StepClimb()
    {
        if (!grounded || !_inMotion)
            return;
        
        RaycastHit hitLower;
        
        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward).normalized*distanceFirstStep, Color.green);
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, distanceFirstStep))
        {
            Debug.DrawRay(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward).normalized*distanceSecondStep, Color.red);
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, distanceSecondStep))
            {
                //_rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                _rb.position = Vector3.Lerp(_rb.position,_rb.position-new Vector3(0f, -stepSmooth * Time.deltaTime, 0f), Time.deltaTime);
               //_rb.AddForce(new Vector3(0f, stepSmooth, 0f),ForceMode.Acceleration);
               return;
            }
        }

        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1).normalized*distanceFirstStep, Color.blue);
        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1), out hitLower45, distanceFirstStep))
        {
            Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1).normalized*distanceSecondStep, Color.magenta);
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f,0,1), out hitUpper45, distanceSecondStep))
            {
                _rb.position = Vector3.Lerp(_rb.position,_rb.position-new Vector3(0f, -stepSmooth * Time.deltaTime, 0f), Time.deltaTime);
                //_rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                //_rb.AddForce(new Vector3(0f, stepSmooth, 0f),ForceMode.Acceleration);
                return;
            }
        }

        Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1).normalized*distanceFirstStep, Color.blue);
        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, distanceFirstStep))
        {
            RaycastHit hitUpperMinus45;
            Debug.DrawRay(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1).normalized*distanceSecondStep, Color.magenta);
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, distanceSecondStep))
            {
                _rb.position = Vector3.Lerp(_rb.position,_rb.position-new Vector3(0f, -stepSmooth * Time.deltaTime, 0f), Time.deltaTime);
                //_rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                //_rb.AddForce(new Vector3(0f, stepSmooth, 0f),ForceMode.Acceleration);
                return;
            }
        }
    }
    
    
}


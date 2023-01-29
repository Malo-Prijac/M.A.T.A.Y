using System;
using System;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
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

    [Header("Jump frames")]
    [ReadOnly] [SerializeField] private float _frameJump = 0;
    
    [SerializeField] private float startFrame;
    [SerializeField] private float transitionFrame;
    [SerializeField] private float endFrame;

    public bool bague = false;


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
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayer();
        CheckGrounded();
        RotateTargetForCamera();
        AnimationBehavior();
        //UpdateSizeCapsuleCollision();
    }

    private void CheckGrounded()
    {
        
        Vector3 position = transform.position;
        //print(position);
        grounded = Physics.Raycast(position, Vector3.down, groundDistanceMax);
        //grounded = Physics.CheckSphere(position, groundDistanceMax);
        //print(grounded);
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
        Gizmos.DrawSphere(position,groundDistanceMax);
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

    private void RotateTargetForCamera()
    {
        toFollow.rotation = Quaternion.Slerp(toFollow.rotation,Quaternion.Euler(_yRotation, _xRotation,0 ),dampingCamera);
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
        _yRotation -= mouseY;

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
    
    
    void MovePlayer()
    {
        if (_inMotion)
        {
            _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
            _moveDirection.y = 0;
        }
        
        if (grounded)
        {
            _rb.AddForce(_actualSpeed * _moveDirection.normalized,ForceMode.VelocityChange);
            
            _rigidbodyDrag = new Vector3(-_rb.velocity.x, 0, -_rb.velocity.z)*groundDrag;
        }

        if (!grounded)
        {
            _rigidbodyDrag = -_rb.velocity*airDrag;
            _rb.AddForce(_rigidbodyDrag*groundDrag, ForceMode.Acceleration);

        }
        
        _rb.AddForce(_rigidbodyDrag, ForceMode.Acceleration);

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


        //characterAnimator.SetBool(IsJumping, _isJumping);

        //characterAnimator.SetBool(IsJumping, _isJumping);
        
    }

    private void MoveUpCapsuleCollision()
    {
        Vector3 capsuleCenter = _capsuleCollider.center;
        if (_capsuleCollider.height > playerHeight / 1.5)
        {
            float updatedCapsuleHeight = _capsuleCollider.height - Time.deltaTime * updateColliderSpeedUp;
            _capsuleCollider.height = updatedCapsuleHeight;
        }

        //print("ok");

        if (capsuleCenter.y < centerJumpCollider) //capsuleCenter.y <= originCenterCollider)
        {
            float updatedCapsuleCenter = _capsuleCollider.center.y + Time.deltaTime * updateColliderSpeedUp;

            _capsuleCollider.center = new Vector3(capsuleCenter.x, updatedCapsuleCenter, capsuleCenter.z);
        }

        //flameCollider.center = new Vector3(initFlameCenter.x,initFlameCenter.y,upgradedFlameSize/2);

    }
    
    private void MoveDownCapsuleCollision()
    {
        Vector3 capsuleCenter = _capsuleCollider.center;
        if (_capsuleCollider.height < playerHeight)
        {
            float updatedCapsuleHeight = _capsuleCollider.height + Time.deltaTime * updateColliderSpeedDown;

            _capsuleCollider.height = updatedCapsuleHeight;
        }
        else
        {
            _capsuleCollider.height = playerHeight;
        }

        if (capsuleCenter.y > originCenterCollider) //capsuleCenter.y <= originCenterCollider)
        {
            float updatedCapsuleCenter = _capsuleCollider.center.y - Time.deltaTime * updateColliderSpeedDown;

            _capsuleCollider.center = new Vector3(capsuleCenter.x, updatedCapsuleCenter, capsuleCenter.z);
        }
        else
        {
            _capsuleCollider.center = new Vector3(capsuleCenter.x, originCenterCollider, capsuleCenter.z);
        }

    }
    /*
    private void UpdateSizeCapsuleCollision()
    {

        if(_jumpStarted && !grounded){
            
        _frameJump += Time.deltaTime*30 ;
            
            //jusqu'à 0.15
            if (startFrame < _frameJump && _frameJump < transitionFrame ) // || _capsuleCollider.height > playerHeight/1.5)//flameCollider.size.z < initFlameCenter.z * stats.range && target)
            {
                MoveUpCapsuleCollision();
            }
            
            //jusqu'à la fin
            else if (transitionFrame < _frameJump && _frameJump < endFrame)
            {
                MoveDownCapsuleCollision();
            }
            
            else if (_frameJump > endFrame)
            {
                _jumpStarted = false;
                _frameJump = 0;
            }
        }

    }*/

    private void StepClimb()
    {
        if (!_inMotion)
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


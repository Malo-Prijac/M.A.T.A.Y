using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmove : MonoBehaviour
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
    
    [Header("Debug movement")]
    [ReadOnly][SerializeField]private bool _isRunning;
    private bool _inMotion;
    [ReadOnly][SerializeField]private bool _isJumping;
    [ReadOnly][SerializeField]private float _verticalInput;
    [ReadOnly][SerializeField]private float _horizontalInput;
    //private Vector2 _inputDirection;
    private float _xRotation;
    private Vector3 _moveDirection;
    private CapsuleCollider _capsuleCollider;

    [Header("Ground Check")]
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private float groundDrag;
    [SerializeField] private float offsetGround = 0.15f;
    [ReadOnly][SerializeField]private float playerHeight;
    [ReadOnly][SerializeField]private bool grounded;
    [SerializeField] private Transform foot;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [ReadOnly][SerializeField] private bool readyToJump;
    [SerializeField] private float updateColliderSpeedUp;
    [SerializeField] private float updateColliderSpeedDown;
    [SerializeField] private float centerJumpCollider;
    [ReadOnly] [SerializeField] private float originCenterCollider;
    private bool _jumpStarted = false;
    
    [Header("Jump frames")]
    [ReadOnly] [SerializeField] private float _frameJump = 0;
    
    [SerializeField] private float startFrame;
    [SerializeField] private float transitionFrame;
    [SerializeField] private float endFrame;


    private CharacterController _controller;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayer();
        //CheckGrounded();
        RotateTargetForCamera();
        AnimationBehavior();
        //UpdateSizeCapsuleCollision();

        UpdateVelocity();
        MovePlayer();
        RotatePlayer();
    }

    private void FixedUpdate()
    {


    }

    private void RotateTargetForCamera()
    {
        toFollow.rotation = Quaternion.Slerp(toFollow.rotation,Quaternion.Euler(0, _xRotation, 0),dampingCamera);
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
    
    void InputPlayer()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        
        _inMotion = _horizontalInput != 0 || _verticalInput != 0;

        _isRunning = Input.GetButton("Run");
        
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * speedX;
        _xRotation += mouseX;

        if (Input.GetButton("Jump") && readyToJump && grounded)
        {
            readyToJump = false;
            //Jump();
            //Invoke(nameof(ResetJump),jumpCooldown);
        }
    }
    
    void MovePlayer()
    {
        if (_inMotion)
        {
            _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
            _controller.Move(_actualSpeed*Time.deltaTime*_moveDirection);

        }

        /*
        if (grounded)
            _rb.AddForce(_actualSpeed * _moveDirection.normalized,ForceMode.VelocityChange);
        
        if(!grounded)
            _rb.AddForce(_actualSpeed * _moveDirection.normalized * airMultiplier,ForceMode.Force);
        
        */
    }
    
    void RotatePlayer()
    {
        if (_moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(_moveDirection);
            transform.parent.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * dampingRotation);
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
        
        characterAnimator.SetBool(IsJumping, _isJumping);
    }
}

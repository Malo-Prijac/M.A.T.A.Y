using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SAVE_PlayerCharacterController : MonoBehaviour
{
   [SerializeField] private float walkSpeed = 4;
   [SerializeField] private float runSpeed = 8;
   [SerializeField] private float rotationSpeedX = 10;

    private float _actualSpeed = 0;
    private Rigidbody _rb;
    private Vector2 _cameraRotate;
    
    private float _verticalInput;
    private float _horizontalInput;
    //private Vector2 _inputDirection;
    private float _xRotation;

    private Vector3 _moveDirection;
    
    
    
    
    /*
    public float rotationSpeedY = 10;
    public float playerHeight;
    public LayerMask groundLayer;
    
    bool grounded;
    public float groundDrag;
    */

    //[SerializeField] private float _jumpForce = 300;
    // Start is called before the first frame update

    [SerializeField] private Animator characterAnimator;
    private static readonly int IsWalkingForward = Animator.StringToHash("IsWalkingForward");
    private static readonly int IsWalkingBackward = Animator.StringToHash("IsWalkingBackward");    
    private static readonly int IsWalkingLeft = Animator.StringToHash("IsWalkingLeft");
    private static readonly int IsWalkingRight = Animator.StringToHash("IsWalkingRight");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    
    
    [SerializeField] private Transform toFollow;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        InputPlayer();
        /*
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
        if (grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
        */

        RotateTarget();
        AnimationBehavior();
    }
    
    private void FixedUpdate()
    {
        if (_horizontalInput == 0 && _verticalInput == 0)
            return;

        if (Input.GetButton("Run"))
            _actualSpeed = runSpeed;
        else
            _actualSpeed = walkSpeed;
        
        MovePlayer();
        RotatePlayer();
    }

    private void RotateTarget()
    {
        float damping = 10;
        toFollow.transform.rotation = Quaternion.Slerp(toFollow.transform.rotation,Quaternion.Euler(0, _xRotation, 0),damping);
    }
    
    void InputPlayer()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * rotationSpeedX;
        _xRotation += mouseX;
    }
    void MovePlayer()
    {
        _moveDirection = toFollow.transform.forward * _verticalInput + toFollow.right * _horizontalInput;
        _rb.velocity = _actualSpeed * _moveDirection.normalized;
    }

    void RotatePlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(_moveDirection);
        float damping = 10;
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void AnimationBehavior()
    {
        characterAnimator.SetBool(IsWalkingForward, _verticalInput > 0 && _horizontalInput == 0);
        characterAnimator.SetBool(IsWalkingBackward, _verticalInput < 0 && _horizontalInput == 0);
        
        characterAnimator.SetBool(IsWalkingRight, _horizontalInput > 0);
        characterAnimator.SetBool(IsWalkingLeft, _horizontalInput < 0);

        characterAnimator.SetBool(IsRunning, Mathf.Approximately(_actualSpeed,runSpeed) && (_verticalInput != 0 || _horizontalInput != 0));
    }
}


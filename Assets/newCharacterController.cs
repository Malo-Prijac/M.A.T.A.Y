using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCharacterController : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    private Rigidbody _rb;
    private Vector2 cameraRotate;
    private float verticalInput;
    private float horizontalInput;
    
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;
    public float groundDrag;
    
    //[SerializeField] private float _jumpForce = 300;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);
        InputPlayer();
        if (grounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void InputPlayer()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }
    void MovePlayer()
    {
        //Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        _rb.AddForce(10f*_speed*moveDirection.normalized,ForceMode.VelocityChange);
    }
}

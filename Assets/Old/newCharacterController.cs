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

    public float rotationSpeedX = 10;
    public float rotationSpeedY = 10;
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;
    public float groundDrag;

    private float xRotation = 0;

    private Vector2 inputDirection;
    //[SerializeField] private float _jumpForce = 300;
    // Start is called before the first frame update

    [SerializeField] private Transform toFollow;
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
        
        
        
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputDirection = inputs.normalized;
        
        
        Vector3 rotationSpeed = new Vector3(0, rotationSpeedX, 0);
        Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotationSpeed*Time.deltaTime );
        
        //transform.Rotate(deltaRotation.eulerAngles);

        /*
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotationSpeedX;
        xRotation += mouseX;
        print(xRotation);
        transform.Rotate(new Vector3(0,mouseX,0));
        */

        //Set the player rotation based on the look transform
        //transform.rotation = Quaternion.Euler(0, toFollow.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        //toFollow.transform.Rotate(deltaRotation.eulerAngles);
        var damping = 10;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * rotationSpeedX;
        xRotation += mouseX;
        //toFollow.transform.rotation = Quaternion.Euler(0, xRotation, 0);
        toFollow.transform.rotation = Quaternion.Slerp(toFollow.transform.rotation,Quaternion.Euler(0, xRotation, 0),damping);
        
        
        var test = new Vector3(0, mouseX, 0);
        
        var rotation = Quaternion.Euler (0, xRotation, 0);
        //toFollow.transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
        //toFollow.RotateAround(transform.position, test, Time.deltaTime * rotationSpeedX);
        //MovePlayer();
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        
        //float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilityX;
        //xRotation += mouseX;
    
       // transform.rotation = Quaternion.Euler(0,xRotation,0);
    
        //myRigidbody.MovePosition(myRigidbody.position + transform.TransformDirection(playerMovement) * movementSpeed * Time.fixedDeltaTime);

        /*
        Quaternion deltaRotation = Quaternion.Euler(_rb.rotation.eulerAngles * rotationSpeed * Time.fixedDeltaTime);
        float mouseX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime;
        xRotation += mouseX;
        //_rb.MoveRotation(_rb.rotation * deltaRotation);
        _rb.MoveRotation(Quaternion.Euler(0,xRotation,0));*/

        
        Vector3 rotationSpeed = new Vector3(0, rotationSpeedX, 0);
        Quaternion deltaRotation = Quaternion.Euler(inputDirection.x * rotationSpeed*Time.fixedDeltaTime );
        //print(inputDirection.x);
        /*
        _rb.MoveRotation(_rb.rotation * deltaRotation);
        */
        //_rb.MovePosition(_rb.position + transform.forward * movementSpeed * inputDirection.y * Time.deltaTime);
        
        /*
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * rotationSpeedX;
        xRotation += mouseX;
        print(xRotation);
        */
        //transform.Rotate(Vector3.up*mouseX);
        
        //_rb.AddTorque(transform.up*rotationSpeedX*mouseX);
        //_rb.MoveRotation(_rb.rotation * deltaRotation);

    }

    void InputPlayer()
    {
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }
    void MovePlayer()
    {
        if (horizontalInput == 0 && verticalInput == 0)
            return;
        
        Vector3 moveDirection = toFollow.transform.forward * verticalInput + toFollow.right * horizontalInput;

        print(moveDirection);
        //transform.forward = moveDirection;
        var rotation = Quaternion.LookRotation(moveDirection);
        var damping = 10;
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
        //Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        //Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        //_rb.MovePosition(transform.position+_speed*moveDirection*Time.deltaTime);
        //transform.position += _speed*moveDirection*Time.deltaTime;
        _rb.velocity = _speed * moveDirection.normalized;
        //_rb.rot
        //_rb.MovePosition(_rb.position + Time.deltaTime*_speed*moveDirection.normalized);
        //_rb.AddForce(_speed*moveDirection.normalized,ForceMode.VelocityChange);
    }
    
    void LookRightLeft()
    {
        if (verticalInput == 0)
            return;
        //Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 moveDirection = toFollow.transform.forward * verticalInput + toFollow.right * horizontalInput;
        
        transform.forward = toFollow.transform.forward;
        //Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        _rb.AddForce(_speed*moveDirection.normalized,ForceMode.VelocityChange);
        //_rb.rot
        //_rb.MovePosition(_rb.position + Time.deltaTime*_speed*moveDirection.normalized);
    }
}

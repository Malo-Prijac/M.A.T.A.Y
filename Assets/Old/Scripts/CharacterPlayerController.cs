using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlayerController : MonoBehaviour
{
    [SerializeField] private float speedWalk;
    [SerializeField] private float speedRun;
    [SerializeField] private float ratioLeftRight = 0.5f;
    [SerializeField] private float ratioFordBack = 1.5f;

    private float vertical;
    private float horizontal;
    private Vector3 currentPosition;
    private Vector3 targetPosition;
    public float dashSpeed;
    public float dashTime;
    private Rigidbody rb;
    public bool characterOnTheGround = true;
    [SerializeField] private float inpulse = 10f;

    [SerializeField] private Animator characterAnimator;
    private static readonly int IsWalkingForward = Animator.StringToHash("IsWalkingForward");
    private static readonly int IsWalkingBackward = Animator.StringToHash("IsWalkingBackward");    
    private static readonly int IsWalkingLeft = Animator.StringToHash("IsWalkingLeft");
    private static readonly int IsWalkingRight = Animator.StringToHash("IsWalkingRight");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");


    private Vector2 cameraRotate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        CameraRotation();

        if (Input.GetButton("Run"))
        {
            MoveCharacter(speedRun);
        }
        else
        {
            MoveCharacter(speedWalk);
        }

        //Dash
        if(Input.GetButtonDown("Dash"))
        {
            characterAnimator.SetBool(IsDashing, true);

            StartCoroutine(Dash());
        }

        //Jump
        if(Input.GetButtonDown("Jump") && characterOnTheGround)
        {
            characterAnimator.SetBool(IsJumping, true);
            rb.AddForce(new Vector3(0,inpulse,0), ForceMode.Impulse);
            characterOnTheGround = false;
        }
    }

    //Reset Jump
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            characterOnTheGround = true;
            characterAnimator.SetBool(IsJumping, false);
            //Debug.Log("Jump ready");
        }
    }

    IEnumerator Dash(){
        float startTime = Time.time;
        while(Time.time < startTime + dashTime){
            transform.position = Vector3.MoveTowards(currentPosition, currentPosition+transform.forward, Time.deltaTime * dashSpeed);
            yield return null;
        }
        characterAnimator.SetBool(IsDashing, false);
    }

    void MoveCharacter(float speed)
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        currentPosition = transform.position;
        
        targetPosition = currentPosition + transform.forward*vertical+transform.right*horizontal;//new Vector3(horizontal, 0, vertical);
        
        //targetPosition += targetPosition;
        //transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * speed);

        
        characterAnimator.SetBool(IsWalkingForward, vertical > 0 && horizontal == 0);
        characterAnimator.SetBool(IsWalkingBackward, vertical < 0 && horizontal == 0);
        
        characterAnimator.SetBool(IsWalkingRight, horizontal > 0);
        characterAnimator.SetBool(IsWalkingLeft, horizontal < 0);

        characterAnimator.SetBool(IsRunning, Mathf.Approximately(speed,speedRun) && (vertical != 0 || horizontal != 0));

        /*
        if(vertical != 0)
        {
            transform.LookAt(targetPosition);
        }*/

        float newSpeed = speed;
        if (vertical != 0)
        {
            newSpeed *= ratioFordBack;
        }

        if (horizontal != 0)
        {
            newSpeed *= ratioLeftRight;
        }
        
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, Time.deltaTime * newSpeed);
    }

    void CameraRotation()
    {
        cameraRotate.x += Input.GetAxis("Mouse X");
        transform.localRotation = Quaternion.Euler(0,cameraRotate.x,0);
    }
}
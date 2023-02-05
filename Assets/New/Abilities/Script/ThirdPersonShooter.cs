 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 using Unity.VisualScripting;

 public class ThirdPersonShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform bulletProjectile;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform toFollow;
    private PlayerCharacterController playerController;
    [SerializeField] private CinemachineVirtualCamera cinemachineMain;
    [ReadOnly] [SerializeField] private Vector3 aimDirection;
    [SerializeField] private GameObject weapon;

    private void Start()
    {
        playerController = GetComponent<PlayerCharacterController>();
    }

    private void Update()
    {
                
        /*
        if (Input.GetMouseButton(0))
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawn.position).normalized;
            Instantiate(bulletProjectile, bulletSpawn.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        */
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCam.ScreenPointToRay(screenCenterPoint);


        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (Input.GetMouseButton(1))
        {
            cinemachineMain.gameObject.SetActive(false);
            aimVirtualCamera.gameObject.SetActive(true);
            
            Vector3 eulerAngles = toFollow.rotation.eulerAngles;
            eulerAngles.x = playerController.gameObject.transform.eulerAngles.x;
            Quaternion angle = Quaternion.Euler(eulerAngles);
            playerController.RotatePlayer(angle);

            weapon.transform.rotation = toFollow.rotation;
            //Vector3 worldAimTarget = mouseWorldPosition;
            //worldAimTarget.y = transform.position.y;
            //worldAimTarget.x = transform.position.x;
            //aimDirection = (worldAimTarget); // - transform.position).normalized;

            // _moveDirection = toFollow.forward * _verticalInput + toFollow.right * _horizontalInput;
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //toFollow.LookAt(worldAimTarget);
            //playerController.RotateTargetForCamera(aimDirection.x,0);


            //Make the character face where he's aiming | transform.forward
            //playerController.gameObject.transform.forward = Vector3.Lerp(playerController.gameObject.transform.forward, aimDirection, Time.deltaTime * 10f);
            //cinemachineMain.
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            cinemachineMain.gameObject.SetActive(true);

        }
    }
    
    


    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1))
        {

        }


        Quaternion rotation = Quaternion.LookRotation(aimDirection);
        //playerController.RotatePlayer(rotation);
        //playerController.RotatePlayer(aimDirection);

    }
}

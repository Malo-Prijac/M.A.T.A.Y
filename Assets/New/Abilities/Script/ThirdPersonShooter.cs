 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
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

    private void Start()
    {
        playerController = GetComponent<PlayerCharacterController>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCam.ScreenPointToRay(screenCenterPoint);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (Input.GetMouseButton(1))
        {
            aimVirtualCamera.gameObject.SetActive(true);
            
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            //worldAimTarget.x = transform.position.x;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            
            //Make the character face where he's aiming | transform.forward
            toFollow.forward = Vector3.Lerp(toFollow.forward, aimDirection, Time.deltaTime * 10f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }
        
        /*
        if (Input.GetMouseButton(0))
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawn.position).normalized;
            Instantiate(bulletProjectile, bulletSpawn.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
        */
    }
}

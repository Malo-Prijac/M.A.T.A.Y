 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 using Unity.VisualScripting;

 public class ThirdPersonShooter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCam;
    [SerializeField] private Transform toFollow;
    [SerializeField] private CinemachineVirtualCamera mainVirtualCam;
    [SerializeField] private GameObject weapon;
    
    private PlayerCharacterController _playerController;
    private Abilities _abilities;
    public Vector3 mouseWorldPosition;

    private Camera cam;
    private void Start()
    {
        _playerController = GetComponent<PlayerCharacterController>();
        _abilities = GetComponent<Abilities>();

        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {

        if (Input.GetButton("Aim") && _abilities.HasRangedWeapon)
        {
            mainVirtualCam.gameObject.SetActive(false);
            aimVirtualCam.gameObject.SetActive(true);


            Vector3 eulerAngles = toFollow.rotation.eulerAngles;
            eulerAngles.x = _playerController.transform.eulerAngles.x;
            Quaternion angle = Quaternion.Euler(eulerAngles);
            
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = cam.ScreenPointToRay(screenCenterPoint);

            Physics.Raycast(ray, out RaycastHit raycastHit);
            mouseWorldPosition = ray.GetPoint(20);
            Vector3 direction = mouseWorldPosition - toFollow.position;
            direction.y = 0;
            _playerController.RotatePlayer(angle);
            //weapon.transform.LookAt(direction);
            weapon.transform.rotation = toFollow.rotation;

        }
        else
        {
            aimVirtualCam.gameObject.SetActive(false);
            mainVirtualCam.gameObject.SetActive(true);

        }
    }
    
}

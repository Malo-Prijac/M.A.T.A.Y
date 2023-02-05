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

    private void Start()
    {
        _playerController = GetComponent<PlayerCharacterController>();
    }

    private void Update()
    {

        if (Input.GetMouseButton(1))
        {
            mainVirtualCam.gameObject.SetActive(false);
            aimVirtualCam.gameObject.SetActive(true);
            
            Vector3 eulerAngles = toFollow.rotation.eulerAngles;
            eulerAngles.x = _playerController.gameObject.transform.eulerAngles.x;
            Quaternion angle = Quaternion.Euler(eulerAngles);
            _playerController.RotatePlayer(angle);
            weapon.transform.rotation = toFollow.rotation;

        }
        else
        {
            aimVirtualCam.gameObject.SetActive(false);
            mainVirtualCam.gameObject.SetActive(true);

        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private float _axisThreshold = 1f;

    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private float _verticalInput;
    private float _horizontalInput;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;

    private void FixedUpdate()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _verticalInput = Mathf.Abs(_verticalInput) >= _axisThreshold ? _verticalInput : 0f;
        /*if (Mathf.Abs(_verticalInput) >= _axisThreshold)
        {
            
        }
        else
        {
            _verticalInput = 0f;
        }*/
        _horizontalInput = Input.GetAxis("Horizontal");
        _horizontalInput = Mathf.Abs(_horizontalInput) >= _axisThreshold ? _horizontalInput : 0f;
        _currentPosition = transform.position;
        _targetPosition = _currentPosition + new Vector3(_horizontalInput, 0, _verticalInput);
        transform.position = Vector3.MoveTowards(_currentPosition, _targetPosition, _speed * Time.deltaTime);
        transform.LookAt(_targetPosition);
        _characterAnimator.SetBool(IsWalking, _verticalInput != 0f || _horizontalInput != 0f);
    }

}

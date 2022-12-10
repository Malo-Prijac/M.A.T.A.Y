using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [SerializeField] private float sensibilityX = 100;
    [SerializeField] private float sensibilityY = 100;
    private float xRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensibilityX;
        xRotation += mouseX;
        transform.rotation = Quaternion.Euler(0,xRotation,0);
    }
}

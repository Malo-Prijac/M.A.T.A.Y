using UnityEngine;

public class CameraJoueurBall : MonoBehaviour
{

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 2.21f, -4.75f);
    }
}
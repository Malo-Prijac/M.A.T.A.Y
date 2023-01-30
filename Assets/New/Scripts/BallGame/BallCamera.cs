using UnityEngine;

public class BallCamera : MonoBehaviour
{

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 7.5f, 0);
    }
}
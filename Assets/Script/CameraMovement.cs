using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float speed = 2f;
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, newPos, speed * Time.deltaTime);
    }

}

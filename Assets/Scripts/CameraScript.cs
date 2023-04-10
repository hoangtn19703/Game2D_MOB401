using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float start, end, up, down;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // trục x player
        var playerX = player.transform.position.x;
        var playerY = player.transform.position.y;
        // trục x camera
        var cameraX = transform.position.x;
        var cameraY = transform.position.y;

        if (playerX > start && playerX < end)
        {
            cameraX = playerX;

        }
        else
        {
            if (playerX < start)
            {
                cameraX = start;
            }
            if (playerX > end)
            {
                cameraX = end;
            }
        }
        if (playerY > up && playerY < down)
        {
            cameraY = playerY;
        }
        else
        {
            if (playerY < up)
            {
                cameraY = up;
            }
            if (playerY > down)
            {
                cameraY = down;
            }
        }
        transform.position = new Vector3(cameraX, cameraY, -10);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPosFromCameraAspectRatio : MonoBehaviour
{
    public Player Player;
    public CameraAspectRatio Camera;
    public float adjustment;

    // Update is called once per frame
    void Update()
    {
        var pos = Player.transform.position;
        pos.y = Camera.GetComponent<Camera>().orthographicSize + adjustment;
        Player.transform.position = pos;
    }
}

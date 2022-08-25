using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed = 10.0f;
    public Vector3 dist;
    public Transform lookTarget;


    void FixedUpdate()
    {
        if (cameraTarget!=null)
        {
            Vector3 dPos = cameraTarget.position + dist;
            Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
            transform.position = sPos;
            transform.LookAt(lookTarget.position);
        }
        
    }
}

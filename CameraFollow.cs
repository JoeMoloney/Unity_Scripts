using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Usage: 3D & 2D
    //Comment Line 25 For 2D

    //Use Transform to get info about position, rotation & scale
    public Transform target;
    public float smoothSpeed = 1f;
    public Vector3 offset; //Value to offset camera by relative to player

    //Run camera follow function after movement function has been made in Update() > See PlayerMovement Script
    private void FixedUpdate()
    {
        //Transform position of object script is attached to to target coords
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime); //CurrentPosition, TargetPosition, TimeToReachPosition

        transform.position = smoothPosition;

        //Rotate Camera Around Y-Axis
        transform.LookAt(target);
    }
}
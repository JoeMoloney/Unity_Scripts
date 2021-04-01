using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    //Usage 3D & 2D
    float delay = 3.3f;
    void LateUpdate()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler((180 + angle), -90, -90);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(angle, -90, 0), 5);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(180 + angle, -90, -90), delay);
    }
}

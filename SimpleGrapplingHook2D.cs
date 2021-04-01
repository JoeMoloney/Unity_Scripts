using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGrapplingHook2D : MonoBehaviour
{
    public Camera mainCamera; //Scene Camera Object (for getting grid coords)
    public LineRenderer _lineRenderer; //The line to be animated in
    public DistanceJoint2D _distanceJoint; //The physics line that connects object to mouse click after coroutine
    public float time = 0.1f; //Time to draw line

    void Start()
    {
        _distanceJoint.enabled = false; //Disable joint on script load
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //On Left Click
        {
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition); //Use X & Y Axis Of Screen
            _lineRenderer.SetPosition(0, mousePos); //Get position of mouse click
            _lineRenderer.SetPosition(1, transform.position); //Get position of object script is attached to
            _distanceJoint.connectedAnchor = mousePos; //Anchors first object to mouse click location
            _lineRenderer.enabled = true; //Enable the line to be drawn
            _distanceJoint.enabled = true; //Enable the Rigidbody2D Link
            StartCoroutine(LineDraw(time)); //Animate the line being drawn 
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) //On Left Click Release
        {
            StartCoroutine(LineRetract(time)); //Animate line retracting
        }
        if (_distanceJoint.enabled)
        {
            _lineRenderer.SetPosition(1, transform.position);
        }
    }

    IEnumerator LineDraw(float time)
    {
        float t = 0;
        Vector3 orig = _lineRenderer.GetPosition(0); //Mouse Position
        Vector3 orig2 = _lineRenderer.GetPosition(1); //Object Position
        _lineRenderer.SetPosition(0, orig2); //Set Mouse Position For Line Renderer
        Vector3 newPos;

        for (; t < time; t += Time.deltaTime)
        {
            newPos = Vector3.Lerp(orig2, orig, t / time); //Interpolate Between Object Position & Mouse Position
            _lineRenderer.SetPosition(1, newPos); //Each Loop - Set Current Line Renderer Position [From Object To Mouse]
            yield return null;
        }
        _lineRenderer.SetPosition(0, orig); //Set New Mouse Position For Line Renderer
    }
    IEnumerator LineRetract(float time)
    {
        float t = 0;
        Vector3 orig = _lineRenderer.GetPosition(0); //Mouse Position
        Vector3 orig2 = _lineRenderer.GetPosition(1); //Object Position
        _lineRenderer.SetPosition(0, orig); //Set Object Position For Line Renderer
        Vector3 newPos;

        for (; t < time; t += Time.deltaTime)
        {
            newPos = Vector3.Lerp(orig, orig2, t / time); //Interpolate Between Mouse Position & Object Position
            _lineRenderer.SetPosition(0, newPos); //Each Loop - Set Current Line Renderer Position [From Mouse To Object]
            yield return null;
        }
        _lineRenderer.SetPosition(0, orig2); //Set Object Position For Line Renderer
        _distanceJoint.enabled = false; //Disable The Rigidbody2D Link
        _lineRenderer.enabled = false; //Disable The LineRenderer
    }

    //IEnumerator LineDraw(float time)
    //{
    //    float t = 0;
    //    Vector3 orig = _lineRenderer.GetPosition(0);
    //    Vector3 orig2 = _lineRenderer.GetPosition(1);
    //    _lineRenderer.SetPosition(1, orig);
    //    Vector3 newPos;

    //    for (; t < time; t += Time.deltaTime)
    //    {
    //        newPos = Vector3.Lerp(orig, orig2, t / time);
    //        _lineRenderer.SetPosition(1, newPos);
    //        yield return null;
    //    }
    //    _lineRenderer.SetPosition(1, orig2);
    //    _distanceJoint.enabled = true;
    //}
    //IEnumerator LineRetract(float time)
    //{
    //    float t = 0;
    //    Vector3 orig = _lineRenderer.GetPosition(0);
    //    Vector3 orig2 = _lineRenderer.GetPosition(1);
    //    _lineRenderer.SetPosition(1, orig);
    //    Vector3 newPos;

    //    _distanceJoint.enabled = false;
    //    for (; t < time; t += Time.deltaTime)
    //    {
    //        newPos = Vector3.Lerp(orig2, orig, t / time);
    //        _lineRenderer.SetPosition(1, newPos);
    //        yield return null;
    //    }
    //    _lineRenderer.SetPosition(1, orig2);
    //    _lineRenderer.enabled = false;
    //}
}
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
        _distanceJoint.enabled = false; //Disable Rigidbody2D Joint on script load
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //On Left Click
        {
            StopCoroutine(LineRetract(time)); //Stop LineRetract Coroutine
            GetPositions(); //Get Coords & Enable LineRenderer + Rigidbody2D Joint
            StartCoroutine(LineDraw(time)); //Animate the line being drawn 
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) //On Left Click Release
        {
            StopCoroutine(LineDraw(time)); //Stop LineDraw Coroutine
            StartCoroutine(LineRetract(time)); //Animate line retracting
        }
        if (_distanceJoint.enabled) //If Rigidbody2D Joint Is Enabled
            _lineRenderer.SetPosition(1, transform.position); //Set LineRenderer To Follow Connected Object
    }

    public void GetPositions() //Get Coords & Enable LineRenderer + Rigidbody2D Joint
    {
        Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition); //Use X & Y Axis Of Screen
        _lineRenderer.SetPosition(0, mousePos); //Get position of mouse click
        _lineRenderer.SetPosition(1, transform.position); //Get position of object script is attached to
        _distanceJoint.connectedAnchor = mousePos; //Anchors first object to mouse click location
        _lineRenderer.enabled = true; //Enable the line to be drawn
        _distanceJoint.enabled = true; //Enable the Rigidbody2D Link
    }

    public void ReleasePositions() //Disable LineRenderer & RigidBody2D Joint
    {
        _lineRenderer.enabled = false; //Disable the line to be drawn
        _distanceJoint.enabled = false; //Disable the Rigidbody2D Link
    }

    IEnumerator LineDraw(float time) //Draw Line From Object To Mouse Position
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
    IEnumerator LineRetract(float time) //Retract Line From Mouse Position To Object
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
        ReleasePositions(); //Disable LineRenderer & RigidBody2D Joint
    }
}
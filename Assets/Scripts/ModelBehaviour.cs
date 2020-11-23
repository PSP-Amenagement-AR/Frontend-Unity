using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ModelBehaviour : MonoBehaviour
{
    //movement speed in units per second
    private float movementSpeed;
    private bool Selected = false;
    private Guid _guid;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    //public Joystick VerticalRotationJoystick;

    private void Awake()
    {
        this._guid = Guid.NewGuid();
        if (ARSession.state == ARSessionState.Unsupported)
        {
            movementSpeed = 30f;
        } else
        {
            movementSpeed = 1f;
        }

    }

    public Guid GetId()
    {
        return this._guid;
    }


    private void Update()
    {
        if (!Selected)
            return;

        //var rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = new Vector3(MovementJoystick.Horizontal * 100f,
        //    rigidbody.velocity.y,
        //    MovementJoystick.Vertical * 100f);
        //float speed = RotationJoystick.Horizontal * 1f;

        float speed = RotationJoystick.Horizontal * -1f;

        /*float rotateVertical = MovementJoystick.Vertical * 1f;
        float rotateHorizontal = MovementJoystick.Horizontal * 1f;*/
        transform.position = transform.position + new Vector3(MovementJoystick.Horizontal * movementSpeed * Time.deltaTime, 0, MovementJoystick.Vertical * movementSpeed * Time.deltaTime);
        transform.Rotate(0, 0, speed * 50f * Time.deltaTime);
        //transform.Rotate(rotateHorizontal, 0, rotateVertical);
    }

    public void SetSelected(bool val)
    {
        Selected = val;
    }
    public bool IsSelected()
    {
        return Selected;
    }
}

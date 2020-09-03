using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBehaviour : MonoBehaviour
{
    private bool Selected = false;
    private Guid _guid;
    //public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    public Joystick VerticalRotationJoystick;

    private void Awake()
    {
        this._guid = Guid.NewGuid();

    }

    public Guid GetId()
    {
        return this._guid;
    }


    private void Update()
    {
        if (!Selected)
            return;

        var rigidbody = GetComponent<Rigidbody>();
        /*rigidbody.velocity = new Vector3(MovementJoystick.Horizontal * 100f,
            rigidbody.velocity.y,
            MovementJoystick.Vertical * 100f);*/
        //float speed = RotationJoystick.Horizontal * 1f;

        float speed = RotationJoystick.Horizontal * -1f;
        float speed1 = VerticalRotationJoystick.Vertical * 1f;

        /*float rotateVertical = MovementJoystick.Vertical * 1f;
        float rotateHorizontal = MovementJoystick.Horizontal * 1f;*/

        transform.Rotate(speed1 * 50f * Time.deltaTime, 0, speed * 50f * Time.deltaTime);
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

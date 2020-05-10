using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBehaviour : MonoBehaviour
{
    private bool Selected = false;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;

    private void Update()
    {
        if (!Selected)
            return;

        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(MovementJoystick.Horizontal * 100f,
            rigidbody.velocity.y,
            MovementJoystick.Vertical * 100f);
        float speed = RotationJoystick.Horizontal;
        transform.Rotate(Vector3.back * speed * 50f * Time.deltaTime);
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

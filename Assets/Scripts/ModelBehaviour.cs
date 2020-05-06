using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBehaviour : MonoBehaviour
{
    private bool Selected = false;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;

    void Start()
    {
    }

    private void Update()
    {
        if (!Selected)
            return;

        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(MovementJoystick.Horizontal * 50f,
            rigidbody.velocity.y,
            MovementJoystick.Vertical * 50f);
        float speed = RotationJoystick.Horizontal;
        transform.Rotate(Vector3.down * speed * 50f * Time.deltaTime);
    }

    public void SetSelected(bool val)
    {
        Selected = val;
        if (Selected)
            GlobalAction.ActivateSelectedInterface();
        else
            GlobalAction.DeactivateSelectedInterface();
    }
    public bool IsSelected()
    {
        return Selected;
    }
}

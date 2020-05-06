using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBehaviour : MonoBehaviour
{

    private bool Selected = false;
    private Joystick MovementJoystick;
    private Joystick RotationJoystick;
    public Text debugField;

    void Start()
    {
        Joystick[] Joysticks;
        Joysticks = FindObjectsOfType<Joystick>();
        RotationJoystick = Joysticks[0];
        MovementJoystick = Joysticks[1];

        Debug.Log(MovementJoystick);
        Debug.Log(RotationJoystick);
        Selected = true;
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
    }
    public bool IsSelected()
    {
        return Selected;
    }
}

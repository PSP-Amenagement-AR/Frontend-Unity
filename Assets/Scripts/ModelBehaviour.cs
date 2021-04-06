using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Class for model behaviour.
/// </summary>
public class ModelBehaviour : MonoBehaviour
{
    //movement speed in units per second
    /// Float variable for movement speed.
    public float movementSpeed;
    /// Boolean for the selection.
    public bool Selected = false;
    /// Guid object.
    /// @see Guid
    public Guid _guid;
    /// Joystick object for the movement joystick.
    public Joystick MovementJoystick;
    /// Joystick object for the rotation joystick.
    public Joystick RotationJoystick;
    //public Joystick VerticalRotationJoystick;

    /// CameraHandler object.
    public CameraHandler cameraHandler;

    /// Initiate Guid object and the speed.
    /// @note Function executed when the script is started.
    public void Awake()
    {
        this._guid = Guid.NewGuid();
        if (ARSession.state == ARSessionState.Unsupported)
        {
            movementSpeed = 30f;
        }
        else
        {
            movementSpeed = 1f;
        }

    }

    /// Get id.
    /// @returns Guid object.
    /// @see Guid
    public Guid GetId()
    {
        return this._guid;
    }

    /// Update the position when the joystick is used.
    /// @note Function executed once per frame.
    public void Update()
    {
        if (!Selected)
            return;

        float speed = RotationJoystick.Horizontal * -1f;
        transform.Rotate(0, 0, speed * 50f * Time.deltaTime);

        if (MovementJoystick)
        {
            Vector3 newPos = transform.position + new Vector3(MovementJoystick.Horizontal * movementSpeed * Time.deltaTime, 0, MovementJoystick.Vertical * movementSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }

    /// Set the boolean if there is a selection.
    /// @param val Boolean indicating the selection.
    public void SetSelected(bool val)
    {
        Selected = val;
    }
    /// Indicate if it's selected.
    /// @returns Boolean variable.
    public bool IsSelected()
    {
        return Selected;
    }
}

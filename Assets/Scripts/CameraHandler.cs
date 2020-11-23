using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraHandler : MonoBehaviour
{
    public Camera ARCamera;
    public Camera Camera;
    IEnumerator Start()
    {
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            Debug.Log("No AR mode");
            this.ARCamera.gameObject.SetActive(false);
            this.Camera.gameObject.SetActive(true);
        }
        else
        {
            this.ARCamera.gameObject.SetActive(true);
            this.Camera.gameObject.SetActive(false);
        }
    }

    public Camera GetCamera()
    {
        if (ARSession.state == ARSessionState.Unsupported)
            return Camera;
        else
            return ARCamera;
    }
}

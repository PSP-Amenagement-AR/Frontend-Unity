using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CameraHandler : MonoBehaviour
{
    public Camera ARCamera;
    public Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Camera GetCamera()
    {
        if (ARSession.state == ARSessionState.Unsupported)
            return Camera;
        else
            return ARCamera;
    }

    private void Awake()
    {
        if (ARSession.state == ARSessionState.Unsupported)
        {
            this.ARCamera.gameObject.SetActive(false);
            this.Camera.gameObject.SetActive(true);
        } else
        {
            this.ARCamera.gameObject.SetActive(true);
            this.Camera.gameObject.SetActive(false);
        }
    }
}

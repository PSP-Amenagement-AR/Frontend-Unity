using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{

    //public GameObject gameObjectToInstantiate;

    private bool objectToPlace = false;
    private ARRaycastManager _arRaycastManager;
    //public Joystick MovementJoystick;
    public GameObject AddPanel;
    public GameObject PanelPlacementIndicator;
    public ScreenHandler screenHandler;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject ItemToPlace;
    GameObject[] List3DModels;

    [SerializeField]
    public ARItemsHandling itemsHandling;

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        List3DModels = Resources.LoadAll<GameObject>("Prefabs");
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            /*Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {*/
                if (AddPanel.activeSelf == false && objectToPlace)
                {
                    touchPosition = Input.GetTouch(0).position;
                    return true;
                }

            //}
        }
        
        touchPosition = default;
        return false;
    }

    public GameObject GetModel3D(string name)
    {
        foreach (GameObject model3D in List3DModels)
        {
            if (model3D.name == name)
            {
                Debug.Log("Item Found in the List3DModels");
                Rigidbody model3DRigidBody = model3D.AddComponent<Rigidbody>();
                try
                {
                    model3DRigidBody.useGravity = false;
                }
                catch (System.Exception e)
                {
                }
                return model3D;
            }
        }
        return null;
    }


    public void PreAddItem(string name)
    {
        this.ItemToPlace = this.GetModel3D(name);
        Debug.Log("SetItemToPlaceName : " + this.ItemToPlace);
        this.EnableInterface();
        Update();
    }

    public void EnableInterface()
    {
        PanelPlacementIndicator.SetActive(true);
        this.objectToPlace = true;
        screenHandler.HideUi();
    }
    public void CleanInterface()
    {
        PanelPlacementIndicator.SetActive(false);
        this.ItemToPlace = null;
        objectToPlace = false;
        screenHandler.ShowUi();
    }

    public void Update()
    {
        if (objectToPlace)
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;
            if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                itemsHandling.AddItem(this.ItemToPlace, hitPose.position, hitPose.rotation);
                this.CleanInterface();
            }
        }
    }
}

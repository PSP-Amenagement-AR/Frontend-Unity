using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>Class for manage the tap of user on screen and object placement in the stage.</summary>
[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    /// <summary>Boolean if object is placed or not.</summary>
    public bool objectToPlace = false;
    /// <summary>ARRaycastManager object for plane detection.</summary>
    /// @see ARRaycastManager()
    public ARRaycastManager _arRaycastManager;
    /// <summary>Add of a new panel.</summary>
    public GameObject AddPanel;
    /// <summary>Indicator for the object placement.</summary>
    public GameObject PanelPlacementIndicator;
    /// <summary>Instance of ScreenHandler class.</summary>
    /// @see ScreenHandler()
    public ScreenHandler screenHandler;
    /// <summary>List of ARRaycastHit for each hit of the user on screen.</summary>
    /// @see ARRaycastHit()
    public static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    /// <summary>GameObject for the object to place in the stage.</summary>
    public GameObject ItemToPlace;
    /// <summary>List of the 3D objects.</summary>
    public GameObject[] List3DModels;
    /// <summary>Instance of ARItemsHandling class.</summary>
    /// @see ARItemsHandling
    public ARItemsHandling itemsHandling;

    /// Function executed when the script is started.
    /// Load prefab objects from the ressources and get of scanned plane.
    public void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        List3DModels = Resources.LoadAll<GameObject>("Prefabs");
    }

    /// Try to get the touched position by the user.
    /// @param touchPosition Vector2 object which contains the position of the touch point.
    /// @return Return a boolean if the touched position is valid.
    public bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
                if (AddPanel.activeSelf == false && objectToPlace)
                {
                    touchPosition = Input.GetTouch(0).position;
                    return true;
                }
        }
        
        touchPosition = default;
        return false;
    }

    /// Get 3D Model from the list by his name.
    /// @param name Name of the 3D object to get.
    /// @return Return the 3D object or null if it is not recovered.
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

    /// Get prefab of the object to place.
    /// @param prefab PrefabJSON object to place.
    public void PreAddItem(PrefabJSON prefab)
    {
        ARItemsHandling.ConfigItem found = itemsHandling.GetConfigItems().Find((config) => config.name == prefab.typeName);
        if (found != null)
        {
            this.ItemToPlace = found.loadedPrefab;
            itemsHandling.SetPrefabJSON(prefab);
            Debug.Log("SetItemToPlaceName : " + this.ItemToPlace);
            this.EnableInterface();
            Update();
        }
    }

    /// Activation of the selection interface when an object is selected.
    public void EnableInterface()
    {
        PanelPlacementIndicator.SetActive(true);
        this.objectToPlace = true;
        screenHandler.HideUi();
    }

    /// Clean of the interface.
    public void CleanInterface()
    {
        PanelPlacementIndicator.SetActive(false);
        this.ItemToPlace = null;
        objectToPlace = false;
        screenHandler.ShowUi();
    }

    /// Function executed once per frame.
    /// If there is an object to place, the interface is set.
    /// If the user hit the screen, the function analyze if the position is valid or if the his is for set an object.
    public void Update()
    {
        if (objectToPlace)
        {
            if (ARSession.state == ARSessionState.Unsupported)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    itemsHandling.AddItem(this.ItemToPlace, new Vector3(Screen.width / 2, 0, 0), Quaternion.identity);
                    this.CleanInterface();
                }
            } else
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
}

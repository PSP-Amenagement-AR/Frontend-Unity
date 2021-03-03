using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                if (AddPanel.activeSelf == false && objectToPlace)
                {
                    touchPosition = Input.GetTouch(0).position;
                    return true;
                }
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

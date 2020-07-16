﻿using System.Collections;
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

    private GameObject spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    public Button DelButton;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private string ItemToPlace;
    GameObject[] List3DModels;

    private ModelBehaviour selectedModel;
    public ModelBehaviour SelectedModel
    {
        get
        {
            return selectedModel;
        }
        set
        {
            if (!value)
                ActivateSelectedInterfaceTo(false);
            else
            {
                if (selectedModel)
                {
                    selectedModel.SetSelected(false); // unselect old model
                    selectedModel.MovementJoystick = null;
                    selectedModel.RotationJoystick = null;
                }
                selectedModel = value;
                selectedModel.SetSelected(true); // select new model
                selectedModel.MovementJoystick = MovementJoystick;
                selectedModel.RotationJoystick = RotationJoystick;
                ActivateSelectedInterfaceTo(true);
            }
        }
    }

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        List3DModels = Resources.LoadAll<GameObject>("Prefabs");
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void SetItemToPlaceName(string itemName)
    {
        spawnedObject = null;
        this.ItemToPlace = itemName;
        Debug.Log("SetItemToPlaceName : " + this.ItemToPlace);
    }

    public GameObject SelectModel3D()
    {
        foreach (GameObject model3D in List3DModels)
        {
            if (model3D.name == this.ItemToPlace)
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

    /*public void AddModel()
    {
        GameObject model = Instantiate(SelectModel3D(), new Vector3(0.0f, 0.0f, 0.0f), transform.rotation, ParentTarget);
        model.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        model.transform.localScale = model.transform.localScale * 3;
        ModelBehaviour modelBehaviour = model.AddComponent<ModelBehaviour>() as ModelBehaviour;
        SelectedModel = modelBehaviour;
    }*/

    public void ActivateSelectedInterfaceTo(bool val)
    {
        if (MovementJoystick && RotationJoystick && DelButton)
        //if (MovementJoystick && RotationJoystick)
        {
            MovementJoystick.gameObject.SetActive(val);
            RotationJoystick.gameObject.SetActive(val);
            DelButton.gameObject.SetActive(val);
            //spawnedObject = null;
        }
    }

    public void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;
        if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            // Positionnement de l'objet - Sélection
            if (spawnedObject == null)
            {
                //spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                spawnedObject = Instantiate(SelectModel3D(), hitPose.position, hitPose.rotation);
                spawnedObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
                spawnedObject.transform.localScale = spawnedObject.transform.localScale * 3;
                ModelBehaviour modelBehaviour = spawnedObject.AddComponent<ModelBehaviour>() as ModelBehaviour;
                SelectedModel = modelBehaviour;
                Debug.Log("Test1");
            }
            // Déplacement de l'objet en drag (si il est toujours sélectionné)
            else
            {
                spawnedObject.transform.position = hitPose.position;
                Debug.Log("Test2");
            }
        }
    }

    public void DeleteSelectedModel()
    {
        GameObject g = SelectedModel.gameObject;
        if (SelectedModel)
            SelectedModel = null;
        Destroy(g);
    }

    public void UnselectSelection()
    {
        SelectedModel = null;
    }
}

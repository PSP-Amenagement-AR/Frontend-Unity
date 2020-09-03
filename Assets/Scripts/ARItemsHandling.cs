﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARItemsHandling : MonoBehaviour
{
    private GameObject SelectedItem;
    public Joystick RotationJoystick;
    public Joystick VerticalRotationJoystick;
    public Button DeleteButton;
    public Button ValidateButton;

    private List<GameObject> Items = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ModelBehaviour GetModelBehaviour(GameObject item)
    {
        return item.GetComponent<ModelBehaviour>();
    }

    public GameObject FindItem(GameObject itemToFind)
    {
        return this.Items.Find(item => GetModelBehaviour(item).GetId() == GetModelBehaviour(itemToFind).GetId());
    }

    public void ActivateSelectedInterface(bool val)
    {
        if (VerticalRotationJoystick && RotationJoystick && DeleteButton && ValidateButton)
        {
            //MovementJoystick.gameObject.SetActive(val);
            RotationJoystick.gameObject.SetActive(val);
            VerticalRotationJoystick.gameObject.SetActive(val);
            DeleteButton.gameObject.SetActive(val);
            ValidateButton.gameObject.SetActive(val);
        }
    }

    public void SelectItem(GameObject item)
    {
        GameObject found = this.FindItem(item);
        if (found)
        {
            Debug.Log("Item Found: " + found.ToString());
            if (SelectedItem)
            {
                ModelBehaviour model = this.GetModelBehaviour(SelectedItem);
                if (model)
                {
                    model.SetSelected(false);
                    model.RotationJoystick = null;
                    model.VerticalRotationJoystick = null;
                }
            }
            ModelBehaviour foundModel = this.GetModelBehaviour(found);
            if (foundModel)
            {
                this.SelectedItem = found;
                foundModel.SetSelected(true);
                foundModel.RotationJoystick = RotationJoystick;
                foundModel.VerticalRotationJoystick = VerticalRotationJoystick;
                this.ActivateSelectedInterface(true);
            }
        }
    }

    public void UnSelectItem()
    {
        GetModelBehaviour(this.SelectedItem).SetSelected(false);
        this.SelectedItem = null;
        this.ActivateSelectedInterface(false);
    }

    public void DeleteSelectedItem()
    {
        GameObject toRemove = this.SelectedItem;
        this.UnSelectItem();
        this.Items.Remove(toRemove);
        Destroy(toRemove);
    }

    public void AddItem(GameObject objectModel, Vector3 position, Quaternion rotation)
    {
        GameObject spawnedObject = Instantiate(objectModel, position, rotation);
        spawnedObject.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);

        _ = spawnedObject.AddComponent<ModelBehaviour>() as ModelBehaviour;
        this.Items.Add(spawnedObject);
        this.SelectItem(spawnedObject);
    }
}

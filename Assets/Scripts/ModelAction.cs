﻿using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelAction : MonoBehaviour
{

    public GameObject Cube;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;

    public void AddModel()
    {
        Model[] allModels = FindObjectsOfType<Model>();
        foreach (var mod in allModels)
        {
            ModelBehaviour mb = mod.GetComponent<ModelBehaviour>() as ModelBehaviour;
            mb.SetSelected(false);
        }

        GameObject model = Instantiate(Cube, transform.position, transform.rotation);
        model.AddComponent<Model>();
        ModelBehaviour modelBehaviour = model.AddComponent<ModelBehaviour>() as ModelBehaviour;
        modelBehaviour.SetSelected(true);
        modelBehaviour.MovementJoystick = MovementJoystick;
        modelBehaviour.RotationJoystick = RotationJoystick;
    }

    public void DeleteSelectedModel()
    {
        Model[] allModels = FindObjectsOfType<Model>();

        foreach (Model mod in allModels)
        {
            ModelBehaviour mb = mod.GetComponent<ModelBehaviour>() as ModelBehaviour;
            if (mb.IsSelected())
                Destroy(mod.gameObject);
        }
    }
}

using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ModelAction : MonoBehaviour
{
    //public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    public Button DelButton;
    public Transform ParentTarget;
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
                    //selectedModel.MovementJoystick = null;
                    selectedModel.RotationJoystick = null;
                }
                selectedModel = value;
                selectedModel.SetSelected(true); // select new model
                //selectedModel.MovementJoystick = MovementJoystick;
                selectedModel.RotationJoystick = RotationJoystick;
                ActivateSelectedInterfaceTo(true);
            }
        }
    }

    public void Start()
    {
        List3DModels = Resources.LoadAll<GameObject>("Prefabs");
    }

    public void AddModel()
    {
        GameObject model = Instantiate(SelectModel3D(), new Vector3(0.0f, 0.0f, 0.0f), transform.rotation, ParentTarget);
        model.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        model.transform.localScale = model.transform.localScale * 3;
        ModelBehaviour modelBehaviour = model.AddComponent<ModelBehaviour>() as ModelBehaviour;
        SelectedModel = modelBehaviour;
    }

    public void DeleteSelectedModel()
    {
        GameObject g = SelectedModel.gameObject;
        if (SelectedModel)
            SelectedModel = null;
        Destroy(g);
    }

    public void ActivateSelectedInterfaceTo(bool val)
    {
        if (RotationJoystick && DelButton)
        {
            //MovementJoystick.gameObject.SetActive(val);
            RotationJoystick.gameObject.SetActive(val);
            DelButton.gameObject.SetActive(val);
        }
    }

    public GameObject SelectModel3D()
    {      
        foreach(GameObject model3D in List3DModels)
        {
            if (model3D.name == this.ItemToPlace)
            {
                Rigidbody model3DRigidBody = model3D.AddComponent<Rigidbody>();
                try
                {
                    model3DRigidBody.useGravity = false;
                } catch (System.Exception e)
                {
                }
                return model3D;
            }
        }
        return null;
    }

    public void SetItemToPlace(string name)
    {
        this.ItemToPlace = name;
    }

    public void UnselectSelection()
    {
        SelectedModel = null;
    }
}

using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ModelAction : MonoBehaviour
{
    public GameObject Cube;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    public Button DelButton;
    private string ItemToPlace;
    
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

    public void AddModel()
    {

        GameObject model = Instantiate(SelectModel3D(), transform.position, transform.rotation);
        model.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        model.transform.localScale = new Vector3(200, 200, 200);
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
        if (MovementJoystick && RotationJoystick && DelButton)
        {
            MovementJoystick.gameObject.SetActive(val);
            RotationJoystick.gameObject.SetActive(val);
            DelButton.gameObject.SetActive(val);
        }
    }

    public GameObject SelectModel3D()
    {
        bool found = false;
        GameObject[] List3DModels;
        List3DModels = Resources.LoadAll<GameObject>("Prefabs");
        foreach(GameObject model3D in List3DModels)
        {
            if (model3D.name == this.ItemToPlace)
            {
                Debug.Log("The 3D Model " + model3D + " is found");
                found = true;

                //Cube = Instantiate(model3D, transform.position, transform.rotation);
                Rigidbody model3DRigidBody = model3D.AddComponent<Rigidbody>();
                //model3DRigidBody.mass = 5;
                return model3D;
            }
        }
        Debug.Log("This is " + found);
        return null;
    }

    public void SetItemToPlace(string name)
    {
        this.ItemToPlace = name;
        Debug.Log("the item to place is " + this.ItemToPlace);
    }

    public void UnselectSelection()
    {
        SelectedModel = null;
    }
}

using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelAction : MonoBehaviour
{

    public GameObject Cube;
    public Joystick MovementJoystick;
    public Joystick RotationJoystick;
    public Button DelButton;
    
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
        GameObject model = Instantiate(Cube, transform.position, transform.rotation);
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

    public void UnselectSelection()
    {
        SelectedModel = null;
    }
}

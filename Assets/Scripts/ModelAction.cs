using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ModelAction : MonoBehaviour
{
    /* TODO :
     ajouter le script Model sur les models 3D
     récupéer le nom de l'item lors de la sélection
     aller chercher le model 3D avec le bon nom
     */
    private GameObject Cube;
    public Text DebugField;
    private string ItemToPlace;

    public void AddModel()
    {
        Model[] allModels = FindObjectsOfType<Model>();
        foreach (var mod in allModels)
        {
            ModelBehaviour mb = mod.GetComponent<ModelBehaviour>() as ModelBehaviour;
            mb.SetSelected(false);
        }

        GameObject model = Instantiate(SelectModel3D(), transform.position, transform.rotation);
        //model.transform.rotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
        model.transform.localScale = new Vector3(200, 200, 200);

        ModelBehaviour modelBehaviour = model.AddComponent<ModelBehaviour>() as ModelBehaviour;
        modelBehaviour.debugField = DebugField;
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
}

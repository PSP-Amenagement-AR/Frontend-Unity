using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    private string objectType;
    private GameObject prefab;
    private Dictionary<GameObject, string> features;
    //[SerializeField]
    private GameObject canvasTemplate;
    [SerializeField]
    private GameObject content;

    void Start()
    {
        canvasTemplate = this.content.transform.GetChild(0).gameObject;
    }

    void Update()
    {
        
    }

    public void SetObject(string name)
    {
        this.objectType = name;

        this.prefab = SetPrefab();
        SelectFeatures();

        UpdateCreatorManager();
        /*foreach(var pair in this.features)
        {
            GameObject feature = pair.Key;
            string color = pair.Value;
            Debug.Log("-> " + feature.name + " is in " + color);
        }*/
    }

    public GameObject SetPrefab()
    {
        string pathToPrefab = "Items/" + this.objectType + "/" + this.objectType;

        return Resources.Load<GameObject>(pathToPrefab);
    }

    public void SelectFeatures()
    {
        int childs = this.prefab.transform.childCount;
        this.features = new Dictionary<GameObject, string>();

        for (int i = 0; i < childs; i++)
        {
            GameObject feature = this.prefab.transform.GetChild(i).gameObject;
            this.features.Add(feature, "white");
        }
    }

    public void UpdateCreatorManager()
    {
        int indice = 0;
        foreach (var pair in this.features)
        {
            if (indice == 0)
            {
                GameObject first_feature = canvasTemplate.transform.Find("Name").gameObject;
                Text name = first_feature.GetComponent<Text>();
                name.text = pair.Key.name;
            }
            else
            {
                GameObject newCanvas = Instantiate(canvasTemplate) as GameObject;
                newCanvas.SetActive(true);

                Text name = GameObject.Find("Name").GetComponent<Text>();
                name.text = pair.Key.name;
                newCanvas.transform.SetParent(canvasTemplate.transform.parent, false);
            }
            indice += 1;
        }
    }

    public void ClearFeatures()
    {
        int counter = content.transform.childCount;
        GameObject feature_to_remove = new GameObject();

        for (int i = 0; i < counter; i++)
        {
            feature_to_remove = this.content.transform.GetChild(i).gameObject;
            if (i != 0)
            {
                Destroy(feature_to_remove);
            }
        }
    }

}

/*
// Récupérer le nombre de Prefab enfant par objet ainsi que leurs npm
int childs = objectModel.transform.childCount;

for (int i = 0; i<childs; i++)
{
    Debug.Log("TEST " + i + " child " + objectModel.transform.GetChild(i).gameObject.name);
}

// Edit Prefab
if (objectModel.name == "chair_1")
{
    Material cloth1 = (Material)Resources.Load("Materials/cloth_1");
    Material cloth2 = (Material)Resources.Load("Materials/cloth_2");
    GameObject child = objectModel.transform.GetChild(0).gameObject;

    // Edit Prefab Chair
    GameObject seat = objectModel.transform.Find("seat").gameObject;
    GameObject metal = objectModel.transform.Find("metal").gameObject;
    GameObject plastic = objectModel.transform.Find("plastic").gameObject;

    // Apply Color & Material on Prefab
    seat.GetComponent<MeshRenderer>().material = cloth1;
    //seat.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", Color.red);
    metal.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", Color.blue);
    plastic.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", Color.green);
 }
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
//using System.Windows.ColorConverter;

// TODO
//
// Mettre l'objet créé dans l'inventaire
// Ajouter d'autres objets dans le Customiseur de prefab

// lorsque un Color ou Texture palette est ouvert, bloquer les autres features

// Enregistrer l'objet en Back
// SI l'utilisateur est connecté, le Prefab est rengistré en Back & ajouté dans l'inventaire
// SINON le prefab est juste ajotué dans l'inventaire
//
// TODO

public class CreatorManager : MonoBehaviour
{
    private string objectType;
    private GameObject prefab;
    private Dictionary<GameObject, Appearance> features;
    private GameObject canvasTemplate;
    private string title;

    [SerializeField]
    private GameObject content;
    [SerializeField]
    Text indice;
    [SerializeField]
    private GameObject colorWindow;
    [SerializeField]
    private GameObject textureWindow;

    public ARItemsHandling itemsHandling;

    void Start()
    {
        canvasTemplate = this.content.transform.GetChild(0).gameObject;
    }

    void Update() {}

    public void SetObject(string name)
    {
        this.objectType = name;

        this.prefab = SetPrefab();
        SelectFeatures();

        UpdateCreatorManager();
    }

    public GameObject SetPrefab()
    {
        string pathToPrefab = "Items/" + this.objectType + "/" + this.objectType;

        return Resources.Load<GameObject>(pathToPrefab);
    }

    public void SelectFeatures()
    {
        Appearance val;
        int childs = this.prefab.transform.childCount;
        this.features = new Dictionary<GameObject, Appearance>();

        for (int i = 0; i < childs; i++)
        {
            val.color = "default";
            val.texture = "base_material";

            GameObject feature = this.prefab.transform.GetChild(i).gameObject;
            val.name = feature.name;
            this.features.Add(feature, val);
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
                Text index = GameObject.Find("Indice").GetComponent<Text>();
                index.text = indice.ToString();

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
            if (i == 0)
            {
                feature_to_remove.transform.Find("Color").gameObject.GetComponent<Text>().text = "default";
                feature_to_remove.transform.Find("Texture").gameObject.GetComponent<Text>().text = "base_material";
            }
            else
            {
                Destroy(feature_to_remove);
            }
        }
    }

    public void ApplyColor(string pColor)
    {
        int index = Convert.ToInt16(this.indice.text);

        for (int i = 0; i < this.features.Count; i++)
        {
            if (i == index)
            {
                Appearance val = this.features.ElementAt(i).Value;
                val.color = pColor;
                this.features[this.features.ElementAt(i).Key] = val;

                // Update Color label in the feature
                GameObject actualFeature = content.transform.GetChild(i).gameObject;
                GameObject colorObject = actualFeature.transform.Find("Color").gameObject;

                Text colorLabel = colorObject.GetComponent<Text>();
                colorLabel.text = pColor;
            }
        }
    }

    public void ApplyTexture(string pTexture)
    {
        int index = Convert.ToInt16(this.indice.text);

        for (int i = 0; i < this.features.Count; i++)
        {
            if (i == index)
            {
                Appearance val = this.features.ElementAt(i).Value;
                val.texture = pTexture;
                this.features[this.features.ElementAt(i).Key] = val;

                // Update Texture label in the feature
                GameObject actualFeature = content.transform.GetChild(i).gameObject;
                GameObject textureObject = actualFeature.transform.Find("Texture").gameObject;

                Text textureLabel = textureObject.GetComponent<Text>();
                textureLabel.text = pTexture;
            }
        }
    }

    public void SetIndice(Text indice)
    {
        this.indice = indice;
    }

    public void ReadFeatures()
    {
        foreach(var pair in this.features)
        {
            GameObject feature = pair.Key;
            string col = pair.Value.color;
            string mat = pair.Value.texture;
            Debug.Log("-> " + feature.name + " is in " + col + " and " + mat);
        }
    }

    public void OpenColorPalette()
    {
        if (!this.textureWindow.activeSelf) { this.colorWindow.SetActive(true); }
    }

    public void OpenTexturePalette()
    {
        if (!this.colorWindow.activeSelf) { this.textureWindow.SetActive(true); }
    }

    public void EditNewObject()
    {
        int count = 0;
        GameObject feature_to_edit;
        string feature_name;
        string valColor;
        string valTexture;
        Color myColor;
        Material myMaterial;
        string path_to_material;

        foreach (var pair in this.features)
        {
            feature_name = pair.Key.name;
            valColor = pair.Value.color;
            valTexture = pair.Value.texture;

            path_to_material = "Materials/" + valTexture;
            myMaterial = (Material)Resources.Load(path_to_material);

            myColor = Color.clear;
            if (valColor == "default") { valColor = "#FFFFFF"; }
            ColorUtility.TryParseHtmlString(valColor, out myColor);

            feature_to_edit = this.prefab.transform.Find(feature_name).gameObject;
            feature_to_edit.GetComponent<MeshRenderer>().material = myMaterial;
            feature_to_edit.GetComponent<MeshRenderer>().material.SetColor("_Color", myColor);
        }
    }

    public void SetTitle(Text pTitle)
    {
        this.title = pTitle.text;
    }

    public void SaveObject()
    {
        PrefabJSON prf = new PrefabJSON();
        prf.appearances = new List<Appearance>();

        prf.TypeName = this.prefab.name;
        if (this.title == "") { prf.title = "Default Object"; }
        else { prf.title = this.title; }
        
        foreach (var pair in this.features)
        {
            Appearance apr = new Appearance();
            
            if (pair.Value.color == "default") { apr.color = "#FFFFFF"; }
            else { apr.color = pair.Value.color; }
            
            apr.texture = pair.Value.texture;
            apr.name = pair.Value.name;

            prf.appearances.Add(apr);
        }
        string JSONresult = JsonConvert.SerializeObject(prf);
        Debug.Log(JSONresult);
    }
}

/*
// Récupérer le nombre de Prefab enfant par objet ainsi que leurs noms
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
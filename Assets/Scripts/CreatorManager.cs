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
using SimpleJSON;

// TODO
// Ajouter d'autres objets dans le Customiseur de prefab
// lorsque un Color ou Texture palette est ouvert, bloquer les autres features
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
    [SerializeField]
    private GameObject buttonTemplate;
    public InputField titleField;

    void Start()
    {
        canvasTemplate = this.content.transform.GetChild(0).gameObject;
    }

    void Update() { }

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
        foreach (var pair in this.features)
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

    public void SetTitle()
    {
        if (titleField.text == "" || titleField.text == null)
            this.title = "Default Object";
        else
            this.title = titleField.text;
        titleField.text = "";
    }

    public void SaveObject()
    {
        int i = 0;
        PrefabJSON prf = new PrefabJSON();
        prf.appearances = new Appearance[this.features.Count];

        prf.typeName = this.prefab.name;
        if (this.title == "") { prf.title = "Default Object"; }
        else { prf.title = this.title; }

        foreach (var pair in this.features)
        {
            Appearance apr = new Appearance();

            if (pair.Value.color == "default") { apr.color = "#FFFFFF"; }
            else { apr.color = pair.Value.color; }

            apr.texture = pair.Value.texture;
            apr.name = pair.Value.name;

            prf.appearances[i] = apr;
            i += 1;
        }

        if (GlobalStatus.token != "")
        {
            var JSONresult = JsonConvert.SerializeObject(prf);
            this.SaveToBack(JSONresult);
        }
        
        this.UpdateInventory(prf);
    }

    public JSONNode SaveToBack(string JSONresult)
    {
        var request = GlobalStatus.webApi.SendApiRequest("/objects", "POST", JSONresult);
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);

        if (request.responseCode == 200 || request.responseCode == 201)
        {
            Debug.Log(request.responseCode + " is a success");
            return dataJSON;
        }
        else
        {
            Debug.Log(request.responseCode + " is a fail");
            return null;

        }
    }

    public void UpdateInventory(PrefabJSON prf)
    {
        if (buttonTemplate) { Debug.Log("Test 1"); }

        GameObject newButton = Instantiate(buttonTemplate) as GameObject;
        newButton.SetActive(true);

        switch(this.prefab.name)
        {
            case "chair_1":
                Debug.Log("CHAIR 1");
                newButton.GetComponent<InventoryButton>().SetIcon(Resources.Load<Sprite>("Sprites/Items/chair_1_prefab"));
                break;
            case "bed_1":
                Debug.Log("BED 1");
                newButton.GetComponent<InventoryButton>().SetIcon(Resources.Load<Sprite>("Sprites/Items/bed_1_prefab"));
                break;
            case "torchere_1":
                Debug.Log("TORCHERE 1");
                newButton.GetComponent<InventoryButton>().SetIcon(Resources.Load<Sprite>("Sprites/Items/torchere_1_prefab"));
                break;
        }

        newButton.GetComponent<InventoryButton>().SetName(this.title);
        newButton.GetComponent<InventoryButton>().SetPrefabJSON(prf);
        newButton.transform.SetParent(buttonTemplate.transform.parent, false);
    }

}
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

/// <summary>Class for manage the custom 3D object creator.</summary>
public class CreatorManager : MonoBehaviour
{
    /// Type of the object.
    public string objectType;
    /// The prefab of the object.
    public GameObject prefab;
    ///Dictionnary which contains the prefab and a Appearnace object whith the design and specificty of each features in the object.
    /// @see Appearance()
    public Dictionary<GameObject, Appearance> features;
    ///Template of the canvas which contains a description of each feature of the object and the access to the pallets.
    public GameObject canvasTemplate;
    ///Tite of the object to create.
    public string title;
    ///GameObject which contain "content" panel.
    [SerializeField]
    public GameObject content;
    ///Text object for the index of the feature.
    [SerializeField]
    Text indice;
    ///GameObject for the color pallet.
    [SerializeField]
    public GameObject colorWindow;
    ///GameObject for the texture pallet.
    [SerializeField]
    public GameObject textureWindow;
    ///GameObject for the button template.
    [SerializeField]
    public GameObject buttonTemplate;
    ///InputField object for the title field.
    public InputField titleField;

    /// Initiate the canvas template.
    /// @note Function executed when the script is started.
    public void Start()
    {
        canvasTemplate = this.content.transform.GetChild(0).gameObject;
    }

    /// Function executed once per frame.
    void Update() { }

    /// Set of the object from his name.
    /// @param name Name of the object to set.
    public void SetObject(string name)
    {
        this.objectType = name;

        this.prefab = SetPrefab();
        SelectFeatures();

        UpdateCreatorManager();
    }

    /// Set the prefab from the prefab directory.
    /// @returns A GameObject which contains the prefab.
    public GameObject SetPrefab()
    {
        string pathToPrefab = "Items/" + this.objectType + "/" + this.objectType;

        return Resources.Load<GameObject>(pathToPrefab);
    }

    /// Add of the description of all object features in the dictionnary.
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

    /// Update the creator manager interface and add new features.
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

    /// Clear the creator manager interface and delete the features on it.
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

    /// Set the color selected for a feature in terms of the index.
    /// @param pColor The name of the color selected.
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

    /// Set the texture selected for a feature in terms of the index.
    /// @param pTexture The name of the texture selected.
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

    /// Change the index to the index of the feature selected only if no pallet is open.
    /// @param indice Text object which contains the index of the feature selected by the user.
    public void SetIndice(Text indice)
    {
        if (!this.textureWindow.activeSelf && !this.colorWindow.activeSelf)
            this.indice = indice;
    }

    /// Read the description of each features.
    /// @note Function only useful when debugging.
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

    /// Open the color pallet.
    public void OpenColorPalette()
    {
        if (!this.textureWindow.activeSelf) { this.colorWindow.SetActive(true); }
    }

    /// Open the texture pallet.
    public void OpenTexturePalette()
    {
        if (!this.colorWindow.activeSelf) { this.textureWindow.SetActive(true); }
    }

    /// Set the title of the new object created.
    public void SetTitle()
    {
        if (titleField.text == "" || titleField.text == null)
            this.title = "Default Object";
        else
            this.title = titleField.text;
        titleField.text = "";
    }

    /// Save the object description in Database if the user is connected.
    /// The inventory is updated.
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

    /// Save the new object description in database.
    /// @param JSONresult Body of the request.
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

    /// Update the inventory and add a new button in terms of the template used.
    /// @param prf PrefabJSON object which contains the description of the object.
    /// @note Each button own a prefab JSON parameter with the description of the object to which it is assigned.
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

/// <summary> Class for inventory button. </summary>
public class InventoryButton : MonoBehaviour
{
    /// Image object for the button icon.
    [SerializeField]
    public Image icon;
    /// Name of the button.
    [SerializeField]
    public Text name;
    /// The button assigned to the InventoryButton object.
    public Button myButton;
    /// The plane scaned.
    /// @see ARTapToPlaceObject()
    [SerializeField]
    public ARTapToPlaceObject myPlane;
    /// Boolean indicating if an item is selected.
    public bool isItem;
    /// PrefabJSON object containing the objectt description.
    /// @see PrefabJSON()
    public PrefabJSON prefabDescription;

    /// Intiate the button.
    public void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    /// Task executed during a click.
    public void TaskOnClick()
    {
        ReadPrefabJSON();
        if (isItem)
        {
            myPlane.PreAddItem(prefabDescription);
        }
    }

    /// Set the icon.
    /// @param mySprite Sprite object containing the icon.
    public void SetIcon(Sprite mySprite)
    {
        icon.sprite = mySprite;
    }

    /// Set the name.
    /// @param myName The name of the object.
    public void SetName(string myName)
    {
        name.text = myName;
    }

    /// Apply some transparency on a button.
    /// @param transparency Percentage of transparency.
    public void PutTransparency(float transparency)
    {
        Color color = icon.color;
        color.a = transparency;
        icon.color = color;
    }

    /// Initiate the PrefabJSON object for the button.
    /// Each type of features have color and texture set with default values in terms of prefab name.
    /// @see PrefabJSON()
    public void InitPrefabJSON()
    {
        prefabDescription = new PrefabJSON();
        prefabDescription.typeName = name.text;
        prefabDescription.title = name.text;
        if (name.text == "chair_1")
        {
            prefabDescription.appearances = new Appearance[3];
            prefabDescription.appearances[0] = new Appearance { name = "metal", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[1] = new Appearance { name = "plastic", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[2] = new Appearance { name = "seat", color = "#FFFFFF", texture = "base_material" };
        }
        if (name.text == "bed_1")
        {
            prefabDescription.appearances = new Appearance[5];
            prefabDescription.appearances[0] = new Appearance { name = "base", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[1] = new Appearance { name = "blanket", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[2] = new Appearance { name = "mattress", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[3] = new Appearance { name = "pillow", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[4] = new Appearance { name = "pillow 1", color = "#FFFFFF", texture = "base_material" };
        }
        if (name.text == "torchere_1")
        {
            prefabDescription.appearances = new Appearance[2];
            prefabDescription.appearances[0] = new Appearance { name = "base", color = "#FFFFFF", texture = "base_material" };
            prefabDescription.appearances[1] = new Appearance { name = "plafond", color = "#FFFFFF", texture = "base_material" };
        }
    }

    /// Set the prefab JSON description.
    /// @param prefab PrefabJSON object containing object description.
    /// @see PrefabJSON()
    public void SetPrefabJSON(PrefabJSON prefab)
    {
        this.prefabDescription = prefab;
    }

    /// Read the prefab JSON description.
    /// @note Function useful only when debugging.
    /// @see PrefabJSON()
    public void ReadPrefabJSON()
    {
        string JSONresult = JsonConvert.SerializeObject(this.prefabDescription);
        Debug.Log("Result : " + JSONresult);
    }
}

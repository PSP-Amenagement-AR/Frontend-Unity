using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    public Image icon;

    [SerializeField]
    private Text name;

    public Button myButton;

    [SerializeField]
    public ARTapToPlaceObject myPlane;

    public bool isItem;

    public PrefabJSON prefabDescription;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        ReadPrefabJSON();
        if (isItem)
        {
            myPlane.PreAddItem(prefabDescription.typeName);
        }
    }

    public void SetIcon(Sprite mySprite)
    {
        icon.sprite = mySprite;
    }

    public void SetName(string myName)
    {
        name.text = myName;
    }

    public void PutTransparency(float transparency)
    {
        Color color = icon.color;
        color.a = transparency;
        icon.color = color;
    }

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

    public void SetPrefabJSON(PrefabJSON prefab)
    {
        this.prefabDescription = prefab;
    }

    public void ReadPrefabJSON()
    {
        string JSONresult = JsonConvert.SerializeObject(this.prefabDescription);
        Debug.Log("Result : " + JSONresult);
    }
}

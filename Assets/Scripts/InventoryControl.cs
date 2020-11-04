using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryControl : MonoBehaviour
{
    private List<PlayerItem> playerInventory;

    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private GridLayoutGroup gridGroup;

    public ARItemsHandling aRItemsHandling;

    public bool isItemList;

    void Start()
    {
        buttonTemplate.SetActive(false);
        playerInventory = new List<PlayerItem>();

        if (isItemList)
            UpdateListItems();
        else
            UpdateListStages();

        GenInventory();
    }

    void UpdateListItems()
    {
        List<ARItemsHandling.ConfigItem>  configItems = aRItemsHandling.GetConfigItems();
        foreach (ARItemsHandling.ConfigItem configItem in configItems)
        {
            AddNewItem(configItem.name, configItem.SpritePath());
        }
    }

    void UpdateListStages()
    {
        AddNewItem("hall", "Sprites/Stages/hall");
        AddNewItem("kitchen", "Sprites/Stages/kitchen");
        AddNewItem("parent_room", "Sprites/Stages/parent_room");
        AddNewItem("room", "Sprites/Stages/room");
    }

    void AddNewItem(string name, string spritePath)
    {
        PlayerItem newItem = new PlayerItem();

        newItem.iconSprite = Resources.Load<Sprite>(spritePath);
        newItem.iconName = name;
        playerInventory.Add(newItem);

    }

    void GenInventory()
    {
        if (playerInventory.Count < 5)
        {
            gridGroup.constraintCount = playerInventory.Count;
        }
        else
        {
            gridGroup.constraintCount = 4;
        }

        foreach (PlayerItem newItem in playerInventory)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<InventoryButton>().SetIcon(newItem.iconSprite);
            newButton.GetComponent<InventoryButton>().SetName(newItem.iconName);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    public class PlayerItem
    {
        public Sprite iconSprite;
        public string iconName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the inventory button control.
/// </summary>
public class InventoryControl : MonoBehaviour
{
    /// List of PlayerItem objects in the inventory.
    /// @see PlayerItem
    public List<PlayerItem> playerInventory;

    /// GameObject for the button template.
    [SerializeField]
    public GameObject buttonTemplate;

    /// GameObject for the grid in the interface containing the inventory.
    [SerializeField]
    public GridLayoutGroup gridGroup;

    /// ARItemsHandling object for manage item handling.
    /// @see ARItemsHandling()
    public ARItemsHandling aRItemsHandling;

    /// Boolean indicating if the item is in the list.
    public bool isItemList;

    /// Initialization of PlayerItem list and update of stage and item inventory.
    /// @see PlayerItem
    public void Start()
    {
        buttonTemplate.SetActive(false);
        playerInventory = new List<PlayerItem>();

        if (isItemList)
            UpdateListItems();
        else
            UpdateListStages();

        GenInventory();
    }

    /// Update of the items list.
    public void UpdateListItems()
    {
        List<ARItemsHandling.ConfigItem>  configItems = aRItemsHandling.GetConfigItems();
        foreach (ARItemsHandling.ConfigItem configItem in configItems)
        {
            AddNewItem(configItem.name, configItem.SpritePath());
        }
    }

    /// Update of the stage list.
    public void UpdateListStages()
    {
        AddNewItem("hall", "Sprites/Stages/hall");
        AddNewItem("kitchen", "Sprites/Stages/kitchen");
        AddNewItem("parent_room", "Sprites/Stages/parent_room");
        AddNewItem("room", "Sprites/Stages/room");
    }

    /// Add of a new item in the items list.
    /// @param name Name of the item
    /// @param spritePath Paht of the sprite
    public void AddNewItem(string name, string spritePath)
    {
        PlayerItem newItem = new PlayerItem();

        newItem.iconSprite = Resources.Load<Sprite>(spritePath);
        newItem.iconName = name;
        playerInventory.Add(newItem);

    }

    /// Inventory generation.
    public void GenInventory()
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
            newButton.GetComponent<InventoryButton>().InitPrefabJSON();
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    /// Class containing item informations
    public class PlayerItem
    {
        /// Sprite object for the item sprite.
        public Sprite iconSprite;
        /// Name of the icon.
        public string iconName;
    }
}

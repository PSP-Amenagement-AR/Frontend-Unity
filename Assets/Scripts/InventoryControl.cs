﻿using System.Collections;
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

    [SerializeField]
    private Sprite[] iconSprites;

    [SerializeField]
    private string[] iconNames;

    void Start()
    {
        playerInventory = new List<PlayerItem>();
        int value;

        for (int i = 1; i <= 35; i++)
        {
            PlayerItem newItem = new PlayerItem();
            value = Random.Range(0, iconSprites.Length);

            newItem.iconSprite = iconSprites[value];
            newItem.iconName = iconNames[value];

            playerInventory.Add(newItem);
        }

        GenInventory();
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

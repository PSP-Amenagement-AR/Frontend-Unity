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
    private Image myItem;

    void Start()
    {
        playerInventory = new List<PlayerItem>();

        for (int i = 1; i <= 70; i++)
        {
            PlayerItem newItem = new PlayerItem();
            newItem.iconSprite = iconSprites[Random.Range(0, iconSprites.Length)];

            playerInventory.Add(newItem);
        }

        GenInventory();
    }

    void GenInventory()
    {
        if (playerInventory.Count < 11)
        {
            gridGroup.constraintCount = playerInventory.Count;
        }
        else
        {
            gridGroup.constraintCount = 10;
        }

        foreach (PlayerItem newItem in playerInventory)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<InventoryButton>().SetIcon(newItem.iconSprite);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    public class PlayerItem
    {
        public Sprite iconSprite;
    }
}

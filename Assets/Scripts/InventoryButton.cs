using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    public Image icon;

    //[SerializeField]
    private Image myItem;

    [SerializeField]
    private Text name;

    public Button myButton;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log(icon.sprite);
        myItem.GetComponent<Image>().sprite = icon.sprite;
    }

    public void SetIcon(Sprite mySprite)
    {
        icon.sprite = mySprite;
    }

    public void SetName(string myName)
    {
        name.text = myName;
    }
}

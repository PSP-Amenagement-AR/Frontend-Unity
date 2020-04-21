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
    private string name;

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
        name = myName;
    }

    // A VIRER
    /*void Update()
    {
        Debug.Log("Test");
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Click on button");
            Debug.Log(myIcon.sprite);
            Debug.Log("Next Step => Item apparition");
            //myItem.GetComponent<Item>().SetIcon(myIcon.sprite);
            myItem.GetComponent<Image>().sprite = myIcon.sprite;
            Debug.Log("Sprite Done");
        } else
        {
            Debug.Log("chelou");
        }
        Debug.Log("Test Final");
    }*/
}

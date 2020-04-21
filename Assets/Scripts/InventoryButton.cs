using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    public Image myIcon;

    [SerializeField]
    //private Item myItem;
    private Image myItem;

    public Button myButton;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log(myIcon.sprite);
        //myItem.GetComponent<Item>().SetIcon(myIcon.sprite);
        myItem.GetComponent<Image>().sprite = myIcon.sprite;
    }

    public void SetIcon(Sprite mySprite)
    {
        myIcon.sprite = mySprite;
    }

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

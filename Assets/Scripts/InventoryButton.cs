using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    public Image icon;

    //[SerializeField]
    //private Image myItem;

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
        GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeObj.transform.localScale = new Vector3(5, 5, 5);
        cubeObj.transform.position = new Vector3(0, 0, 0);
        cubeObj.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
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

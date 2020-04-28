using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    public Image icon;

    [SerializeField]
    private Text name;

    public Button myButton;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        // TODO put effect on the image when clicking button
        //this.PutTransparency(0.4f);
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
}

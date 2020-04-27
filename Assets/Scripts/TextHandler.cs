using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Text Text;

    public void OnPointerDown(PointerEventData eventData)
    {
        Text.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Color c;
        ColorUtility.TryParseHtmlString("#AAAAAA", out c); // grey
        Text.color = c;
    }
}

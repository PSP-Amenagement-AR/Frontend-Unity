using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class for manage text handler.
/// </summary>
public class TextHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// Text object for all text features.
    public Text Text;

    /// Set text color on white.
    /// @param eventData Event data
    public void OnPointerDown(PointerEventData eventData)
    {
        Text.color = Color.white;
    }

    /// Set text color on grey when the text is pointed up.
    /// @param eventData Event data
    public void OnPointerUp(PointerEventData eventData)
    {
        Color c;
        ColorUtility.TryParseHtmlString("#AAAAAA", out c); // grey
        Text.color = c;
    }
}

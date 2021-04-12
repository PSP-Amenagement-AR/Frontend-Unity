using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing the selection indicator.
/// </summary>
public class SelectionIndicator : MonoBehaviour
{
    /// ARItemsHandling object for manage the items handler.
    /// @see ARItemsHandling
    public ARItemsHandling ItemsHandler;

    /// Initiate the items handler.
    public void Start()
    {
        this.gameObject.SetActive(true);
        ItemsHandler = GameObject.FindObjectOfType<ARItemsHandling>();
    }

    /// Make appear the indicator.
    public void ShowIndicator()
    {
        if (!this.gameObject.GetComponentInChildren<Renderer>().enabled)
        {
            Debug.Log("Show indicator");
            this.gameObject.GetComponentInChildren<Renderer>().enabled = true;
        } 
    }

    /// Hide the indicator
    public void HideIndicator()
    {
        if (this.gameObject.GetComponentInChildren<Renderer>().enabled)
        {
            Debug.Log("Hide indicator");
            this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
        }
    }

    /// Manage the appearance of indicator.
    /// @note Function called once per frame.
    public void Update()
    {
        if (ItemsHandler.IsAnItemSelected())
        {
            Bounds bounds = ItemsHandler.SelectedItem.GetComponentInChildren<Renderer>().bounds;

            float diameter = bounds.size.z;

            this.transform.position = ItemsHandler.SelectedItem.transform.position;
            this.transform.localScale = new Vector3(diameter, 1, diameter);
            this.ShowIndicator();
        }
        else
        {
            this.HideIndicator();
        }
    }
}

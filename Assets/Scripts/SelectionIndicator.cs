using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    ARItemsHandling ItemsHandler;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(true);
        ItemsHandler = GameObject.FindObjectOfType<ARItemsHandling>();
    }

    void ShowIndicator()
    {
        if (!this.gameObject.GetComponentInChildren<Renderer>().enabled)
        {
            Debug.Log("Show indicator");
            this.gameObject.GetComponentInChildren<Renderer>().enabled = true;
        } 
    }

    void HideIndicator()
    {
        if (this.gameObject.GetComponentInChildren<Renderer>().enabled)
        {
            Debug.Log("Hide indicator");
            this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
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

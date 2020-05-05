using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    
    private Vector3 center = new Vector3(0.0f, 0.0f, 0.0f);

    public GameObject prefabFurniture;

    public void addFurniture() {
        GameObject furniture = Instantiate(prefabFurniture) as GameObject;
        positionToCenter(furniture.transform);
    }

    public void positionToCenter(Transform transform) {
        transform.position = center;
    }
}

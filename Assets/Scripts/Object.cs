using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{

    public GameObject cube;

    public void AddObject()
    {
        Instantiate(cube, transform.position, transform.rotation);
    }
}

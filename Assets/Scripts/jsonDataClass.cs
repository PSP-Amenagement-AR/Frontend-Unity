using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class jsonDataClass
{
    public string playerName;
    public List<ballList> balls;
    public List<loadinging> loadingImages;
}

[Serializable]
public class ballList
{
    public string name;
    public string description;
    public int price;
    public string image;
    public int size;
    public string weight;
    public string free;
}

[Serializable]
public class loadinging
{
    public string image;
}

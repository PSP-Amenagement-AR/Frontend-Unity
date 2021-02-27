using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Appearance
{
    public string name;
    public string color;
    public string texture;
}

public class PrefabJSON
{
    public string TypeName;
    public string title;
    public List<Appearance> appearances;
}



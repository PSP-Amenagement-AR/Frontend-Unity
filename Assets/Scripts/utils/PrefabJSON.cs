using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Class describing the design of a feature of a furniture. </summary>
public struct Appearance
{
    /// <summary>Name of the feature.</summary>
    public string name;
    /// <summary>Color of the feature.</summary>
    public string color;
    /// <summary>Texture of the feature.</summary>
    public string texture;
}

/// <summary>Class in reference to JSON object for virtual furniture description.</summary>
public class PrefabJSON
{
    /// <summary>Name of the template used.</summary>
    public string typeName;
    /// <summary>Title of the furniture created.</summary>
    public string title;
    /// <summary>Array of Appearance object.</summary>
    /// @see Appearance
    public Appearance[] appearances;
}



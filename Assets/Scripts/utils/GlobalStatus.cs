using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Global class with global parameters of application.</summary>
public class GlobalStatus
{
    /// <summary>URL to access server hosted and deployed on Heroku.</summary>
    public const string BaseUrl = "https://ar-amenagement-back.herokuapp.com";
    /// <summary> Instanciation of APIrequestManager object for manage API request with Back project.</summary>
    /// @see APIrequestManager
    public static APIrequestManager webApi = new APIrequestManager();
    /// <summary>Token variable for identify current user.</summary>
    /// @note The string is empty when no user is connected.
    public static string token = "";
}

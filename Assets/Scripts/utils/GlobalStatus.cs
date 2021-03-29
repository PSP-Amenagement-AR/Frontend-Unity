using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatus
{
    public const string BaseUrl = "https://ar-amenagement-back.herokuapp.com";
    public static APIrequestManager webApi = new APIrequestManager();
    public static string token = "";
}

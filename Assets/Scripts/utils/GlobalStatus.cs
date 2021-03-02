using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStatus
{
    public const string BaseUrl = "http://192.168.1.18:3000";
    public static APIrequestManager webApi = new APIrequestManager();
    public static string token = "";
}

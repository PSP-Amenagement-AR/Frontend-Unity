using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class APImanager : MonoBehaviour
{
    APIrequestManager webApi = new APIrequestManager();

    void Start()
    {
        var request = webApi.SendApiRequest("/users/register", "POST", new Users { email = "userTest@mail.com", password = "userTestPassord@2132" }); 
        JSONNode dataJSON = JSON.Parse(request.downloadHandler.text);
        Debug.Log("response" + request.responseCode + " : " + dataJSON["email"].Value + " / " + dataJSON["password"].Value);

        var request1 = webApi.SendApiRequest("/users/login", "POST", new Users { email = "userTest@mail.com", password = "userTestPassord@2132" });
        JSONNode dataJSON1 = JSON.Parse(request1.downloadHandler.text);
        Debug.Log("response" + request1.responseCode + " : " + dataJSON1["token"].Value);
    }
}

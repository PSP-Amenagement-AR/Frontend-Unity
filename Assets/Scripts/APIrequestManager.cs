﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using SimpleJSON;

public class APIrequestManager : MonoBehaviour
{   
    public string BaseUrl = "http://127.0.0.1:3000";

    UnityWebRequest CreateApiGetRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbGET, body);
    }

    UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(BaseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    UnityWebRequest CreateApiRequest(string url, string method, object body)
    {
        string bodyString = null;
        if (body is string)
        {
            bodyString = (string)body;
        }
        else if (body != null)
        {
            bodyString = JsonUtility.ToJson(body);
        }

        var request = new UnityWebRequest();
        request.url = url;
        request.method = method;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(bodyString) ? null : System.Text.Encoding.UTF8.GetBytes(bodyString));
        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 60;
        return request;
    }

    public UnityWebRequest SendApiRequest(string actionUrl, string method, object body = null)
    {
        UnityWebRequest request = new UnityWebRequest();
        switch (method)
        {
            case "GET":
                request = CreateApiGetRequest(actionUrl, body);
                break;
            case "POST":
                request = CreateApiPostRequest(actionUrl, body);
                break;
        }

        UnityWebRequestAsyncOperation requestHandel = request.SendWebRequest();
        requestHandel.completed += delegate(AsyncOperation pOperation) {
            Debug.Log(request.responseCode);
        };
        while (!requestHandel.isDone);

        return request;
    }
}
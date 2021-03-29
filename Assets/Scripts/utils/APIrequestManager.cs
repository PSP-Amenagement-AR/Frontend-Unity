using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using SimpleJSON;

/// <summary> Class for manage and execute API request with Back project.</summary>
/// @note The UnityWebRequest object is used to communicate with web server.
public class APIrequestManager : MonoBehaviour
{
    /// Function for create a GET API request.
    /// @param actionUrl The URL of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request executed from CreateApiRequest().
    /// @see CreateApiRequest()
    public UnityWebRequest CreateApiGetRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(GlobalStatus.BaseUrl + actionUrl, UnityWebRequest.kHttpVerbGET, body);
    }

    /// Function for create a POST API request.
    /// @param actionUrl The URL of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request executed from CreateApiRequest().
    /// @see CreateApiRequest()
    public UnityWebRequest CreateApiPostRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(GlobalStatus.BaseUrl + actionUrl, UnityWebRequest.kHttpVerbPOST, body);
    }

    /// Function for create a PUT API request.
    /// @param actionUrl The URL of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request executed from CreateApiRequest().
    /// @see CreateApiRequest()
    public UnityWebRequest CreateApiPutRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(GlobalStatus.BaseUrl + actionUrl, UnityWebRequest.kHttpVerbPUT, body);
    }

    /// Function for create a DELETE API request.
    /// @param actionUrl The URL of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request executed from CreateApiRequest().
    /// @see CreateApiRequest()
    public UnityWebRequest CreateApiDeleteRequest(string actionUrl, object body = null)
    {
        return CreateApiRequest(GlobalStatus.BaseUrl + actionUrl, UnityWebRequest.kHttpVerbDELETE, body);
    }

    /// Function for composed an API request from parameters send.
    /// @param url The URL of the request.
    /// @param method The method of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request.
    public UnityWebRequest CreateApiRequest(string url, string method, object body)
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
        request.SetRequestHeader("Authorization", GlobalStatus.token);
        request.timeout = 60;
        return request;
    }

    /// Function for send an API request.
    /// @param actionUrl The URL of the request.
    /// @param method The method of the request.
    /// @param body The body of the request.
    /// @returns a UnityWebRequest object of the request.
    /// @see CreateApiGetRequest() CreateApiPostRequest() CreateApiPutRequest() CreateApiDeleteRequest()
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
            case "PUT":
                request = CreateApiPutRequest(actionUrl, body);
                break;
            case "DELETE":
                request = CreateApiDeleteRequest(actionUrl, body);
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
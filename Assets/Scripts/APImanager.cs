using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class APImanager : MonoBehaviour
{
    public string jsonURL;

    void Start()
    {
        StartCoroutine(GetRequest());
    }

    IEnumerator GetRequest()
    {
        Debug.Log("Processing Data");
        /* WWW _www = new WWW(jsonURL);
         yield return _www;

         if (_www.error == null)
         {
             processJsonData(_www.text);
         }
         else
         {
             Debug.Log("Error something went wrong");
         }*/
        using (UnityWebRequest webRequest = UnityWebRequest.Get(jsonURL))
        {
            yield return webRequest.SendWebRequest();

            var N = JSON.Parse(webRequest.downloadHandler.text);
            /*string result = N["balls"][0]["name"].Value;
            Debug.Log(result);*/
            processJsonData(webRequest.downloadHandler.text);
        }
    }

    private void processJsonData(string data)
    {
        jsonDataClass jsnData = JsonUtility.FromJson<jsonDataClass>(data);

        foreach (ballList x in jsnData.balls)
        {
            Debug.Log(x.name + ": " + x.description);
        }
    }
}

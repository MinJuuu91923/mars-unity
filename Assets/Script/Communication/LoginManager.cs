using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

//아이디를 치는게 아니라 버튼을 누름으로서 진행하니 시작하기 버튼에 이 스크립트 넣어주시면 됩니다. 
public  class LoginManager : MonoBehaviour
{
    public static int SessionId; // 전역 접근 가능한 static 변수

    string loginUrl = "http://172.16.16.170:8081/auth/login";

    void Start()
    {
        StartCoroutine(Login("test", "test"));
    }

    IEnumerator Login(string username, string password)
    {
        var loginData = new
        {
            username = username,
            password = password
        };

        string jsonData = JsonConvert.SerializeObject(loginData);

        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);
            SessionId = response.sessionId;
            Debug.Log($"[Login Success] Session ID: {SessionId}");
            
        }
        else
        {
            Debug.LogError("Login Failed: " + request.error);
        }
    }

    class LoginResponse
    {
        public int sessionId;
        public string username;
        public bool success;
        public string message;
    }
}
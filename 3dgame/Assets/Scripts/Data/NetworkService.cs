using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
    public class NetworkService
    {
        public IEnumerator LoginRoutine(string email, Action<string> onSuccess, Action<string> onError)
        {
            Debug.Log("in login routine");
            string url = AppConfig.SERVER_ENDPOINT + "/user/" + email + "/profile/gamesPlayed";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(webRequest.error);
                }
                else
                {
                    onSuccess?.Invoke(webRequest.downloadHandler.text);
                }
            }
        }

        public IEnumerator FinishGameRoutine(string userEmail, Action onSuccess, Action<string> onError)
        {
            string url = AppConfig.SERVER_ENDPOINT + "/user/" + userEmail + "/finishGame";

            using (UnityWebRequest webRequest = UnityWebRequest.Put(url, ""))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError?.Invoke(webRequest.error);
                }
                else
                {
                    onSuccess?.Invoke();
                }
            }
        }
    }
}

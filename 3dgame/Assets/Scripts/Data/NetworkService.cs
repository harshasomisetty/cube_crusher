using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

        public IEnumerator AwardUser(string userEmail, Action<string> onSuccess, Action<string> onError)
        {
            string url = AppConfig.SERVER_ENDPOINT + "/user/" + userEmail + "/award";

            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.uploadHandler = new UploadHandlerRaw(new byte[0]);
                webRequest.SetRequestHeader("Content-Type", "application/json");

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

        public IEnumerator DownloadImage(string imageUrl, Image targetImage)
        {
            if (targetImage == null)
            {
                Debug.LogError("Image component is null.");
                yield break;
            }

            if (!targetImage.gameObject.activeInHierarchy)
            {
                Debug.LogError("Image component's GameObject is not active in the hierarchy.");
                yield break;
            }

            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download image: " + imageRequest.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                if (texture == null)
                {
                    Debug.LogError("Downloaded texture is null.");
                    yield break;
                }

                if (targetImage != null && targetImage.gameObject.activeInHierarchy)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    targetImage.sprite = sprite;
                    targetImage.preserveAspect = true;
                }
                else
                {
                    Debug.LogError("Image component or its GameObject became inactive after download.");
                }
            }
        }




    }
}

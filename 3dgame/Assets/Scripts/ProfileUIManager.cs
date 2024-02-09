using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Properties;

public class ProfileUIManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public GameObject loginUI;
    public TextMeshProUGUI profileNameText;
    public TextMeshProUGUI gamesPlayed;
    public GameObject profileDataUI;
    public GameObject logoutButton;

    private List<NFTData> nftList = new List<NFTData>();

    [System.Serializable]
    public class UserData
    {
        public string name;
        public List<NFTData> nfts;
    }

    [System.Serializable]
    public class NFTData
    {
        public string id;
        public string name;
        public string imageUrl;
        // Add other NFT attributes here
    }

    [Serializable]
    private class DataCollection
    {
        public AssetCollection assets;
    }

    [Serializable]
    private class AssetCollection
    {
        public AssetData[] data;
    }

    [Serializable]
    private class AssetData
    {
        public string id;
        public string name;
        public string imageUrl;
        public string environment;
        public Attributes[] attributes;
    }

    [Serializable]
    private class Attributes
    {
        public string value;
        public string traitType;
    }


    private void Start()
    {
        UpdateUI();
    }

    public void AttemptLogin()
    {
        string email = emailInputField.text;
        if (!string.IsNullOrEmpty(email))
        {
            StartCoroutine(LoginRoutine(email));
        }
    }

    private void PrintObject(object obj, int indentLevel = 0)
    {
        if (obj == null)
        {
            Debug.Log(new String(' ', indentLevel * 2) + "null");
            return;
        }

        string indent = new String(' ', indentLevel * 2);
        Type objType = obj.GetType();
        if (objType.IsPrimitive || objType == typeof(string))
        {
            Debug.Log(indent + obj);
        }
        else if (objType.IsArray)
        {
            IEnumerable enumerable = obj as IEnumerable;
            foreach (object child in enumerable)
            {
                PrintObject(child, indentLevel + 1);
            }
        }
        else
        {
            foreach (FieldInfo field in objType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = field.GetValue(obj);
                Debug.Log(indent + field.Name + ": ");
                PrintObject(value, indentLevel + 1);
            }
        }
    }


    private IEnumerator LoginRoutine(string email)
    {
        string url = AppConfig.SERVER_ENDPOINT + "/users/" + email;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                PlayerPrefs.SetString("UserEmail", email);
                ProcessResponse(webRequest.downloadHandler.text);
                UpdateUI();
            }
        }
    }

    private void ProcessResponse(string json)
    {
        Debug.Log("process " + json);
        try
        {
            // Assuming 'json' is the JSON string you received from the web request
            var dataCollection = JsonUtility.FromJson<DataCollection>(json);
            Debug.Log("all " + JsonUtility.ToJson(dataCollection, true));
            Debug.Log("single " + JsonUtility.ToJson(dataCollection.assets.data[0], true));

            foreach (var item in dataCollection.assets.data)
            {
                if (item.name == "Profile NFT")
                {
                    Debug.Log("found " + Array.Find(item.attributes, x => x.traitType == "Games Played").value);

                    PlayerPrefs.SetString("GamesPlayed", Array.Find(item.attributes, x => x.traitType == "Games Played").value);
                }

                NFTData nft = new NFTData
                {
                    id = item.id,
                    name = item.name,
                    imageUrl = item.imageUrl
                    // Set other fields as necessary
                };
                nftList.Add(nft);
            }
            UpdateUI(); // Make sure this method knows how to handle and display NFT data
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse JSON response: " + e.Message);
        }
    }



    public void Logout()
    {
        PlayerPrefs.DeleteKey("UserEmail");
        emailInputField.text = "";
        UpdateUI();
    }

    private void UpdateUI()
    {
        string userEmail = PlayerPrefs.GetString("UserEmail", "");
        string gamesPlayedString = PlayerPrefs.GetString("GamesPlayed", "");

        if (!string.IsNullOrEmpty(userEmail))
        {
            loginUI.SetActive(false);
            profileDataUI.SetActive(true);
            logoutButton.SetActive(true);
            profileNameText.text = "Email: " + userEmail;
            gamesPlayed.text = "Games Played: " + gamesPlayedString;
        }
        else
        {
            loginUI.SetActive(true);
            profileDataUI.SetActive(false);
            logoutButton.SetActive(false);
        }
    }
}

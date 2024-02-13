using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
    public class UserDataManager
    {

        public event Action OnUserDataUpdated;

        public void ProcessLoginResponse(string json)
        {
            var dataCollection = JsonUtility.FromJson<DataCollection>(json);
            PlayerPrefs.SetString("UserEmail", dataCollection.email);
            PlayerPrefs.SetString("GamesPlayed", dataCollection.gamesPlayed);
            PlayerPrefs.Save();
            OnUserDataUpdated?.Invoke();
        }

        public DataCollection GetUserData()
        {
            return new DataCollection
            {
                email = PlayerPrefs.GetString("UserEmail", ""),
                gamesPlayed = PlayerPrefs.GetString("GamesPlayed", "")
            };
        }

        public void LogoutUser()
        {
            PlayerPrefs.DeleteKey("UserEmail");
            PlayerPrefs.DeleteKey("GamesPlayed");
            OnUserDataUpdated?.Invoke();
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString("UserEmail", ""));
        }
    }

}
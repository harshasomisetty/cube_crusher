using System;
using UnityEngine;
using UnityEngine.UI;
using Data;
using System.Collections.Generic;
using System.Linq;
using TMPro;

[System.Serializable]
public class InventoryItems
{
    public InventoryItem[] inventory;
}


[System.Serializable]
public class InventoryItem
{
    public string name;
    public string description;
    public long created;
    public string imageUrl;
    public string mintAddress;
    public string priceCents;
    public Attribute[] attributes;
    public Owner owner;
}

[System.Serializable]
public class Attribute
{
    public string value;
    public string traitType;
}

[System.Serializable]
public class Owner
{
    public string address;
    public string referenceId;
}


public class InventoryManager : MonoBehaviour
{

    private NetworkService networkService;
    public GameObject Menu;

    public GameObject nftDisplayPrefab;
    public Transform contentPanel;

    void Start()
    {
        CloseBuyMenu();
        networkService = new NetworkService();

        FetchInventory();

    }

    private void onGetInventorySuccess(string json)
    {
        InventoryItems inventoryWrapper = JsonUtility.FromJson<InventoryItems>(json);
        if (inventoryWrapper == null || inventoryWrapper.inventory == null)
        {
            Debug.LogError("Failed to parse JSON.");
            return;
        }

        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventoryWrapper.inventory)
        {
            DisplayInventory(item);
        }
    }

    private void DisplayInventory(InventoryItem item)
    {
        GameObject nftDisplayObject = Instantiate(nftDisplayPrefab, contentPanel);

        TextMeshProUGUI titleText = nftDisplayObject.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        if (titleText != null)
        {
            titleText.text = item.name;
        }
        else
        {
            Debug.LogError("Title Text component not found.");
        }

        TextMeshProUGUI descText = nftDisplayObject.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
        descText.text = item.description;
        Image image = nftDisplayObject.transform.Find("Image").GetComponent<Image>();
        StartCoroutine(networkService.DownloadImage(item.imageUrl, image));


        nftDisplayObject.transform.SetParent(contentPanel, false);
    }


    public void FetchInventory()
    {
        string userEmail = PlayerPrefs.GetString("UserEmail", "");
        if (!string.IsNullOrEmpty(userEmail))
        {

            StartCoroutine(networkService.GetUserInventory(userEmail, onGetInventorySuccess, error =>
            {
                Debug.LogError("GetUserCharacters error: " + error);
            }));
        }
        else
        {
            Debug.LogError("UserEmail not set in PlayerPrefs");
        }
    }

    public void OpenBuyMenu()
    {
        Menu.SetActive(true);
    }

    public void CloseBuyMenu()
    {
        Menu.SetActive(false);
    }
}

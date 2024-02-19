using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabs; // Assign prefabs in the inspector

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int selectedPlayerID = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject playerToSpawn = playerPrefabs[selectedPlayerID];

        Vector3 spawnPosition = new Vector3(0, 0, 0);
        Quaternion spawnRotation = Quaternion.identity;

        Instantiate(playerToSpawn, spawnPosition, spawnRotation);
    }
}

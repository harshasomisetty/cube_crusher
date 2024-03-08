using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject enemyPrefab;

    void Start()
    {
        SpawnPlayer();
        SpawnEnemy();
    }

    void SpawnPlayer()
    {
        int selectedPlayerID = PlayerPrefs.GetInt("SelectedCharacter", 0);
        GameObject playerToSpawn = playerPrefabs[selectedPlayerID];

        Vector3 spawnPosition = new Vector3(0, 0, 0);
        Quaternion spawnRotation = Quaternion.identity;

        Instantiate(playerToSpawn, spawnPosition, spawnRotation);
    }


    void SpawnEnemy()
    {
        GameObject enemyParent = GameObject.Find("Enemies") ?? new GameObject("Enemies");
        Vector3 spawnPosition = new Vector3(5, 0.25f, 5);
        Quaternion spawnRotation = Quaternion.identity;


        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnRotation);
        enemy.transform.SetParent(enemyParent.transform);

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 100.0f, NavMesh.AllAreas))
            {
                enemy.transform.position = hit.position;
            }
            else
            {
                Debug.LogError("Could not place the enemy on the NavMesh. Spawn position: " + spawnPosition);
                // Here you can handle the failure to place the enemy on the NavMesh
                // For example, you might destroy the enemy, try a different position, or log additional information
            }
        }
        else
        {
            Debug.LogError("NavMeshAgent component not found on the enemy prefab.");
        }
    }



}
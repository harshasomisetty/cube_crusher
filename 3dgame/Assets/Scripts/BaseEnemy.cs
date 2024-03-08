using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
            return;
        }


        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        navMeshAgent.SetDestination(target.transform.position);

    }

}

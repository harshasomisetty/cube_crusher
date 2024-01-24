using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using System.Linq;

public class WaypointFollower : MonoBehaviour
{

    [SerializeField] GameObject[] waypoints;
    int currentWaypoint = 0;

    [SerializeField] float movementSpeed = 5f;

    void Start()
    {
        // need to make sure the waypoints are tagged as a Waypoint
        waypoints = transform.parent.GetComponentsInChildren<Transform>()
                            .Where(t => t != this.transform && t.CompareTag("Waypoint"))
                            .Select(t => t.gameObject)
                            .ToArray();

    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        if (UnityEngine.Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.1f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, movementSpeed * Time.deltaTime);

    }
}

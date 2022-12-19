using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWayfinder : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public List<Transform> vents = new List<Transform>();
    public int randomWaypoint;
    public NavMeshAgent navMesh;
    public EnemyHealth EnemyHealth;
    // Start is called before the first frame update
    void Awake()
    {
        EnemyHealth = this.gameObject.GetComponent<EnemyHealth>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.SetDestination(waypoints[0].position);
        randomWaypoint = 0;
    }
    void Update()
    {
        if (!EnemyHealth.isInjured)
        {
            navMesh.SetDestination(waypoints[randomWaypoint].position);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Waypoint" && (waypoints[randomWaypoint].name == other.gameObject.name))
        {
            randomWaypoint = Random.Range(0, waypoints.Count);
            while (waypoints[randomWaypoint].name == other.gameObject.name)
            {
                randomWaypoint = Random.Range(0, waypoints.Count);
            }
            navMesh.SetDestination(waypoints[randomWaypoint].position);
        }
    }
}

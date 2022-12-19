using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = -1;
    private float timer = 10;
    private NavMeshAgent navMesh;
    private InputMap controls;
    private EnemyWayfinder EnemyWayfinder;
    private int randoVar;
    private float ventDist1;
    private float ventDist2;
    private Transform ventToTravelTo;
    public bool isInjured = false;


    // Start is called before the first frame update
 
    protected private void OnEnable()
    {
        controls.Enable();
    }
    protected private void OnDisable()
    {
        controls.Disable();
    }
    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        controls = new InputMap();
        EnemyWayfinder = this.gameObject.GetComponent<EnemyWayfinder>();
    }
    void Start()
    {
        //navMesh.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if health = 0, stop the enemy for 10 seconds, then return the enemy to full health and they can move again

        if (controls.Movement.Jump.WasPressedThisFrame() && !isInjured)
        {
            isInjured = true;
            StartCoroutine(Stall());
        }
    }

    private IEnumerator Stall()
    {
        navMesh.isStopped = true;
        yield return new WaitForSeconds(5);
        ventDist1 = 0;
        ventDist2 = 0;
        for(int i = 0; i < EnemyWayfinder.vents.Count ; i++)
        {
            ventDist1 = Vector3.Distance(this.gameObject.transform.position, EnemyWayfinder.vents[i].transform.position);
            Debug.Log(ventDist1);
            if((ventDist1 < ventDist2 && ventDist2 != 0) || ventDist2 == 0)
            {
                ventToTravelTo = EnemyWayfinder.vents[i].transform;
                ventDist2 = ventDist1;
            }      
        }
        Debug.Log("I'm travelling to" + ventToTravelTo);
        EnemyWayfinder.navMesh.SetDestination(ventToTravelTo.position);
        navMesh.isStopped = false;
    }

    private IEnumerator Healing()
    {
        yield return new WaitForSeconds(5);
        ventDist1 = 0;
        ventDist2 = 0;
        for(int i = 0; i < EnemyWayfinder.waypoints.Count ; i++)
        {
            ventDist1 = Vector3.Distance(this.gameObject.transform.position, EnemyWayfinder.waypoints[i].transform.position);

            if((ventDist1 < ventDist2 && ventDist2 != 0) || ventDist2 == 0)
            {

                EnemyWayfinder.randomWaypoint = i;
                ventDist2 = ventDist1;
            }      
        }
        
        isInjured = false;
        Debug.Log("Ping");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vent" && isInjured == true)
        {
            StartCoroutine(Healing());
        }
    }
}
/*
        randoVar = waypointStealer.randomWaypoint;
        Debug.Log(waypointStealer.waypoints[randoVar].transform.position);
*/
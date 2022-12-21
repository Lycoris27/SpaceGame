using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Follow : MonoBehaviour
{
    public GameObject Player;
    public GameObject Companion;

    public Vector3 Dir;
    public NavMeshAgent compAgent;

    public int hearDistance;

    Vector3 moveLoc;

    public delegate void tempFunc(Vector3 InputV);
    tempFunc actionComp;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Companion = GameObject.FindGameObjectWithTag("Companion1");
        compAgent = Companion.GetComponent<NavMeshAgent>();
        
        hearDistance = 100;
        actionComp = stay;
    }

    void Update()
    {
        if (Vector3.Magnitude(Player.transform.position - Companion.transform.position) < hearDistance)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (actionComp == stay)
                {
                    actionComp = follow;
                }
                else
                {
                    actionComp = stay;
                }

                compAgent.stoppingDistance = 4;
            }

            if(Keyboard.current.zKey.wasPressedThisFrame)
            {
                actionComp = goLoc;
                moveLoc = findLoc();
                compAgent.stoppingDistance = 0;
            }
        }
        actionComp(moveLoc);
    }

    void stay(Vector3 temp)
    {
        GetComponent<NavMeshAgent>().destination = this.transform.position;
    }

    void follow(Vector3 temp)
    {
        GetComponent<NavMeshAgent>().destination = Player.transform.position;
    }

    Vector3 findLoc()
    {
        RaycastHit hit;
        
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit);
        Vector3 hitPos = hit.point;

        return hitPos;
    }

    void goLoc(Vector3 Loc)
    {
        GetComponent<NavMeshAgent>().destination = Loc;

        if(compAgent.remainingDistance == 0)
        {
            actionComp = stay;
        }
    }
}

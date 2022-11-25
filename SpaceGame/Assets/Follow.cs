using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 Dir;
    public Rigidbody thisRigid;


    void Start()
    {
        //this.gameobject.rigidbody
        Player = GameObject.FindGameObjectWithTag("PlayerHitbox");
    }

    // Update is called once per frame
    void Update()
    {
        
        GetComponent<NavMeshAgent>().destination = Player.transform.position;
    }
}

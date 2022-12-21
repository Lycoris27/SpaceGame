using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingPlayer : MonoBehaviour
{
    private float timer;
    public EnemyVision EnemyVision;
    public NavMeshAgent navMesh;
    public GameObject

    // Start is called before the first frame update
    void Awake()
    {
        EnemyVision = this.gameObject.GetComponent<EnemyVision>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyVision.canSeePlayer)
        {
            TrackPlayer();
        }
    }
    void TrackPlayer()
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectInv : MonoBehaviour
{
    public GameObject CraftInv;
    public GameObject CompanionCrafter;
    public GameObject Player;

    private MeshRenderer rendInv;

    public int DistanceCraft = 4;

    //public Vector3 currDist;

    // Start is called before the first frame update
    void Start()
    {
        CraftInv = GameObject.FindGameObjectWithTag("CraftInventory");
        CompanionCrafter = GameObject.FindGameObjectWithTag("Companion1");
        Player = GameObject.FindGameObjectWithTag("PlayerHitbox");

        rendInv = CraftInv.GetComponent<MeshRenderer>();
        rendInv.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(CompanionCrafter)
        {
            //currDist = Player.transform.position - CompanionCrafter.transform.position;
            if (Vector3.Magnitude(Player.transform.position - CompanionCrafter.transform.position) < DistanceCraft)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    if (rendInv.enabled)
                    {
                        rendInv.enabled = false;
                    }
                    else
                    {
                        rendInv.enabled = true;
                    }
                }
               
            }
            else
            {
                rendInv.enabled = false;
            }
        }
    }
}

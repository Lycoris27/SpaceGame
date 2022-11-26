using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectInv : MonoBehaviour
{
    public GameObject CraftInv;
    public GameObject CompanionCrafter;
    public GameObject Player;
    
    public FirstPersonController PlayerControl;

    public int DistanceCraft = 4;

    void Start()
    {
        CraftInv = GameObject.FindGameObjectWithTag("CraftInventory");   
        CompanionCrafter = GameObject.FindGameObjectWithTag("Companion1");
        Player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        PlayerControl = GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>();

        CraftInv.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(CompanionCrafter)
        {
            if (Vector3.Magnitude(Player.transform.position - CompanionCrafter.transform.position) < DistanceCraft)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    if (CraftInv.activeInHierarchy)
                    {
                        exitMenu();
                    }
                    else
                    {
                        enterMenu();
                    }
                }
               
            }
        }
    }

    
    public void exitMenu()
    {
        CraftInv.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerControl.enabled = true;
    }

    void enterMenu()
    {
        CraftInv.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerControl.enabled = false;
    }
}

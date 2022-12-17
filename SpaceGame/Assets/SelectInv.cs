using JetBrains.Annotations;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectInv : MonoBehaviour
{
    public GameObject Inv;
    private RectTransform InvTrans;

    public GameObject CraftInv;

    public GameObject CompanionCrafter;
    public GameObject Player;
    
    public FirstPersonController PlayerControl;

    public int DistanceCraft;
    public int widthGrid;
    public int heightGrid;
    public int numHotbarSlot;

    public GameObject ISprefab;
    private GameObject[,] slots;
    private GameObject[] HotbarSlot;

    void Start()
    {
        //The entire inventory object (UI)
        CraftInv = GameObject.FindGameObjectWithTag("CraftInventory");
        Inv = GameObject.FindGameObjectWithTag("Inventory");
        Inv.SetActive(false);
        CraftInv.SetActive(false);

        CompanionCrafter = GameObject.FindGameObjectWithTag("Companion1");
        Player = GameObject.FindGameObjectWithTag("PlayerHitbox");
        PlayerControl = GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>();

        //Initiate mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        DistanceCraft = 4;
        heightGrid = 3;
        widthGrid = 5;
        numHotbarSlot = 3;

        //Generate all inventory slots (generated because its easier to keep track). 
        ISprefab = (GameObject)Resources.Load("InventorySlot");
        InvTrans = Inv.GetComponent<RectTransform>();

        //Generate them at positions relative to the Inventory Object
        //Need to attach the prefab to the inventory as a child to have it display the texture. 
        slots = new GameObject[widthGrid, heightGrid];
        for (int x = 0; x < widthGrid; x++)
        {
            for (int y = 0; y < heightGrid; y++)
            {
                Vector3 temp = new Vector3(InvTrans.position.x - 350 + x * 100, 
                                           InvTrans.position.y + 300 - y * 100, //+ InvTrans.rect.height/2 - 300 - y * 100, 
                                           InvTrans.position.z);
                slots[x, y] = (GameObject)Instantiate(ISprefab, temp, Quaternion.identity);
                slots[x, y].transform.parent = Inv.transform;
                slots[x, y].name = "Slot_" + x + "_" + y;

                int tempX = x, tempY = y; //Need this for it to not reset
                slots[x, y].GetComponent<Button>().onClick.AddListener(() => selectSlot(tempX, tempY));
            }
        }

        //Inventory slots (2 x hands and a backpack slot)
        HotbarSlot = new GameObject[numHotbarSlot];
        for (int i = 0; i < numHotbarSlot; i++)
        {
            Vector3 temp = new Vector3(Inv.transform.position.x - 250 + i * 100,
                                       InvTrans.position.y - 350,
                                       InvTrans.position.z);
            HotbarSlot[i] = (GameObject)Instantiate(ISprefab, temp, Quaternion.identity);
            HotbarSlot[i].transform.parent = Inv.transform;
            HotbarSlot[i].name = "Hotbar_" + i;

            int tempI = i;
            HotbarSlot[i].GetComponent<Button>().onClick.AddListener(() => selectHotbar(tempI));
        }
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (CompanionCrafter)
            {
                if (Vector3.Magnitude(Player.transform.position - CompanionCrafter.transform.position) < DistanceCraft)
                {
                    if (CraftInv.activeInHierarchy)
                    {
                        exitCraftMenu();
                    }
                    else
                    {
                        enterCraftMenu();
                    }
                }
            }

            if (Inv.activeInHierarchy)
            {
                exitInvMenu();
            }
            else
            {
                enterInvMenu();
            }
        }
    }

    public void selectHotbar(int i)
    {
        GameObject hotbarSlot = GameObject.Find("Hotbar_" + i);
        Debug.Log(hotbarSlot.name);
    }
    public void selectSlot(int x, int y)
    {
        GameObject Slot = GameObject.Find("Slot_" + x + "_" + y);
        Debug.Log(Slot.name);
    }

    public void enterInvMenu()
    {
        Inv.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerControl.enabled = false;
    }

    public void exitInvMenu()
    {
        Inv.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerControl.enabled = true;
    }
    
    public void exitCraftMenu()
    {
        CraftInv.SetActive(false);
    }

    void enterCraftMenu()
    {
        CraftInv.SetActive(true);
    }
}

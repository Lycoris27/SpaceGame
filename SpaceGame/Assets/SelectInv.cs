using JetBrains.Annotations;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class SelectInv : MonoBehaviour
{
    private GameObject Inv;
    private RectTransform InvTrans;

    private GameObject CraftInv;
    private RectTransform CraftInvTrans;

    private GameObject CompanionCrafter;
    private GameObject Player;
    private GameObject playerCamera;
    
    private FirstPersonController PlayerControl;

    [SerializeField] private int DistanceCraft;
    private int widthGrid;
    private int heightGrid;
    private int numHotbarSlot;

    private int craftWidthGrid;
    private int craftHeightGrid;

    private int selectCollectableDist;

    private GameObject ISprefab;
    private GameObject[,] slots;
    private GameObject[] HotbarSlot;
    private GameObject[,] CraftingSlots;

    private GameObject Collectables;

    private GameObject plasticPrefab;
    private GameObject[] plasticObj;
    private int numPlastic;

    //Crafting data such as number of a certain item
    public int carryPlastic;

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

        //Variables
        DistanceCraft = 4;
        heightGrid = 3;
        widthGrid = 5;
        craftWidthGrid = 5;
        craftHeightGrid = 4;
        numHotbarSlot = 3;
        numPlastic = 10;
        selectCollectableDist = 3;

        carryPlastic = 0;

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
                slots[x, y] = Instantiate(ISprefab, temp, Quaternion.identity);
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
            HotbarSlot[i] = Instantiate(ISprefab, temp, Quaternion.identity);
            HotbarSlot[i].transform.parent = Inv.transform;
            HotbarSlot[i].name = "Hotbar_" + i;

            int tempI = i;
            HotbarSlot[i].GetComponent<Button>().onClick.AddListener(() => selectHotbar(tempI));
        }

        //Crafting slots (select these to get a pop up of the materials needed to craft)
        CraftingSlots = new GameObject[craftWidthGrid,craftHeightGrid];
        CraftInvTrans = CraftInv.GetComponent<RectTransform>();
        for (int x = 0; x < craftWidthGrid; x++)
        {
            for(int y = 0; y < craftHeightGrid; y++)
            {
                Vector3 temp = new Vector3(CraftInv.transform.position.x + x * 100,
                                       CraftInv.transform.position.y - y * 100,
                                       CraftInv.transform.position.z);
                CraftingSlots[x, y] = Instantiate(ISprefab, temp, Quaternion.identity);
                CraftingSlots[x, y].transform.parent = CraftInv.transform;
                CraftingSlots[x, y].name = "CS_" + x + "_" + y;

                int tempX = x, tempY = y;
                CraftingSlots[x, y].GetComponent<Button>().onClick.AddListener(() => selectCraft(tempX, tempY));
            }
        }

        Collectables = GameObject.Find("Collectables");

        //Generate Plastic objects randomly
        plasticPrefab = (GameObject)Resources.Load("PlasticObject");
        plasticObj = new GameObject[numPlastic];
        for(int plastic = 0; plastic < numPlastic; plastic++)
        {
            Vector3 pos = new Vector3(Random.Range(-36, 16), 2, Random.Range(-34, 10));
            plasticObj[plastic] = Instantiate(plasticPrefab, pos, Quaternion.identity);
            plasticObj[plastic].transform.parent = Collectables.transform;
            plasticObj[plastic].name = "Plastic_" + plastic;
        }
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
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

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            carryPlastic = checkGameObject(plasticObj, carryPlastic);
        }
    }

    public int checkGameObject(GameObject[] Array, int numObject)
    {
        for (int i = 0; i < Array.Length; i++)
        {
            //Checking if its not removed
            if (Array[i])
            {
                Vector3 tmp = Vector3.Normalize(Array[i].transform.position - Player.transform.position);
                //Debug.Log(Vector3.Angle(tmp, Camera.main.transform.forward));

                //Need to make sure facing is within certain angle as well
                if (Vector3.Magnitude(Player.transform.position - Array[i].transform.position) < selectCollectableDist &&
                    Vector3.Angle(tmp, Camera.main.transform.forward) < 15)
                {
                    numObject++;
                    Destroy(Array[i]);
                }
            }
        }

        return numObject;
    }

    public void selectCraft(int x, int y)
    {
        GameObject craftingSlot = CraftingSlots[x, y];
        Debug.Log(craftingSlot.name);
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

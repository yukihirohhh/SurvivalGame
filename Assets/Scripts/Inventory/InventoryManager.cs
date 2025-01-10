using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Weapon[] weapons;

    public bool opened;
    public KeyCode inventoryKey = KeyCode.Tab;

    [Header("Settings")]
    public int inventorySize = 24;
    public int hotbarSize = 6;

    [Header("Refs")]
    public GameObject dropModel;
    public Transform dropPos;
    public GameObject slotTemplate;
    public Transform contentHolder;
    public Transform hotbarContentHolder;

    

    [HideInInspector] public Slot[] inventorySlots;
    private Slot[] hotbarSlots;

    [SerializeField] private Slot[] allSlots;

    private void Start()
    {
        GenerateHotbarlots();
        Generateslots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            hotbarSlots[0].Try_Use();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            hotbarSlots[1].Try_Use();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            hotbarSlots[2].Try_Use();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            hotbarSlots[3].Try_Use();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            hotbarSlots[4].Try_Use();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            hotbarSlots[5].Try_Use();

        if (Input.GetKeyDown(inventoryKey))
          opened = !opened;

        if(opened)
        {
            transform.localPosition = new Vector3(0,0,0);
        }
        else
        {
            transform.localPosition = new Vector3 (-10000,0,0);
        }
    }


    private void Generateslots()
    {
        List<Slot> inventrySlots_ = new List<Slot>();
        List<Slot> allSlots_ = new List<Slot>();

        // GET THE ALL SLOTS ARAY INTO THE LOCAL LIST
        for(int i = 0; i < allSlots.Length; i++)
        {
            allSlots_.Add(allSlots[i]);
        }

        // GENERATE SLOTS
        for(int i = 0; i < inventorySize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject,contentHolder).GetComponent<Slot>();

            inventrySlots_.Add(slot);
            allSlots_.Add(slot);
        }

        inventorySlots = inventrySlots_.ToArray();
        allSlots = allSlots_.ToArray();
    }

    private void GenerateHotbarlots()
    {
        List<Slot> inventrySlots_ = new List<Slot>();
        List<Slot> hotbarList = new List<Slot> ();

        // GENERATE SLOTS
        for (int i = 0; i < hotbarSize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject, hotbarContentHolder).GetComponent<Slot>();

            inventrySlots_.Add(slot);
            hotbarList.Add(slot);
        }

        inventorySlots = inventrySlots_.ToArray();
        hotbarSlots = hotbarList.ToArray();
    }

    public void DragDrop(Slot from, Slot to)
    {
        // SWAPING
        if(from.data != to.data)
        {
            ItemSO data = to.data;
            int stackSize = to.stackSize;

            to.data = from.data;
            to.stackSize = from.stackSize;

            from.data = data;
            from.stackSize = stackSize;
        }
        // STACKING
        else
        {
            if(from.data.isStackable)
            {
                if(from.stackSize + to.stackSize > from.data.maxStack)
                {
                    int amountLeft = (from.stackSize + to.stackSize) - from.data.maxStack;

                    from.stackSize = amountLeft;

                    to.stackSize = to.data.maxStack;

                }
            }
            else
            {
                ItemSO data = to.data;
                int stackSize = to.stackSize;

                to.data = from.data;
                to.stackSize = from.stackSize;

                from.data = data;
                from.stackSize = stackSize;
            }
        }


        from.UpdateSlot();
        to.UpdateSlot();
    }

    public void AddItem(Pickup pickUp)
    {
        if (pickUp.data.isStackable)
        {
            Slot stackableSlot = null;

            // TRY FINDING STACKABLE SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].IsEmpty)
                {
                    if (inventorySlots[i].data == pickUp.data && inventorySlots[i].stackSize < pickUp.data.maxStack)
                    {
                        stackableSlot = inventorySlots[i];
                        break;
                    }

                }
            }

            if (stackableSlot != null)
            {

                // IF IT CANNOT FIT THE PICKED UP AMOUNT
                if (stackableSlot.stackSize + pickUp.stackSize > pickUp.data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + pickUp.stackSize) - pickUp.data.maxStack;



                    // ADD IT TO THE STACKABLE SLOT
                    stackableSlot.AddItemToSlot(pickUp.data, pickUp.data.maxStack);

                    // TRY FIND A NEW EMPTY STACK
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(pickUp.data, amountLeft);
                            inventorySlots[i].UpdateSlot();

                            break;
                        }
                    }



                    Destroy(pickUp.gameObject);
                }
                // IF IT CAN FIT THE PICKED UP AMOUNT
                else
                {
                    stackableSlot.AddStackAmount(pickUp.stackSize);

                    Destroy(pickUp.gameObject);
                }

                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;


                // FIND EMPTY SLOT
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                    emptySlot.UpdateSlot();

                    Destroy(pickUp.gameObject);
                }
                else
                {
                    pickUp.transform.position = dropPos.position;
                }
            }

        }
        else
        {
            Slot emptySlot = null;


            // FIND EMPTY SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                emptySlot.UpdateSlot();

                Destroy(pickUp.gameObject);
            }
            else
            {
                pickUp.transform.position = dropPos.position;
            }

        }
    }


    public void AddItem(ItemSO data, int stackSize)
    {
        if (data.isStackable)
        {
            Slot stackableSlot = null;

            // TRY FINDING STACKABLE SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].IsEmpty)
                {
                    if (inventorySlots[i].data == data && inventorySlots[i].stackSize < data.maxStack)
                    {
                        stackableSlot = inventorySlots[i];
                        break;
                    }

                }
            }

            if (stackableSlot != null)
            {

                // IF IT CANNOT FIT THE PICKED UP AMOUNT
                if (stackableSlot.stackSize + stackSize > data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + stackSize) - data.maxStack;



                    // ADD IT TO THE STACKABLE SLOT
                    stackableSlot.AddItemToSlot(data, data.maxStack);

                    // TRY FIND A NEW EMPTY STACK
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(data, amountLeft);
                            inventorySlots[i].UpdateSlot();

                            break;
                        }
                    }
                }
                // IF IT CAN FIT THE PICKED UP AMOUNT
                else
                {
                    stackableSlot.AddStackAmount(stackSize);
                }
                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;


                // FIND EMPTY SLOT
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(data, stackSize);
                    emptySlot.UpdateSlot();
                }
                else
                {
                    DropItem(data, stackSize);
                }
            }

        }
        else
        {
            Slot emptySlot = null;


            // FIND EMPTY SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(data, stackSize);
                emptySlot.UpdateSlot();
            }
            else
            {
                DropItem(data, stackSize);
            }
        }
    }

    public void DropItem(Slot slot)
    {
        Pickup pickup = Instantiate(dropModel, dropPos).AddComponent<Pickup>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);

        pickup.data = slot.data;
        pickup.stackSize = slot.stackSize;

        slot.Clean();
    }

    public void DropItem(ItemSO data, int stackSize)
    {
        Pickup pickup = Instantiate(dropModel, dropPos).AddComponent<Pickup>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);

        pickup.data = data;
        pickup.stackSize = stackSize;
    }
}

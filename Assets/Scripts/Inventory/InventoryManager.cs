using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool opened;
    public KeyCode inventoryKey = KeyCode.Tab;



    [Header("Refs")]
    public GameObject slotTemplate;
    public Transform contentHolder;

    [Header("Settings")]
    public int inventorySize = 24;

    private Slot[] inventorySlots;
    [SerializeField] private Slot[] allSlots;

    private void Start()
    {
        Generateslots();
    }

    private void Update()
    {
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

}

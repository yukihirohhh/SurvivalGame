using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemSO data;
    public int stackSize;
    [Space]
    public Image Icon;
    public Text stackText;


    private bool isEmpty;
    public bool IsEmpty => isEmpty;

    private void Start()
    {
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if(data == null)
        {
            isEmpty = true;

            Icon.gameObject.SetActive(false);
            stackText.gameObject.SetActive(false);
        }
        else
        {
            isEmpty = false;

            Icon.sprite = data.icon;
            stackText.text = $"x{stackSize}";

            Icon.gameObject.SetActive(true);
            stackText.gameObject.SetActive(true);
        }
    }

    public void AddItemToSlot(ItemSO data_,int stackSize_)
    {
        data = data_;
        stackSize = stackSize_;
    }

    public void AddStackAmount(int stackSize_)
    {
        stackSize += stackSize_;
    }

    public void Drop()
    {
        GetComponentInParent<InventoryManager>().DropItem(this);
    }

    public void Clean()
    {
        data = null;
        stackSize = 0;

        UpdateSlot();
    }
}

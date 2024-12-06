using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections.Concurrent;

public class Slot : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{

    private DragDropHandler dragDropHandler;
    private InventoryManager inventory;

    public ItemSO data;
    public int stackSize;
    [Space]
    public Image Icon;
    public Text stackText;


    private bool isEmpty;
    public bool IsEmpty => isEmpty;

    private void Start()
    {
        dragDropHandler = GetComponentInParent<DragDropHandler>();
        inventory = GetComponentInParent<InventoryManager>();

        UpdateSlot();
    }

    public void UpdateSlot()
    {

        if (data != null)
        {
            if (data.itemType != ItemSO.ItemType.Weapon)
            {
                if (stackSize <= 0)
                {
                    data = null;
                }
            }
        }


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

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!dragDropHandler.isDragging)
        {
            if(eventData.button == PointerEventData.InputButton.Left && !IsEmpty)
            {
                dragDropHandler.slotDraggedFrom = this;
                dragDropHandler.isDragging = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(dragDropHandler.isDragging)
        {
            // DROP
            if(dragDropHandler.slotDraggedTo == null)
            {
                dragDropHandler.slotDraggedFrom.Drop();
                dragDropHandler.isDragging = false;
            }
            // DRAG & DROP
            else if (dragDropHandler.slotDraggedTo != null)
            {
                inventory.DragDrop(dragDropHandler.slotDraggedFrom, dragDropHandler.slotDraggedTo);
                dragDropHandler.isDragging = false;
            }
        }
    }

    public void Try_Use()
    {
        if (data == null)
            return;
        if (data.itemType == ItemSO.ItemType.Consumable)
            Consume();
    }

    public void Consume()
    {
        PlayerStats stats = GetComponentInParent<PlayerStats>();

        stats.health += data.healthChange;
        stats.hunger += data.hungerChange;
        stats.thirst += data.thirstChange;

        stackSize--;

        UpdateSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(dragDropHandler.isDragging)
        {
            dragDropHandler.slotDraggedTo = this;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dragDropHandler.isDragging)
        {
            dragDropHandler.slotDraggedTo = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu (fileName = "New Item",menuName = "Survival Game/Inventory/New Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType { Generic, Consumable, Weapon, MeleeWeapon }

    [Header("General")]
    public ItemType itemType;
    public Sprite icon;
    public string itemName = "New Item";
    public string description = "New Item Description";
    [Space]
    public bool isStackable;
    public int maxStack = 1;

    [Header("Consumable")]
    public float healthChange = 10f;
    public float hungerChange = 10f;
    public float thirstChange = 10f;

}
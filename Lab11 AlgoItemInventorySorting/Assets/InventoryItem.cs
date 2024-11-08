using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryItem 
{
    public int ID;
    public string Name;
    public float Value;
    public InventoryItem(int id, string name, float value)
    {
        ID = id;
        Name = name;
        Value = value;
    }
    public void DisplayItemInfo()
    {
        Debug.Log($"Item ID: {ID}, Name: {Name}, Value: {Value}");
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class WeaponList
{
    public List<string> weapons; // List to hold weapon names & serialization for json
}

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int NumberofItems;

    private int minCost = 1000;
    private int maxCost = 100000;
    public string searchItemName;
    public int searchItemID;


    private List<string> weaponNames = new List<string>();
    void Start()
    {
        LoadWeaponNamesFromJSON();
        PopulateInventory();
        BinarySearchByID(searchItemID);
        QuickSortByValue(0, inventory.Count -1);
        LinearSearchByName(searchItemName);
    }

    void LoadWeaponNamesFromJSON()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "weaponlist.json");
        
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            WeaponList weaponList = JsonUtility.FromJson<WeaponList>(json);
            weaponNames = weaponList.weapons;  // Load the weapon names into the list
        }
        else
        {
            Debug.LogError("Weapon list JSON file not found!");
        }
    }
    void PopulateInventory()
    {
        // Ensure weaponNames is loaded and not empty
        if (weaponNames == null || weaponNames.Count == 0)
        {
            Debug.LogError("Weapon list is empty or not loaded properly.");
            return;
        }
        HashSet<int> usedIDs = new HashSet<int>();
        for (int i = 0; i < NumberofItems; i++)
        {
            int itemID;
            // Generate a unique itemID
            do
            {
                // Generate a dynamic random ID between 0 and NumberofItems
                itemID = Random.Range(0, NumberofItems);
            }
            while (usedIDs.Contains(itemID)); // Keep generating a new ID until it is unique

            // Add the unique itemID to the HashSet
            usedIDs.Add(itemID);

            string PickRandomWeapons = weaponNames[Random.Range(0, weaponNames.Count)];
            int RandomWeaponValueCost = Random.Range(minCost, maxCost);

            // Create a new InventoryItem with randomized ID and value
            InventoryItem newItem = new InventoryItem(itemID, PickRandomWeapons, RandomWeaponValueCost);

            // Add the item to the inventory list
            inventory.Add(newItem);
        }
    }

    void itemdisplay()
    {
        foreach(var item in inventory )
        {
            item.DisplayItemInfo();
        }
    }
        
      
    // Task 1: Linear Search for Finding an Item by Name
    public InventoryItem LinearSearchByName(string itemName)
    {
      
        foreach (var item in inventory)
        {
            if (item.Name == itemName)
            {
                Debug.LogWarning($"Linear search Item found: {item.Name} (ID: {item.ID}, Value cost: {item.Value} gold coins.)");
                return item; // Item found
            }
        }

        Debug.Log($"Item with name '{itemName}' not found.");
        return null; // Item not found
    }
    // Binary Search for Finding an Item by ID
    public InventoryItem BinarySearchByID(int itemID)
    {
        inventory.Sort((item1, item2) => item1.ID.CompareTo(item2.ID));
        Debug.Log("Sorting inventory by ID:");
        itemdisplay();
        int low = 0;
        int high = inventory.Count - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            InventoryItem midItem = inventory[mid];

            if (midItem.ID == itemID)
            {
                Debug.LogWarning($"Binary Item found: {midItem.Name} (ID: {midItem.ID}, Value cost: {midItem.Value} gold coins.)");
                // Return the item, but also display the inventory list
                
                return midItem; // Item found
            }

            if (midItem.ID > itemID)
            {
                high = mid - 1; // Search the left half
            }
            else
            {
                low = mid + 1; // Search the right half
            }
        }

        if (low > high)
        {
            Debug.Log($"Item with ID '{searchItemID}' not found.");
        }

        
        return null;
    }
    public void QuickSortByValue(int low, int high)
    {
        
        if (low < high)
        {   
            int pivotIndex = PartitionByValue(low, high);
            
            QuickSortByValue(low, pivotIndex - 1); // Sort left side
            QuickSortByValue(pivotIndex + 1, high); // Sort right side
        }
        if (low == 0 && high == inventory.Count - 1)
        {
            Debug.Log("Sorting inventory by value:");
            itemdisplay();  // Display the inventory only once after the entire sorting process
        }
    }

    // Partitioning the list for QuickSort by Value
    private int PartitionByValue(int low, int high)
    {
        
        InventoryItem pivot = inventory[high]; // Choose the last element as pivot
        int i = low - 1;
      

        // Rearrange the list so that items with value less than pivot come before it
        // and items with value greater than pivot come after it
        for (int j = low; j < high; j++)
        {
            if (inventory[j].Value <= pivot.Value)
            {
                i++;
                // Swap items[i] and items[j]
                InventoryItem temp = inventory[i];
                inventory[i] = inventory[j];
                inventory[j] = temp;
            }
        }
        
        // Swap the pivot element with items[i + 1]
        InventoryItem tempPivot = inventory[i + 1];
        inventory[i + 1] = inventory[high];
        inventory[high] = tempPivot;

        return i + 1; // Return the pivot index
        
    }
}


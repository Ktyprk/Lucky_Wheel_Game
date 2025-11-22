using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action OnInventoryChanged;

    private Dictionary<string, int> _inventoryData = new Dictionary<string, int>();
    private Dictionary<string, Sprite> _itemIcons = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(SliceData item)
    {
        if (item.Type == RewardType.Bomb) return;

        if (!_inventoryData.ContainsKey(item.Name))
        {
            _inventoryData.Add(item.Name, 0);
        }

        if (!_itemIcons.ContainsKey(item.Name))
        {
            _itemIcons.Add(item.Name, item.Icon);
        }

        _inventoryData[item.Name] += item.Amount;
        
        OnInventoryChanged?.Invoke();
    }

    public Dictionary<string, int> GetInventory()
    {
        return _inventoryData;
    }

    public Sprite GetIcon(string itemName)
    {
        return _itemIcons.ContainsKey(itemName) ? _itemIcons[itemName] : null;
    }

    public void ClearInventory()
    {
        _inventoryData.Clear();
        _itemIcons.Clear(); 
        
        OnInventoryChanged?.Invoke();
    }
}
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _contentParent; 
    [SerializeField] private InventoryItemSlot _itemPrefab;

    private Dictionary<string, InventoryItemSlot> _spawnedSlots = new Dictionary<string, InventoryItemSlot>();

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshInventory;
        RefreshInventory();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshInventory;
        }
    }

    private void RefreshInventory()
    {
        Dictionary<string, int> currentData = InventoryManager.Instance.GetInventory();
        
        if (currentData.Count == 0 && _spawnedSlots.Count > 0)
        {
            ClearAllSlots();
            return; 
        }

        foreach (var item in currentData)
        {
            string itemName = item.Key;
            int amount = item.Value;

            if (_spawnedSlots.ContainsKey(itemName))
            {
                _spawnedSlots[itemName].UpdateAmount(amount);
            }
            else
            {
                CreateNewSlot(itemName, amount);
            }
        }
    }
    
    private void ClearAllSlots()
    {
        foreach (var slot in _spawnedSlots.Values)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        
        _spawnedSlots.Clear();
    }

    private void CreateNewSlot(string itemName, int amount)
    {
        InventoryItemSlot newSlot = Instantiate(_itemPrefab, _contentParent);
        Sprite icon = InventoryManager.Instance.GetIcon(itemName);
        
        newSlot.Initialize(icon, amount);

        _spawnedSlots.Add(itemName, newSlot);
    }
}
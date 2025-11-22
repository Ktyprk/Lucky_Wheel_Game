using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public event Action<int> OnMoneyChanged;
    public event Action<int> OnGoldChanged;

    [Header("Starting Values")] 
    [SerializeField] private int _startingMoney = 1000; 
    [SerializeField] private int _startingGold = 500;   

    private int _currentMoney;
    private int _currentGold;

    private const string MONEY_KEY = "PlayerMoney";
    private const string GOLD_KEY = "PlayerGold";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadCurrencies();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(_currentMoney);
        OnGoldChanged?.Invoke(_currentGold);
    }

    public int GetMoney() => _currentMoney;
    public int GetGold() => _currentGold;
    
    public void AddCurrency(RewardType type, int amount)
    {
        switch (type)
        {
            case RewardType.Money:
                _currentMoney += amount;
                SaveCurrency(MONEY_KEY, _currentMoney);
                OnMoneyChanged?.Invoke(_currentMoney);
                break;

            case RewardType.Gold:
                _currentGold += amount;
                SaveCurrency(GOLD_KEY, _currentGold);
                OnGoldChanged?.Invoke(_currentGold);
                break;
        }
    }

    public bool TrySpendCurrency(RewardType type, int amount)
    {
        switch (type)
        {
            case RewardType.Money:
                if (_currentMoney >= amount)
                {
                    _currentMoney -= amount;
                    SaveCurrency(MONEY_KEY, _currentMoney);
                    OnMoneyChanged?.Invoke(_currentMoney);
                    return true;
                }
                break;

            case RewardType.Gold:
                if (_currentGold >= amount)
                {
                    _currentGold -= amount;
                    SaveCurrency(GOLD_KEY, _currentGold);
                    OnGoldChanged?.Invoke(_currentGold);
                    return true;
                }
                break;
        }
        return false;
    }

    private void SaveCurrency(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    private void LoadCurrencies()
    { 
        _currentMoney = PlayerPrefs.GetInt(MONEY_KEY, _startingMoney);  
        _currentGold = PlayerPrefs.GetInt(GOLD_KEY, _startingGold);   
    }
    
    [ContextMenu("Reset Save Data")]
    public void ResetData()
    {
        PlayerPrefs.DeleteKey(MONEY_KEY);
        PlayerPrefs.DeleteKey(GOLD_KEY);
        PlayerPrefs.Save();
        
        _currentMoney = _startingMoney;
        _currentGold = _startingGold;
        
        OnMoneyChanged?.Invoke(_currentMoney);
        OnGoldChanged?.Invoke(_currentGold);
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private WheelUIManager _uiManager;
    [SerializeField] private InventoryManager _inventoryManager;
    [SerializeField] private PopupManager _popupManager;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _safeZoneText;
    [SerializeField] private TextMeshProUGUI _superZoneText;
    
    [Header("Buttons")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _playButton;

    [Header("Settings")]
    [SerializeField] private int _reviveCost = 500;
    [SerializeField] private int _gameStartCost = 25;

    private int _currentLevel = 1;

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitButtonPressed);
        _playButton.onClick.AddListener(OnPlayClicked);

        _popupManager.ClaimButton.onClick.AddListener(OnClaimClicked);
        _popupManager.ContinueButton.onClick.AddListener(OnExitContinueClicked);
        
        _popupManager.GiveUpButton.onClick.AddListener(OnGiveUpClicked);
        _popupManager.ReviveButton.onClick.AddListener(OnReviveClicked);

        SetupMainMenuState();
    }

    private void SetupMainMenuState()
    {
        _currentLevel = 1;
        
        _playButton.gameObject.SetActive(true);
        _uiManager.SetSpinButtonActive(false);
        _exitButton.interactable = false; 

        UpdateLevelContext();
    }

    private void StartGameplayLoop()
    {
        _inventoryManager.ClearInventory();

        _playButton.gameObject.SetActive(false);
        _uiManager.SetSpinButtonActive(true);
        _uiManager.SetButtonInteractable(true);
        _exitButton.interactable = true;

        UpdateLevelContext();
    }

    private void OnPlayClicked()
    {
        if (CurrencyManager.Instance.TrySpendCurrency(RewardType.Gold, _gameStartCost))
        {
            StartGameplayLoop();
        }
        else
        {
            Debug.LogWarning("NOT ENOUGH MONEY");
        }
    }

    private void UpdateLevelContext()
    {
        _levelText.text = $"LEVEL {_currentLevel}";

        int wheelIndex = 0;

        if (_currentLevel % 30 == 0)
            wheelIndex = 2;
        else if (_currentLevel % 5 == 0)
            wheelIndex = 1;
        else
            wheelIndex = 0;

        _uiManager.SelectWheel(wheelIndex);
        UpdateZoneTexts();
    }

    private void UpdateZoneTexts()
    {
        int nextSafeZone = ((_currentLevel / 5) + 1) * 5;
        int nextSuperZone = ((_currentLevel / 30) + 1) * 30;

        _safeZoneText.text = nextSafeZone.ToString();
        _superZoneText.text = nextSuperZone.ToString();
    }

    public void ProcessTurnResult(RewardType rewardType)
    {
        if (rewardType == RewardType.Bomb)
        {
            _popupManager.ShowDeathPopup();
        }
        else
        {
            AdvanceLevel();
        }
    }

    private void AdvanceLevel()
    {
        _currentLevel++;
        UpdateLevelContext();
        _uiManager.SetButtonInteractable(true);
    }

    private void OnExitButtonPressed()
    {
        _popupManager.ShowExitPopup();
    }

    private void OnClaimClicked()
    {
        SaveSessionRewardsToWallet();
        _popupManager.HideExitPopup();
        _inventoryManager.ClearInventory();
        SetupMainMenuState();
    }

    private void OnExitContinueClicked()
    {
        _popupManager.HideExitPopup();
    }

    private void OnGiveUpClicked()
    {
        _popupManager.HideDeathPopup();
        _inventoryManager.ClearInventory();
        SetupMainMenuState();
    }

    private void OnReviveClicked()
    {
        if (CurrencyManager.Instance.TrySpendCurrency(RewardType.Gold, _reviveCost))
        {
            _popupManager.HideDeathPopup();
            _uiManager.SetButtonInteractable(true);
        }
        else
        {
            Debug.LogWarning("NOT ENOUGH MONEY");
        }
    }

    private void SaveSessionRewardsToWallet()
    {
        var inventory = _inventoryManager.GetInventory();

        foreach (var item in inventory)
        {
            if (item.Key == "Money") 
            {
                CurrencyManager.Instance.AddCurrency(RewardType.Money, item.Value);
            }
            else if (item.Key == "Gold")
            {
                CurrencyManager.Instance.AddCurrency(RewardType.Gold, item.Value);
            }
        }
    }
}
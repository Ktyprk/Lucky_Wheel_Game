using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private Button _exitButton;

    private int _currentLevel = 1;

    private void Start()
    {
        _exitButton.onClick.AddListener(OnExitButtonPressed);

        _popupManager.ClaimButton.onClick.AddListener(OnClaimClicked);
        _popupManager.ContinueButton.onClick.AddListener(OnExitContinueClicked);
        
        _popupManager.GiveUpButton.onClick.AddListener(OnGiveUpClicked);
        _popupManager.ReviveButton.onClick.AddListener(OnReviveClicked);

        StartGame();
    }

    private void StartGame()
    {
        _currentLevel = 1;
        _inventoryManager.ClearInventory();
        UpdateLevelContext();
        _uiManager.SetButtonInteractable(true);
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
        _popupManager.HideExitPopup();
        
        _inventoryManager.ClearInventory();
        _currentLevel = 1;
        UpdateLevelContext();
        _uiManager.SetButtonInteractable(true);
    }

    private void OnExitContinueClicked()
    {
        _popupManager.HideExitPopup();
    }

    private void OnGiveUpClicked()
    {
        _popupManager.HideDeathPopup();
        
        _inventoryManager.ClearInventory();
        _currentLevel = 1;
        UpdateLevelContext();
        _uiManager.SetButtonInteractable(true);
    }

    private void OnReviveClicked()
    {
        _popupManager.HideDeathPopup();
        _uiManager.SetButtonInteractable(true);
    }
}
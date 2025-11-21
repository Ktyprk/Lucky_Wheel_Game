using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [Header("Exit Popup References")]
    [SerializeField] private GameObject _exitPopupPanel;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Button _continueButton;

    [Header("Death Popup References")]
    [SerializeField] private GameObject _deathPopupPanel;
    [SerializeField] private Button _giveUpButton;
    [SerializeField] private Button _reviveButton;

    public Button ClaimButton => _claimButton;
    public Button ContinueButton => _continueButton;
    public Button GiveUpButton => _giveUpButton;
    public Button ReviveButton => _reviveButton;

    public void ShowExitPopup()
    {
        _exitPopupPanel.SetActive(true);
    }

    public void HideExitPopup()
    {
        _exitPopupPanel.SetActive(false);
    }

    public void ShowDeathPopup()
    {
        _deathPopupPanel.SetActive(true);
    }

    public void HideDeathPopup()
    {
        _deathPopupPanel.SetActive(false);
    }
}
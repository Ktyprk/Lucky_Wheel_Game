using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _goldText;

    private void Start()
    {
        if (CurrencyManager.Instance == null) return;

        CurrencyManager.Instance.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(CurrencyManager.Instance.GetMoney());

        CurrencyManager.Instance.OnGoldChanged += UpdateGoldText;
        UpdateGoldText(CurrencyManager.Instance.GetGold());
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance == null) return;

        CurrencyManager.Instance.OnMoneyChanged -= UpdateMoneyText;
        CurrencyManager.Instance.OnGoldChanged -= UpdateGoldText;
    }

    private void UpdateMoneyText(int amount)
    {
        if (_moneyText != null)
        {
            _moneyText.text = amount.ToString("N0");
        }
    }

    private void UpdateGoldText(int amount)
    {
        if (_goldText != null)
        {
            _goldText.text = amount.ToString("N0");
        }
    }
}
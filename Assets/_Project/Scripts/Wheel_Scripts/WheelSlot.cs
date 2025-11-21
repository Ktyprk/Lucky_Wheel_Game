using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class WheelSlot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _amountText;

    public SliceData CurrentData { get; private set; }

    public void SetContent(SliceData data)
    {
        CurrentData = data; 
        UpdateIcon(data.Icon);
        UpdateText(data);
    }

    private void UpdateIcon(Sprite icon)
    {
        if (_iconImage == null) return;
        _iconImage.sprite = icon;
        _iconImage.enabled = icon != null;
        _iconImage.preserveAspect = true;
    }

    private void UpdateText(SliceData data)
    {
        if (_amountText == null) return;
        bool hasAmount = data.Type == RewardType.Currency || data.Type == RewardType.Item;
        
        _amountText.text = hasAmount ? data.Amount.ToString() : data.Name;
        _amountText.gameObject.SetActive(hasAmount);
    }
}
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

        bool hasAmount = data.Type == RewardType.Money || 
                         data.Type == RewardType.Gold || 
                         data.Type == RewardType.Item;
    
        if (hasAmount && data.Amount > 0)
        {
            _amountText.text = $"x{data.Amount}"; 
            _amountText.gameObject.SetActive(true);
        }
        else
        {
            _amountText.text = data.Name;
            _amountText.gameObject.SetActive(!string.IsNullOrEmpty(data.Name));
        }
    }
}
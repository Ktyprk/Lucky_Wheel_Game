using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // Opsiyonel: Sayı artarken ufak bir efekt için

public class InventoryItemSlot : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;

    private int _currentAmount;

    // İlk kez oluşturulurken çalışır
    public void Initialize(Sprite icon, int initialAmount)
    {
        if (_iconImage != null)
        {
            _iconImage.sprite = icon;
            _iconImage.preserveAspect = true;
        }
        UpdateAmount(initialAmount);
    }

    // Zaten varsa sadece bu metod çalışır
    public void UpdateAmount(int newAmount)
    {
        _currentAmount = newAmount;
        _countText.text = $"x{_currentAmount}";

        // Ufak bir "Pop" efekti (DOTween varsa) - Oyuncuya geri bildirim verir
        transform.DOScale(1.1f, 0.1f).OnComplete(() => transform.DOScale(1f, 0.1f));
    }
}
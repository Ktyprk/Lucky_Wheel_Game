using UnityEngine;

public class WheelIndicator : MonoBehaviour
{
    private WheelSlot _currentSlot;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out WheelSlot slot))
        {
            _currentSlot = slot;
        }
    }

    public SliceData GetWinningReward()
    {
        if (_currentSlot != null)
        {
            return _currentSlot.CurrentData;
        }
        
        return new SliceData();
    }
}
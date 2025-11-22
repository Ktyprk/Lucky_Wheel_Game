using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WheelUIManager : MonoBehaviour
{
    [SerializeField] private Button _spinButton;
    [SerializeField] private List<WheelController> _allWheels;

    private WheelController _activeWheel;

    private void OnEnable()
    {
        if (_spinButton != null)
        {
            _spinButton.onClick.AddListener(OnSpinButtonClicked);
        }
    }

    private void OnDisable()
    {
        if (_spinButton != null)
        {
            _spinButton.onClick.RemoveListener(OnSpinButtonClicked);
        }
    }

    public void SelectWheel(int index)
    {
        if (_allWheels == null || _allWheels.Count == 0) return;

        if (index < 0 || index >= _allWheels.Count) index = 0;

        _activeWheel = _allWheels[index];

        for (int i = 0; i < _allWheels.Count; i++)
        {
            if (_allWheels[i] != null)
            {
                bool isActive = (i == index);
                _allWheels[i].gameObject.SetActive(isActive);
                
                if (isActive)
                {
                    _allWheels[i].ResetWheelState();
                }
            }
        }
    }

    public WheelController GetActiveWheel()
    {
        return _activeWheel;
    }

    public void SetButtonInteractable(bool isInteractable)
    {
        if (_spinButton != null)
        {
            _spinButton.interactable = isInteractable;
        }
    }
    
    public void SetSpinButtonActive(bool isActive)
    {
        if (_spinButton != null)
        {
            _spinButton.gameObject.SetActive(isActive);
        }
    }

    private void OnSpinButtonClicked()
    {
        if (_activeWheel == null) return;

        _activeWheel.SpinWheel();
        SetButtonInteractable(false);
       
    }
}
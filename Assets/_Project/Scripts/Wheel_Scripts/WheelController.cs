using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;


public class WheelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WheelConfiguration _wheelConfig;
    [SerializeField] private List<WheelSlot> _wheelSlots;
    [SerializeField] private WheelIndicator _indicator;
    [SerializeField] private Transform _wheelTransform; 

    [Header("Settings")]
    [SerializeField] private int _minSpins = 5;
    [SerializeField] private float _duration = 4f; 

    private const int SLOT_COUNT = 8;
    private const float ANGLE_PER_SLOT = 360f / SLOT_COUNT;
    private bool _isSpinning = false;

    private void Start()
    {
        InitializeWheel();
    }

    public void SpinWheel()
    {
        if (_isSpinning) return;
        
        _isSpinning = true;

        int randomSlotIndex = Random.Range(0, SLOT_COUNT);
        float spinAngle = (360f * _minSpins) + (randomSlotIndex * ANGLE_PER_SLOT);
        
        Vector3 targetRotation = new Vector3(0, 0, -spinAngle);

        _wheelTransform
            .DORotate(targetRotation, _duration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart)
            .SetLink(gameObject)
            .OnComplete(OnSpinCompleted);
    }

    private void OnSpinCompleted()
    {
        _isSpinning = false;
        CheckReward();
    }

    private void CheckReward()
    {
        SliceData wonSlice = _indicator.GetWinningReward();
        
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        if(wonSlice.Type != RewardType.Bomb)
        {
            InventoryManager.Instance.AddItem(wonSlice);
            levelManager.ProcessTurnResult(wonSlice.Type);
        }
        else
        {
            levelManager.ProcessTurnResult(RewardType.Bomb);
        }
    }

    private void InitializeWheel()
    {
        if (_wheelConfig == null || _wheelSlots.Count != SLOT_COUNT) return;

        List<SliceData> preparedSlices = CalculateSlices();

        for (int i = 0; i < _wheelSlots.Count; i++)
        {
            _wheelSlots[i].SetContent(preparedSlices[i]);
        }
    }

    private List<SliceData> CalculateSlices()
    {
        List<SliceData> allRewards = new List<SliceData>(_wheelConfig.Slices);
        List<SliceData> selectedRewards = new List<SliceData>();
        
        bool shouldExcludeBomb = _wheelConfig.IsSafeZone || _wheelConfig.IsSuperZone;

        if (!shouldExcludeBomb)
        {
            SliceData bombItem = allRewards.FirstOrDefault(x => x.Type == RewardType.Bomb);
            if (!string.IsNullOrEmpty(bombItem.Name))
            {
                selectedRewards.Add(bombItem);
            }
        }

        var filteredRewards = allRewards
            .Where(x => x.Type != RewardType.Bomb)
            .OrderBy(x => Random.value);

        int neededCount = SLOT_COUNT - selectedRewards.Count;
        selectedRewards.AddRange(filteredRewards.Take(neededCount));

        return selectedRewards.OrderBy(x => Random.value).ToList();
    }
    
    public void ResetWheelState()
    {
        _isSpinning = false;
        if (_wheelTransform != null)
        {
            _wheelTransform.rotation = Quaternion.identity;
        }
        
        InitializeWheel();
    }

}
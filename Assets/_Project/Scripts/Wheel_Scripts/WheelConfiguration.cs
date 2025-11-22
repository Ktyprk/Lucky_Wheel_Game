using UnityEngine;
using System.Collections.Generic;

public enum RewardType
{
    Money,
    Gold,
    Item,
    Bomb,
    
}

[System.Serializable]
public struct SliceData
{
    public string Name;
    public RewardType Type;
    public int Amount;
    public Sprite Icon;
}

[CreateAssetMenu(fileName = "WheelConfig", menuName = "Game/Wheel Configuration")]
public class WheelConfiguration : ScriptableObject
{
    public List<SliceData> Slices = new List<SliceData>();
    public bool IsSafeZone;
    public bool IsSuperZone;
}
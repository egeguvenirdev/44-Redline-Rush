using System;
using UnityEngine;

public static class ActionManager
{
    //Game Actions
    public static Action GameStart { get; set; }
    public static Action<bool> GameEnd { get; set; }
    public static Action<float> Updater { get; set; }
    public static Func<int> GameLevel { get; set; }
    public static Action<AudioType> PlayClip { get; set; }

    //Player Action
    public static Action<float> SwerveValue { get; set; }
    //public static Action<UpgradeType> GamePlayUpgrade {get; set;}
    //public static Func<float> GetUpgradeValue {get; set;}

    //Cam Actions

    //Money Actions
    public static Action<float> UpdateMoney { get; set; }
    public static Action<float> UpdateMoneyMultiplier { get; set; }
    public static Func<float, bool> CheckMoneyAmount { get; set; }


    //Upgrade
    public static Action<UpgradeType, float> GamePlayUpgrade { get; set; }
    public static Action<UpgradeType, float> AddTempUpgrade { get; set; }
    public static Action<UpgradeType> ClearTempUpgrade { get; set; }
    public static Action ClearAllTempUpgrades { get; set; }
    public static Action OnUpgradePurchased { get; set; }

    //Visual Effects
    public static Func<Vector3, Vector3> GetOrtographicScreenToWorldPoint { get; set; }
}

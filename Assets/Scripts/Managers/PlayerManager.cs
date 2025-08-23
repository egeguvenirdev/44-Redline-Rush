using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ManagerBase
{
    [Header("Components")]
    [SerializeField] private RunnerScript runnerScript;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private GameObject[] carPrefabs;
    private UpgradeManager upgradeManager;

    [SerializeField] private float baseHorsePower = 10f;
    [SerializeField] private float baseBoost = 1f;
    [SerializeField] private int baseCarLevel = 1;

    public float CurrentHorsePower { get; set; }
    public float CurrentBaseBoost { get; set; }
    public int CurrentCarLevel { get; set; }
    public Transform GetCharacterTransform
    {
        get => characterTransform;
    }

    //Permanent Upgres
    private readonly Dictionary<UpgradeType, float> permUpgradeMultipliers = new();
    //Temp Upgrades
    private readonly Dictionary<UpgradeType, float> tempMultipliers = new();

    private void OnEnable()
    {
        ActionManager.GamePlayUpgrade += OnGamePlayUpgrade;
        ActionManager.AddTempUpgrade += OnAddTempUpgrade;
        ActionManager.ClearTempUpgrade += OnClearTempUpgrade;
        ActionManager.ClearAllTempUpgrades += OnClearAllTempUpgrades;

        upgradeManager = FindFirstObjectByType<UpgradeManager>();

        OnGameStart();
        RefreshActiveCar();
    }

    public override void Init()
    {
        runnerScript.Init();
        upgradeManager.Init();
    }

    public override void DeInit()
    {
        runnerScript.DeInit();
        upgradeManager.DeInit();

        ActionManager.GamePlayUpgrade -= OnGamePlayUpgrade;
        ActionManager.AddTempUpgrade -= OnAddTempUpgrade;
        ActionManager.ClearTempUpgrade -= OnClearTempUpgrade;
        ActionManager.ClearAllTempUpgrades -= OnClearAllTempUpgrades;
    }

    #region Event Handlers
    private void OnGameStart()
    {
        ResetTempUpgrades();
        upgradeManager.PublishAll();
        RecomputeAllStats();
    }

    private void OnGamePlayUpgrade(UpgradeType upgradeType, float permValue)
    {
        permUpgradeMultipliers[upgradeType] = permValue;
        RecomputeAllStats();
    }

    private void OnAddTempUpgrade(UpgradeType upgradeType, float multipliersDelta)
    {
        if (!tempMultipliers.TryGetValue(upgradeType, out var m)) m = 1;
        m *= Mathf.Max(0f, multipliersDelta <= 0f ? 1f : multipliersDelta);
        tempMultipliers[upgradeType] = m;

        RecomputeAllStats();
    }

    private void OnClearTempUpgrade(UpgradeType upgradeType)
    {
        tempMultipliers.Remove(upgradeType);
        RecomputeOneStat(upgradeType);
    }

    private void OnClearAllTempUpgrades()
    {
        ResetTempUpgrades();
        RecomputeAllStats();
    }
    #endregion


    #region Recompute

    private void RecomputeOneStat(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.CarHorsePower:
                CurrentHorsePower = Effective(baseHorsePower, upgradeType);
                break;

            case UpgradeType.CarBoost:
                CurrentBaseBoost = Effective(baseBoost, upgradeType);
                break;
            case UpgradeType.CarLevel:
                CurrentCarLevel = (int)Effective(baseCarLevel, upgradeType);
                RefreshActiveCar();
                break;
        }
    }

    private void RecomputeAllStats()
    {
        CurrentHorsePower = Effective(baseHorsePower, UpgradeType.CarHorsePower);
        CurrentBaseBoost = Effective(baseBoost, UpgradeType.CarBoost);
        CurrentCarLevel = (int)Effective(baseCarLevel, UpgradeType.CarLevel);

        RefreshActiveCar();
    }

    private float Effective(float baseValue, UpgradeType upgradeType) => baseValue * GetPermMul(upgradeType) * GetTempMul(upgradeType);

    public float GetEffective(UpgradeType upgradeType)
    {
        float baseValue = 0;

        switch (upgradeType)
        {
            case UpgradeType.CarHorsePower:
                baseValue = baseHorsePower;
                break;

            case UpgradeType.CarBoost:
                baseValue = baseBoost;
                break;
            case UpgradeType.CarLevel:
                baseValue = baseCarLevel;
                break;
        }

        return Effective(baseValue, upgradeType);
    }

    private float GetPermMul(UpgradeType upgradeType, float fallback = 1f)
    {
        if (!permUpgradeMultipliers.TryGetValue(upgradeType, out var mul))
        {
            mul = upgradeManager.GetUpgradeValue(upgradeType, 1f);
            permUpgradeMultipliers[upgradeType] = mul;
        }

        return mul;
    }

    private float GetTempMul(UpgradeType upgradeType) =>
    tempMultipliers.TryGetValue(upgradeType, out var m) ? m : 1f;

    private void ResetTempUpgrades() => tempMultipliers.Clear();

    #endregion

    private void RefreshActiveCar()
    {
        if (carPrefabs == null || carPrefabs.Length == 0) return;

        int index = Mathf.Clamp(CurrentCarLevel - 1, 0 , carPrefabs.Length -1);

        for (int i = 0; i < carPrefabs.Length; i++) 
        {
            if(carPrefabs[i] != null && carPrefabs[i].activeSelf)
                carPrefabs[i].SetActive(false);
        }

        var selected = carPrefabs[index];
        if(selected != null) selected.SetActive(true);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : ManagerBase
{
    public static UpgradeManager Instance => MonoInstance.Get<UpgradeManager>();

    [Header("Upgrade Definitions (Optional. If you leave it empty, It'll get from Rescourses.)")]
    [SerializeField] private UpgradeDefinition[] definitions;

    private readonly Dictionary<UpgradeType, UpgradeDefinition> defs = new();
    private bool initialized;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public override void Init()
    {
        if (initialized) return;
        initialized = true;

        defs.Clear();

        if (definitions != null)
        {
            foreach (var def in definitions)
                if (def != null) defs[def.upgradeType] = def;
        }

        if (defs.Count == 0)
        {
            var defsFromFolder = Resources.LoadAll<UpgradeDefinition>("Upgrades");
            foreach (var d in defsFromFolder)
                if (d != null) defs[d.upgradeType] = d;
        }
    }

    public override void DeInit()
    {

    }

    //API
    public float GetUpgradeValue(UpgradeType upgradeType, float fallback = 1f)
    {
        Init();
        if (!defs.TryGetValue(upgradeType, out var def)) return fallback;

        int level = GetLevel(upgradeType);
        float inc = def.upgradeBaseIncrementalValue * (level - 1);
        return def.upgradeBaseValue + inc;
    }

    public int GetUpgradeLevel(UpgradeType upgradeType, int fallback = 1)
    {
        Init();
        return PlayerPrefs.GetInt(Key(upgradeType, "lvl"), fallback);
    }

    public float GetUpgradePrice(UpgradeType upgradeType, float fallback = 0)
    {
        Init();
        if (!defs.TryGetValue(upgradeType, out var def)) return fallback;

        float incPrice = GetIncrementalPrice(upgradeType, def.incrementalBasePrice);
        return def.startPrice + incPrice;
    }

    public bool TryPurchase(UpgradeType type)
    {
        Init();
        if (!defs.TryGetValue(type, out var def)) return false;

        float price = GetUpgradePrice(type);
        if (!ActionManager.CheckMoneyAmount(price)) return false;

        ActionManager.UpdateMoney?.Invoke(-price);
        ActionManager.OnUpgradePurchased?.Invoke();


        int level = GetLevel(type);
        float incPrice = GetIncrementalPrice(type, def.incrementalBasePrice);

        level += 1;
        incPrice += def.incrementalBasePrice;

        PlayerPrefs.SetInt(Key(type, "lvl"), level);
        PlayerPrefs.SetFloat(Key(type, "incp"), incPrice);
        PlayerPrefs.Save();

        ActionManager.GamePlayUpgrade?.Invoke(type, GetUpgradeValue(type));
        return true;
    }

    public void PublishAll()
    {
        Init();

        foreach(var def in defs)
            ActionManager.GamePlayUpgrade?.Invoke(def.Key, GetUpgradeValue(def.Key));       
    }

    public void ResetProgress()
    {
        Init();

        foreach(var def in defs)
        {
            PlayerPrefs.DeleteKey(Key(def.Key, "lvl"));
            PlayerPrefs.DeleteKey(Key(def.Key, "incp"));
        }
    }

    //Helper Methods
    private int GetLevel(UpgradeType t) => PlayerPrefs.GetInt(Key(t, "lvl"), 1);

    private float GetIncrementalPrice(UpgradeType t, float fallback) => PlayerPrefs.GetFloat(Key(t, "incp"), fallback);

    private static string Key(UpgradeType t, string suffix) => $"{t}_{suffix}";
}
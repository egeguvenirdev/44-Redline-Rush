using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Definition", fileName = "NewUpgradeDefinition")]
public class UpgradeDefinition : ScriptableObject
{
    public UpgradeType upgradeType;

    [Header("Name")]
    public string cardName = "Upgrade";

    [Header("Prices")]
    public int startPrice = 100;
    public int incrementalBasePrice = 10;

    [Header("UpgradeValues")]
    public float upgradeBaseValue = 1f;
    public float upgradeBaseIncrementalValue = 1f;
}

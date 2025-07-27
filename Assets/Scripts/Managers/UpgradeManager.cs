using UnityEngine;

public class UpgradeManager : MonoBehaviour, ManagerBase
{
    public virtual void Init()
    {
    }

    public virtual void DeInit()
    {
    }
}

public enum UpgradeType
{
    MoneyMultiplier,
    HorsePower
}
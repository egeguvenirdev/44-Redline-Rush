using UnityEngine;

public class PlayerManager : MonoBehaviour, ManagerBase
{
    [Header("Components")]
    [SerializeField] private RunnerScript runnerScript;
    [SerializeField] private Transform characterTransform;
    private UpgradeManager upgradeManager;


    public Transform GetCharacterTransform
    {
        get => characterTransform;
    }

    public virtual void Init()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        runnerScript.Init();
        upgradeManager.Init();
    }

    public virtual void DeInit()
    {
        runnerScript.DeInit();
        upgradeManager.DeInit();
    }
}

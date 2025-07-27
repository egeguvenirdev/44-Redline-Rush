using UnityEngine;

public class PlayerManager : ManagerBase
{
    [Header("Components")]
    [SerializeField] private RunnerScript runnerScript;
    [SerializeField] private Transform characterTransform;
    private UpgradeManager upgradeManager;


    public Transform GetCharacterTransform
    {
        get => characterTransform;
    }

    public override void Init()
    {
        upgradeManager = FindFirstObjectByType<UpgradeManager>();
        runnerScript.Init();
        upgradeManager.Init();
    }

    public override void DeInit()
    {
        runnerScript.DeInit();
        upgradeManager.DeInit();
    }
}

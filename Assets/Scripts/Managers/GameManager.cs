using UnityEngine;

public class GameManager : MonoSingelton<GameManager>
{
    [Header("Player Pref Settings")]
    [SerializeField] private bool resetPlayerPrefs;

    [Header("Managers")]
    [SerializeField] private ManagerBase[] managerBase;
    [SerializeField] private ManagerBase camManager;
    private PlayerManager playerManager;
    //manager

    private void Start()
    {
        if (resetPlayerPrefs) PlayerPrefs.DeleteAll();

        InitManagers(true);
    }

    private void InitManagers(bool init)
    {
        foreach (var manager in managerBase)
            if (init)
            {
                manager?.Init();
            }
            else
            {
                manager?.DeInit();
                camManager.Init();
            }
    }

    public void StartTheLevel()
    {
        ActionManager.GameStart?.Invoke();

        playerManager = FindFirstObjectByType<PlayerManager>();
        playerManager.Init();

        camManager.Init();

        Debug.Log("Game is up");
    }

    public void OnLevelSuccess()
    {
        //levelmanager.levelup
        InitManagers(false);
    }

    public void OnLevelFailed()
    {
        InitManagers(false);
    }


    public void FinishLevel()
    {
        //playeri durdur
        //player up[gradelerini ac
        //action maanagera lkevel bittigini haber ver
    }
}

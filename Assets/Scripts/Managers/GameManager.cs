using UnityEngine;

public class GameManager : MonoSingelton<GameManager>
{
    [Header("Player Pref Settings")]
    [SerializeField] private bool resetPlayerPrefs;

    [Header("Managers")]
    [SerializeField] private ManagerBase[] managerBase;
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
                manager?.Init();
            else 
                manager?.DeInit();
    }

    public void StartTheLevel()
    {

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

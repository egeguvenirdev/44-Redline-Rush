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
        ActionManager.GameStart?.Invoke();

        //playerin hareketi baslayacak level instantiate edildinten sonra calisacagi icin playeri bul 
        //playeri init et

        //camManager i init et

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

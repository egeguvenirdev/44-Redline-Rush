using UnityEngine;

public class UpdateManager : MonoBehaviour, ManagerBase
{
    private bool canUpdateGame;

    public virtual void Init()
    {
        canUpdateGame = true;
    }

    public virtual void DeInit()
    {
        canUpdateGame = false;
    }

    private void Update()
    {
        if (!canUpdateGame) return;

        ActionManager.Updater?.Invoke(Time.deltaTime);

        //coroutine manager ticxk metodunu cagir
    }
}

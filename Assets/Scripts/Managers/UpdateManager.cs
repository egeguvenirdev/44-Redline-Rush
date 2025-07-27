using UnityEngine;

public class UpdateManager : ManagerBase
{
    private bool canUpdateGame;

    public override void Init()
    {
        canUpdateGame = true;
    }

    public override void DeInit()
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

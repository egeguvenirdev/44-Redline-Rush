using UnityEngine;

public class UpdateManager : ManagerBase
{
    private bool canUpdateGame;
    //playermanager player

    public override void Init()
    {
        canUpdateGame = true;
        //playeri find et
    }

    public override void DeInit()
    {
        canUpdateGame = false;
    }

    private void Update()
    {
        if (!canUpdateGame) return;

        ActionManager.Updater?.Invoke(Time.deltaTime);
        //ActionManager.AiUpdater?.Invoke(player.getchartransform.pos);
        //coroutine manager ticxk metodunu cagir
    }
}

using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeButton : UIPanelBase
{
    [SerializeField] private UpgradeCard[] upgradeCards;
    [SerializeField] private GameObject panelElements;
    private GameManager gameManager;

    public override void Init()
    {
        gameManager = GameManager.Instance;

        foreach (var u in upgradeCards)
            u.Init();
    }

    public override void DeInit()
    {
        foreach (var u in upgradeCards)
            u.DeInit();
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();

        if (panelElements != null)
        {
            panelElements.SetActive(false);
            gameManager.StartTheLevel();
            return;
        }
    }
}

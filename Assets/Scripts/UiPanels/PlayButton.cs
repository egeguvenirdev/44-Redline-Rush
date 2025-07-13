using UnityEngine;

public class PlayButton : UIPanelBase
{
    [SerializeField] private GameObject panelElements;
    [SerializeField] private GameObject upgradePanel;
    private GameManager gameManager;
    private bool gameStarted;

    public override void Init()
    {
        gameManager = GameManager.Instance;
        gameStarted = false;

        if (upgradePanel != null) panelElements.SetActive(true);
    }

    public override void DeInit()
    {
        panelElements.SetActive(false); 
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();

        if(upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            panelElements.SetActive(false);
        }

        if (!gameStarted)
        {
            panelElements.SetActive(false);
            gameManager.StartTheLevel();
            gameStarted = true;
            return;
        }
         
        panelElements.SetActive(false);
    }
}

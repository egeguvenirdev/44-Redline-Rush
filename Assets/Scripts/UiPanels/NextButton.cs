using UnityEngine;

public class NextButton : UIPanelBase
{
    [SerializeField] private GameObject panelElements;
    private GameManager gameManager;

    public override void Init()
    {
        gameManager = GameManager.Instance;
        ActionManager.GameEnd += OnGameEnd;
    }

    public override void DeInit()
    {
        ActionManager.GameEnd -= OnGameEnd;
    }

    private void OnGameEnd(bool check)
    {
        if (check) panelElements.SetActive(true);
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();

        gameManager.OnLevelSuccess();
        panelElements.SetActive(false);
    }
}

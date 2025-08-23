using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : ManagerBase
{
    public static UIManager Instance => MonoInstance.Get<UIManager>();

    [Header("Panels")]
    [SerializeField] private UIPanelBase[] panels;
    [SerializeField] private UIPanelBase upgradePanel;
    [SerializeField] private GameObject inGameUI;

    [Header("Level & Money Texts")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private Image moneyImage;

    private Tween smoothMoneyTween;
    private float smoothMoneyNumber;

    public Transform GetMoneyImageTrasnsform => moneyImage.transform;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public override void Init()
    {
        ActionManager.GameEnd += CloseInGameUIs;

        int level = ActionManager.GameLevel.Invoke();
        levelLabel.text = "Level " + (level + 1);

        foreach (var p in panels)
            p.Init();

        upgradePanel.Init();
    }

    public override void DeInit()
    {
        ActionManager.GameEnd -= CloseInGameUIs;

        foreach (var p in panels)
            p.DeInit();

        upgradePanel.DeInit();
    }

    private void CloseInGameUIs(bool isPassed)
    {
        inGameUI.SetActive(false);
        upgradePanel.DeInit();
    }

    #region Money

    public void SetMoneyLabel(float currentMoney, float targetMoney, bool isAnimated)
    {
        smoothMoneyNumber = currentMoney;
        if (isAnimated)
        {
            smoothMoneyTween.Kill();
            smoothMoneyTween = DOTween.To(() => smoothMoneyNumber, x => smoothMoneyNumber = x, targetMoney, 0.5f).SetSpeedBased(false).OnUpdate(() => UpdateMoneyLabel());
        }
        else
        {
            smoothMoneyTween.Kill();
            smoothMoneyNumber = targetMoney;
            UpdateMoneyLabel();
        }
    }

    private void UpdateMoneyLabel()
    {
        moneyLabel.text = Formatter.FormatFloatToReadableString(smoothMoneyNumber);
    }
    #endregion
}

using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : ManagerBase
{
    public static UIManager Instance => MonoInstance.Get<UIManager>();

    [Header("Panels")]
    [SerializeField] private UIPanelBase[] panels;
    //upgradebuttonlarini degisken oalrak ekle
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private UIPanelBase upgradePanel;

    [Header("Level & Money Texts")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI moneyLabel;
    [SerializeField] private Image moneyImage;

    private Tween smoothMoneyTween;
    private float smoothMoneyNumber;
    //levelmanager

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
        //levelmanageri ata

        ActionManager.GameStart += OpenInGameUIs;
        ActionManager.GameEnd += CloseInGameUIs;

        //level texti guncelle

        foreach (var p in panels)
            p.Init();

        //upgradecard sinifi yazilinca burada init edilmeli
    }

    public override void DeInit()
    {
        ActionManager.GameStart -= OpenInGameUIs;
        ActionManager.GameEnd -= CloseInGameUIs;

        foreach (var p in panels)
            p.DeInit();

        //upgradecard sinifi yazilinca burada deinit edilmeli
    }

    private void OpenInGameUIs()
    {
        inGameUI.SetActive(true);
    }

    private void CloseInGameUIs(bool isPassed)
    {
        inGameUI.SetActive(false);
        upgradePanel.DeInit();
    }

    #region Money

    public void SetMoneyLabel(float totalMoney, bool isAnimated)
    {
        if (isAnimated)
        {
            smoothMoneyTween.Kill();
            smoothMoneyTween = DOTween.To(() => smoothMoneyNumber, x => smoothMoneyNumber = x, totalMoney, 0.5f).SetSpeedBased(false).OnUpdate(() => UpdateMoneyLabel());
        }
        else
        {
            smoothMoneyTween.Kill();
            smoothMoneyNumber = totalMoney;
            UpdateMoneyLabel();
        }

        //upgradebuttonlarini burade bir update et param yetiyor mu????
    }

    private void UpdateMoneyLabel()
    {
        moneyLabel.text = Formatter.FormatFloatToReadableString(smoothMoneyNumber);
    }
    #endregion
}

using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIManager : MonoSingelton<UIManager>, ManagerBase
{
    [Header("Panels")]
    [SerializeField] private UIPanelBase[] panels;
    //upgradebuttonlarini degisken oalrak ekle
    [SerializeField] private GameObject[] inGameUIs;
    [SerializeField] private UIPanelBase upgradePanel;

    [Header("Level & Money Texts")]
    [SerializeField] private TextMeshProUGUI levelLabel;
    [SerializeField] private TextMeshProUGUI moneyLabel;

    private Tween smoothMoneyTween;
    private float smoothMoneyNumber;
    //levelmanager

    public virtual void Init()
    {
        //levelmanageri ata

        ActionManager.GameStart += OpenInGameUIs;
        ActionManager.GameEnd += CloseInGameUIs;

        //level texti guncelle

        foreach (var p in panels)
            p.Init();

        //upgradecard sinifi yazilinca burada init edilmeli
    }

    public virtual void DeInit()
    {
        ActionManager.GameStart -= OpenInGameUIs;
        ActionManager.GameEnd -= CloseInGameUIs;

        foreach (var p in panels)
            p.DeInit();

        //upgradecard sinifi yazilinca burada deinit edilmeli
    }

    private void OpenInGameUIs()
    {
        foreach (var inGameUi in inGameUIs)
            inGameUi.SetActive(true);
    }

    private void CloseInGameUIs(bool isPassed)
    {
        foreach (var inGameUi in inGameUIs)
            inGameUi.SetActive(false);

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

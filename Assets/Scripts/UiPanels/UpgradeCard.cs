using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCard : ButtonBase
{
    [Header("Upgrade Config")]
    [SerializeField] private UpgradeDefinition upgradeDefinition;

    [Header("Button Settings")]
    [SerializeField] private Button button;
    [SerializeField] private Color32 whiteColor;
    [SerializeField] private Color32 redColor;

    [Header("Panel Settings")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI priceText;

    private UpgradeManager upgradeManager;
    private Vector3 startScale = Vector3.one * 2.4f;

    public override void Init()
    {
        base.Init();

        upgradeManager = UpgradeManager.Instance;
        ActionManager.GameStart += OnGameStart;
        ActionManager.OnUpgradePurchased += RefreshAll;
        startScale = transform.localScale;

        RefreshAll();
    }

    public override void DeInit()
    {
        base.DeInit();
        ActionManager.GameStart -= OnGameStart;
        ActionManager.OnUpgradePurchased -= RefreshAll;
    }

    private void OnGameStart()
    {
        RefreshAll();
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();

        if (upgradeManager.TryPurchase(upgradeDefinition.upgradeType))
        {

            transform.DOKill();
            transform.DOScale(startScale, 0);
            transform.DOPunchScale(startScale * 0.3f, .5f, 6).SetUpdate(true);
            return;
        }

        transform.DOKill(true);
        transform.DOShakePosition(.25f, 8f, 20).SetUpdate(true);
    }

    private void RefreshAll()
    {
        if(upgradeDefinition.hasMaxLimit)
        {
            int level = upgradeManager.GetUpgradeLevel(upgradeDefinition.upgradeType);
            if(level >= upgradeDefinition.maxLevel)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        SetTexts();
        SetInterractablesAndColors();
    }

    private void SetTexts()
    {
        int lvl = upgradeManager.GetUpgradeLevel(upgradeDefinition.upgradeType);
        float price = upgradeManager.GetUpgradePrice(upgradeDefinition.upgradeType);

        if (levelText) levelText.text = ConstantVariables.UpgradeTexts.Lv + " " + lvl;
        if (priceText) priceText.text = Formatter.FormatFloatToReadableString(price);
    }

    private void SetInterractablesAndColors()
    {
        bool canBuy = ActionManager.CheckMoneyAmount.Invoke(upgradeManager.GetUpgradePrice(upgradeDefinition.upgradeType)); //upgradeManager.TryPurchase(upgradeDefinition.upgradeType);
        if (button) button.interactable = canBuy;
        if (priceText) priceText.color = canBuy ? whiteColor : redColor;
    }
}

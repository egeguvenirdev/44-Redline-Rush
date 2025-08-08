using UnityEngine;

public class MoneyManager : ManagerBase
{
    [Header("Money Settings")]
    [SerializeField] private int addMoney = 0;
    private float moneyMultiplier = 1;
    private UIManager uiManager;

    public float MoneyMultiplier
    {
        get => moneyMultiplier;
        private set => moneyMultiplier = value;
    }

    public float Money
    {
        get => PlayerPrefs.GetFloat(ConstantVariables.TotalMoneyValue.TotalMoney, 0);

        private set
        {
            float calculatedMoney = value;

            if (value > 0)
                calculatedMoney = value * moneyMultiplier;

            PlayerPrefs.SetFloat(ConstantVariables.TotalMoneyValue.TotalMoney, PlayerPrefs.GetFloat(ConstantVariables.TotalMoneyValue.TotalMoney, 0) + calculatedMoney);
            uiManager = UIManager.Instance;
            uiManager.SetMoneyLabel(calculatedMoney, true);
            //MonoInstance.Get<UIManager>().SetMoneyLabel(calculatedMoney, true);
        }
    }


    public override void Init()
    {
        ActionManager.UpdateMoney += OnUpdateMoney;
        ActionManager.UpdateMoneyMultiplier += OnUpdateMoneyMultiplier;
        ActionManager.CheckMoneyAmount += OnCheckMoneyAmount;

        if (Money <= 0) Money = addMoney;
    }

    public override void DeInit()
    {
        ActionManager.UpdateMoney -= OnUpdateMoney;
        ActionManager.UpdateMoneyMultiplier -= OnUpdateMoneyMultiplier;
        ActionManager.CheckMoneyAmount -= OnCheckMoneyAmount;
    }

    private void OnUpdateMoney(float value) => Money = value;

    private void OnUpdateMoneyMultiplier(float value) => MoneyMultiplier = value;

    private bool OnCheckMoneyAmount(float value)
    {
        if (value <= Money) return true;
        return false;
    }
}

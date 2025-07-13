using UnityEngine;

public class LevelManager : ManagerBase
{
    [Header("Level Props")]
    [SerializeField] private GameObject[] levels;
    [SerializeField] private bool forceLevel;
    [SerializeField] private int forceLevelIndex;

    private GameObject currentLevel;

    public int LevelIndex
    {
        get => PlayerPrefs.GetInt(ConstantVariables.LevelValues.Level, 0);
        set => PlayerPrefs.SetInt(ConstantVariables.LevelValues.Level, PlayerPrefs.GetInt(ConstantVariables.LevelValues.Level) + value);
    }

    public override void Init()
    {
        GenerateCurrentLevel();
    }

    public override void DeInit()
    {
        if(currentLevel != null) 
            Destroy(currentLevel);
    }

    private void GenerateCurrentLevel()
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        if(forceLevel)
        {
            if(forceLevelIndex >= levels.Length) forceLevelIndex = levels.Length - 1;
            currentLevel = Instantiate(levels[forceLevelIndex]);
            return;
        }

        if(LevelIndex >= levels.Length)
        {
            currentLevel = Instantiate(levels[Random.Range(0, levels.Length)]);
            return;
        }

        currentLevel = Instantiate(levels[LevelIndex]);
    }

    public void LevelUp(int levelUpValue = 1)
    {
        LevelIndex = levelUpValue;

        Init();
        //generatecurrentlevel
    }
}

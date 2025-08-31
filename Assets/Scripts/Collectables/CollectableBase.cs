using DG.Tweening;
using UnityEngine;

public class CollectableBase : MonoBehaviour, IPoolable, ICollectable
{
    [Header("Collect Settings")]
    [SerializeField] protected AudioType audioType;
    [SerializeField] protected UpgradeType upgradeType;
    [SerializeField] protected PoolType poolType;
    [SerializeField] protected Collider col;

    [Header("Upgrade Settings")]
    [SerializeField] protected float upgradeValue;

    [Header("Animation Settings")]
    [SerializeField] protected bool canRotate = true;
    [SerializeField] protected bool canElevate = true;
    [SerializeField] protected Vector3 rotateVelocity;
    [SerializeField] protected Vector3 maxHeight = new Vector3(0, 0.5f, 0);
    [SerializeField] protected Space rotateSpace;
    [SerializeField] protected float speed = 4f;

    protected float elapsedTme;
    protected bool isAnimating;
    protected bool destroyed;
    private Vector3 minHeight;
    private Transform imageTransform;

    private UIManager uiManager;
    private Camera cam;

    public PoolType PoolType => throw new System.NotImplementedException();

    private void OnEnable()
    {
        minHeight = transform.position;
        gameObject.SetLayerRecursively("Collectable");

        if (!uiManager) uiManager = UIManager.Instance;
        if (!imageTransform) imageTransform = uiManager.GetMoneyImageTrasnsform;

        isAnimating = true;
        destroyed = false;

        ActionManager.Updater += OnUpdate;
    }

    public void OnTakenFromPool()
    {
    }

    private void OnUpdate(float deltaTime)
    {
        if (!isAnimating) return;

        if (canRotate)
            transform.Rotate(rotateVelocity * deltaTime, rotateSpace);


        if (canElevate)
        {
            elapsedTme += deltaTime;
            float lerpValue = (Mathf.Sin(speed * elapsedTme) + 1f) / 2f;
            transform.position = Vector3.Lerp(minHeight, minHeight + maxHeight, lerpValue);
        }
    }

    public void Collect(Transform target = null)
    {
        isAnimating = false;
        if (col) col.enabled = false;

        if (upgradeType == UpgradeType.Money)
        {
            MoveToMoneyArea();
        }
        else
        {
            if (target)
            {
                Vector3 targetPos = target.position;
                transform.DOJump(targetPos, 1.5f, 1, .4f).OnUpdate(() =>
                {
                    targetPos = target.position;
                }).OnComplete(() =>
                {
                    ActionManager.AddTempUpgrade?.Invoke(upgradeType, upgradeValue);
                    Destroy(gameObject);

                });
            }
            else
            {
                ActionManager.AddTempUpgrade?.Invoke(upgradeType, upgradeValue);
                Destroy(gameObject);
            }
        }
    }

    private void MoveToMoneyArea()
    {
        Camera cam = ActionManager.GetOrtoCam?.Invoke();
        Vector3 firstPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 secondPos = cam.ScreenToWorldPoint(firstPos);
        secondPos.z = imageTransform.position.z;
        transform.position = secondPos;

        gameObject.SetLayerRecursively("UI");

        Vector3[] path = new Vector3[3];
        path[0] = secondPos;
        path[1] = imageTransform.position + Vector3.down * 4 + Vector3.left * 1.5f;
        path[2] = imageTransform.position;

        transform.DOPath(path, 0.6f, PathType.CatmullRom)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                ActionManager.UpdateMoney?.Invoke(upgradeValue);
                imageTransform.DOKill();
                imageTransform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 3);
                Destroy(gameObject);
            });

        transform.DOScale(Vector3.one / 1.3f, .3f)
            .SetDelay(0.3f)
            .SetEase(Ease.Linear);
    }

    public void OnReturnedToPool()
    {
        if (!destroyed) ActionManager.Updater -= OnUpdate;

        destroyed = true;
    }

    private void OnDisable()
    {
        if (!destroyed) ActionManager.Updater -= OnUpdate;

        destroyed = true;
    }

    private void OnDestroy()
    {
        if (!destroyed) ActionManager.Updater -= OnUpdate;

        destroyed = true;
    }
}

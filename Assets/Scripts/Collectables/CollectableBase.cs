using DG.Tweening;
using UnityEngine;

public class CollectableBase : MonoBehaviour, IPoolable, ICollectable
{
    [Header("Collect Settings")]
    [SerializeField] protected AudioType audioType;
    [SerializeField] protected UpgradeType upgradeType;
    [SerializeField] protected PoolType poolType;

    [Header("Upgrade Settings")]
    [SerializeField] protected float upgradeValue;
    //[SerializeField] protected float 


    [Header("Animation Settings")]
    [SerializeField] protected bool canRotate = true;
    [SerializeField] protected bool canElevate = true;
    [SerializeField] protected Vector3 rotateVelocity;
    [SerializeField] protected Vector3 maxHeight = new Vector3(0, 0.5f, 0);
    [SerializeField] protected Space rotateSpace;
    [SerializeField] protected float speed = 4f;

    protected bool isAnimating;
    protected bool destroyed;
    private Vector3 minHeight;
    private Transform imageTransform;

    private UIManager uiManager;
    private Camera cam;

    public PoolType PoolType => throw new System.NotImplementedException();

    public void OnTakenFromPool()
    {
        minHeight = transform.position;
        gameObject.SetLayerRecursively("Collectable");

        if (uiManager) uiManager = UIManager.Instance;
        if (cam) cam = Camera.main;
        if (imageTransform) imageTransform = uiManager.GetMoneyImageTrasnsform;

        isAnimating = true;
        destroyed = false;

        ActionManager.Updater += OnUpdate;
    }

    private void OnUpdate(float deltaTime)
    {
        if (!isAnimating) return;

        if (canRotate)
            transform.Rotate(rotateVelocity * deltaTime, rotateSpace);


        if (canElevate)
        {
            float lerpValue = Mathf.Sin(speed * deltaTime + 1f) / 2f;
            transform.position = Vector3.Lerp(minHeight, minHeight + maxHeight, lerpValue);
        }
    }


    public void Collect(Transform target = null, bool uiAnimated = false)
    {
        isAnimating = false;

        if (target)
        {
            Vector3 targetPos = target.position;
            transform.DOJump(targetPos, 1f, 1, .5f).OnUpdate(() =>
            {
                targetPos = target.position;
            }).OnComplete(() =>
            {
                if (uiAnimated)
                {
                    MoveToMoneyArea();
                    return;
                }
                ActionManager.GamePlayUpgrade?.Invoke(upgradeType, upgradeValue);
                Destroy(gameObject);

            });
        }

        ActionManager.GamePlayUpgrade?.Invoke(upgradeType, upgradeValue);
        Destroy(gameObject);
    }

    private void MoveToMoneyArea()
    {
        Vector3 firstPos = cam.WorldToScreenPoint(transform.position);
        Vector3 secondPos = ActionManager.GetOrtographicScreenToWorldPoint(firstPos);
        secondPos.z = imageTransform.position.z;
        transform.position = secondPos;

        gameObject.SetLayerRecursively("UI");

        Vector3[] path = new Vector3[3];
        path[0] = secondPos;
        path[1] = imageTransform.position + Vector3.down * 4 + Vector3.left * 1.5f;
        path[2] = imageTransform.position;

        transform.DOPath(path, 1f, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                ActionManager.GamePlayUpgrade?.Invoke(upgradeType, upgradeValue);
                //image a dopunch ekle
                Destroy(gameObject);
            });

        transform.DOScale(Vector3.one / 3, .3f)
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

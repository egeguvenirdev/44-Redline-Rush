using UnityEngine;

public class CamManager : ManagerBase
{
    [Header("Components")]
    [SerializeField] private Camera cam;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 followOffSet = Vector3.zero;
    [SerializeField] private float followSpeed = .2f;
    [SerializeField] private float clampLocalX = 1.5f;

    private Transform player;

    public override void Init()
    {
        ActionManager.Updater += OnUpdate;
        player = FindFirstObjectByType<PlayerManager>().GetCharacterTransform;
    }

    public override void DeInit()
    {
        ActionManager.Updater -= OnUpdate;
    }

    private void OnUpdate(float deltaTime)
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + followOffSet;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -clampLocalX, clampLocalX);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * deltaTime);
        }
    }

    //getcam pos for ui animation
}

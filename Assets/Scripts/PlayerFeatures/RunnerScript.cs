using UnityEngine;

public class RunnerScript : MonoBehaviour
{
    [Header("Scripts and Transforms")]
    [SerializeField] private Transform model;
    [SerializeField] private Transform localMover;
    [SerializeField] private PlayerSwerve playerSwerve;

    [Header("Run Settings")]
    [SerializeField] private float followOffset = 1f;
    [SerializeField] private float clampLocalX = 2f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float localTargetSwipeSpeed = 2f;
    [SerializeField] private float characterSwipeLerpSpeed = 2f;
    [SerializeField] private float characterRotateLerpSpeed = 2f;
    [SerializeField] private float characterRotateMaxAngle = 15f;
    [SerializeField] private float characterRoateStartMomentum = 2;
    [SerializeField] private float characterRoateEndMomentum = 0.1f;
    [SerializeField] private bool canFollow;

    private Vector3 oldPosition;
    private bool canRun;
    private bool canSwerve;
    private bool canLookAt;

    public void Init()
    {
        playerSwerve.Init();

        ActionManager.SwerveValue += OnPlayerSwerve;
        ActionManager.Updater += OnUpdate;
        StartToRun(true);
    }

    public void DeInit()
    {
        playerSwerve.DeInit();

        ActionManager.SwerveValue -= OnPlayerSwerve;
        ActionManager.Updater -= OnUpdate;
        StartToRun(false);
    }

    private void OnUpdate(float deltaTime)
    {
        FollowLocalMover(deltaTime);
        oldPosition = model.localPosition;
    }

    private void StartToRun(bool check)
    {
        if (check)
        {
            //playerswerve init
            canRun = true;
            canSwerve = true;
            canLookAt = true;
        }
        else
        {
            //swerve deinit
            canRun = false;
            canSwerve = false;
            canLookAt = false;
        }
    }

    private void OnPlayerSwerve(float direction)
    {
        if (!canSwerve) return;
        localMover.localPosition += Vector3.right * direction * localTargetSwipeSpeed * Time.deltaTime;
        ClampLocalPosition();

    }

    private void ClampLocalPosition()
    {
        Vector3 pos = localMover.localPosition;
        pos.x = Mathf.Clamp(pos.x, -clampLocalX, clampLocalX);
        localMover.localPosition = pos;
    }

    private void FollowLocalMover(float deltaTime)
    {
        if (canRun) localMover.Translate(localMover.forward * deltaTime * runSpeed);

        if(canLookAt)
        {
            Vector3 direction = localMover.localPosition - oldPosition;

            float angle = Vector3.Angle(model.forward, direction);
            angle = Mathf.Min(angle, characterRotateMaxAngle);

            float dynamicLerpSpeed = characterRotateLerpSpeed * Mathf.Lerp(characterRoateStartMomentum, characterRoateEndMomentum, angle / characterRotateMaxAngle);
            model.forward = Vector3.Lerp(model.forward, direction.normalized, dynamicLerpSpeed * deltaTime);
        }

        if (canSwerve)
        {
            Vector3 nextPos = new Vector3(localMover.position.x, model.position.y, localMover.position.z);
            model.localPosition = Vector3.Lerp(model.localPosition, nextPos + Vector3.forward * followOffset, characterSwipeLerpSpeed * deltaTime);
        }
    }
}

using UnityEngine;

public class RunnerScript : MonoBehaviour
{
    [Header("Scripts and Transforms")]
    [SerializeField] private Transform model;
    [SerializeField] private Transform localMover;
    //[SerializeField] private PlayerSwerve playerSwerve;

    [Header("Run Settings")]
    [SerializeField] private float clampLocalX = 2f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float localTargetSwipeSpeed = 2f;
    [SerializeField] private float characterSwipeLerpSpeed = 2f;
    [SerializeField] private float characterRotateLerpSpeed = 2f;
    [SerializeField] private bool canFollow;

    private Vector3 oldPosition;
    private bool canRun;
    private bool canSwerve;
    private bool canLookAt;

    public void Init()
    {
        //swerve value icin swerve e sub ol
        ActionManager.Updater += OnUpdate;
        StartToRun(true);
    }

    public void DeInit()
    {
        ActionManager.Updater -= OnUpdate;
        StartToRun(false);
    }

    private void OnUpdate(float deltaTime)
    {
        FollowLocalMover(deltaTime);
    }

    private void StartToRun(bool check)
    {
        if (check)
        {
            //playerswerve init
            canRun = true;
            canSwerve = true;
        }
        else
        {
            //swerve deinit
            canRun = false;
            canSwerve = false;
        }
    }

    private void OnPlayerSwerve(float direction)
    {
        if (!canSwerve) return;
        localMover.localPosition = Vector3.right * direction * localTargetSwipeSpeed * Time.deltaTime;

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
            model.forward = Vector3.Lerp(model.forward, direction, characterRotateLerpSpeed);
        }

        if (canSwerve)
        {
            Vector3 nextPos = new Vector3(localMover.position.x, localMover.position.y, localMover.position.z);
            model.localPosition = Vector3.Lerp(model.localPosition, nextPos, runSpeed * deltaTime);
        }
    }
}

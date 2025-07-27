using UnityEngine;

public class PlayerSwerve : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed = 1000;
    [SerializeField] private float directionMultiplier = 100;

    private float multiplier = 1f;
    private float dirMaxMagnitude = float.PositiveInfinity;
    private Vector2 deltaDir;
    private Vector2 joystickCenterPos;
    private Vector2 dir;
    private Vector2 dirOld;
    private bool isControl = false;

    public void Init()
    {
        //swerve value icin swerve e sub ol
        ActionManager.Updater += OnUpdate;
    }

    public void DeInit()
    {
        ActionManager.Updater -= OnUpdate;
    }

    private void OnUpdate(float deltaTime)
    {

        if (Input.GetMouseButtonDown(0))
        {
            joystickCenterPos = (Vector2)Input.mousePosition;
            deltaDir = Vector2.zero;
            dirOld = Vector2.zero;
            isControl = true;
        }

        if (Input.GetMouseButtonUp(0))
            isControl = false;


        if (isControl)
        {
            multiplier = directionMultiplier / Screen.width;
            dir = ((Vector2)Input.mousePosition - joystickCenterPos) * multiplier;
            float m = dir.magnitude;

            if (m > dirMaxMagnitude) dir *= dirMaxMagnitude / m;

            deltaDir = dir - dirOld;
            dirOld = dir;

            ActionManager.SwerveValue?.Invoke(deltaDir.x * moveSpeed * deltaTime);
        }
    }
}

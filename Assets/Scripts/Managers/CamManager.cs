using UnityEngine;

public class CamManager : ManagerBase
{
    [Header("Components")]
    [SerializeField] private Camera cam;

    [Header("Follow Settings")]
    [SerializeField] private Vector3 followOffSet;
    [SerializeField] private float followSpeed = 0.2f;
    [SerializeField] private float clampLocalX = 1.5f;

    public override void Init()
    {

    }

    public override void DeInit()
    {
    }
}

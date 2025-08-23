using UnityEngine;

public class ObjectMagnet : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offSet;

    private void Update()
    {
        if(target != null) transform.position = target.position + offSet;
    }
}

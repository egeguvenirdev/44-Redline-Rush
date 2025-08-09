using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == transform) return;

        if (other.transform.parent.TryGetComponent<ICollectable>(out ICollectable hitObject))
            hitObject.Collect(transform);
    }
}

using UnityEngine;

public class MiniGameDedector : MonoBehaviour
{
    [SerializeField] private Collider col;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = false;
            ActionManager.MiniGame?.Invoke();
        }
    }
}

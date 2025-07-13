using UnityEngine;

public class ImageScaler : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 5f;

    private RectTransform rectTransform;
    private Vector3 minScale = new Vector3(.7f, .7f, .7f);
    private Vector3 maxScale = new Vector3(1.1f, 1.1f, 1.1f);

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        float lerpValue = (Mathf.Sin(scaleSpeed * Time.time) + 1f) / 2f;
        rectTransform.localScale = Vector3.Lerp(minScale, maxScale, lerpValue);
    }
}

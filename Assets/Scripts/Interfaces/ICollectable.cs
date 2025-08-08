using UnityEngine;

public interface ICollectable
{
    public void Collect(Transform target = null, bool uiAnimated = false);
}

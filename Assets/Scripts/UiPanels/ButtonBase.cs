using UnityEngine;

public abstract class ButtonBase : MonoBehaviour
{
    [SerializeField] private AudioType audioType;

    public virtual void Init()
    {
    }

    public virtual void DeInit()
    {
        //
    }

    public virtual void OnButtonClick()
    {
        ActionManager.PlayClip?.Invoke(audioType);
    }
}

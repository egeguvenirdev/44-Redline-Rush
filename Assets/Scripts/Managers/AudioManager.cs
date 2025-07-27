using UnityEngine;

public class AudioManager : MonoBehaviour, ManagerBase
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameAudios gameAudios;

    public virtual void Init()
    {
        ActionManager.PlayClip += OnPlayClip;
    }

    public virtual void DeInit()
    {
        ActionManager.PlayClip -= OnPlayClip;
    }

    private void OnPlayClip(AudioType audioType)
    {
        AudioClip clip = gameAudios.GetAudioClip(audioType);
        if(clip != null) audioSource.PlayOneShot(clip);
    }
}

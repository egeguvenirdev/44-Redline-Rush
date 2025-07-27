using UnityEngine;

public class AudioManager : ManagerBase
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameAudios gameAudios;

    public override void Init()
    {
        ActionManager.PlayClip += OnPlayClip;
    }

    public override void DeInit()
    {
        ActionManager.PlayClip -= OnPlayClip;
    }

    private void OnPlayClip(AudioType audioType)
    {
        AudioClip clip = gameAudios.GetAudioClip(audioType);
        if(clip != null) audioSource.PlayOneShot(clip);
    }
}

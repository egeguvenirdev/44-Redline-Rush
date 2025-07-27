using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Settings",  menuName = "Game Audios")]
public class GameAudios : ScriptableObject
{
    [SerializeField] private GameAudioClip[] audioClips;

    public AudioClip GetAudioClip (AudioType audioType)
    {
        foreach ( var item in audioClips)
        {
            if (item.audioType == audioType)
                return item.audioClip;
        }

        return null;
    }


    [Serializable]
    public struct GameAudioClip
    {
        [Header("Audio Infos")]
        public AudioClip audioClip;
        public AudioType audioType;
    }
}

public enum AudioType
{
    Collect,
    UpgradeButton
}

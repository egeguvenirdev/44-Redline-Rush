using System;
using UnityEngine;

public static class ActionManager
{
    public static Action GameStart {get; set;}
    public static Action<bool> GameEnd {get; set;}
    public static Action<float> Updater {get; set;}
    public static Action<AudioClip> PlayClip {get; set;}
}

using System;
using UnityEngine;

public static class ActionManager
{
    //Game Actions
    public static Action GameStart {get; set;}
    public static Action<bool> GameEnd {get; set;}
    public static Action<float> Updater {get; set;}
    public static Action<AudioClip> PlayClip {get; set;}

    //Player Action
    public static Action<float> SwerveValue {get; set;}
    //public static Action<UpgradeType> GamePlayUpgrade {get; set;}
    //public static Func<float> GetUpgradeValue {get; set;}

    //Cam Actions

    //Money Actions
}

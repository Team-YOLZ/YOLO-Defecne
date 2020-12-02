using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Drag,
    }
    //public enum Sound
    //{
    //    Bgm,
    //    Effect,
    //    MaxCount,
    //}
    public enum MouseEvent
    {
        Press,
        Click,
        Drag,
    }
    //public enum CameraMode
    //{
    //    QuarterView,
    //}
    public enum Scene
    {
        Unknown,
        Title,
        Main,
        Lobby,
        Game,
    }
}

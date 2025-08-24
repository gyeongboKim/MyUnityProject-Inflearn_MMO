using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    { 
        Unknown,
        Player,
        Monster,
    }


    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }
    public enum Scene
    {
        Unknown,
        LobbyScene,
        CharacterScene,
        LoadingScene,
        GameScene,

    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum UIEvent
    {
        Enter,
        Exit,
        PointerDown,
        PointerUp,
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,        //마우스 버튼을 누르고 있는 동안 지속적으로 발생 
        PointerDown, //마우스 버튼을 누르는 순간(1회)
        PointerUp,   //마우스 버튼을 떼는 순간(1회)  
        Click,        //마우스 버튼을 눌렀다가 일정 시간 내에 떼는 순간(1회, Dwon/Up 조합)  
    }
    public enum CameraMode
    { 
        QuarterView,
    }



    public enum State
    {
        Idle,
        Moving,
        Skill,      //atack, buff, 
        GetHit,
        Die,
    }
}

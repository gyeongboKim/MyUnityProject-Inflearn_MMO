using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.CharacterScene;
        Debug.Log($"SceneType 초기화 완료 : {SceneType}");

        //Managers.UI.ShowSceneUI<UI_CharacterSelect)>();

        
    }
    public override void Clear()
    {
        Debug.Log("CharacterScene Clear!");
        //TODO
    }
}

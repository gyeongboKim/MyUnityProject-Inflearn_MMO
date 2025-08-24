using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.LobbyScene;
        Debug.Log($"SceneType 초기화 완료 : {SceneType}");

        Managers.UI.ShowSceneUI<UI_Lobby>();

        //UI 버튼에 이벤트 연결
        //Managers.UI.
        
    }

    private void Update()
    {
        //씬 변환 실습
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Scene.LoadSceneImmediately(Define.Scene.GameScene);
        }
    }

    public override void Clear()
    {
        Debug.Log("LobbyScene Clear!");
        //TODO
    }
    

}

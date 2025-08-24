using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
    enum Buttons
    {
        StartNewGameButton,        //새 게임 버튼 ( 팝업
        LoadGameButton,       //불러오기 버튼 (팝업 UI 켜짐)
        MemorialButton,    //기록 버튼(지금 까지 깬 기록들)
        AchievementButton,  //업적 버튼
        OptionButton,
        ExitGameButton,
    }

    enum Texts
    {
        TitleText,
        VersionText,
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        Sprite clickEffectSprite = Managers.Resource.Load<Sprite>("Art/UI/Gui_parts/button_ready_off");
        Debug.Log($"Clicked Sprite : {clickEffectSprite.name}");
        if (clickEffectSprite == null)
        {
            Debug.LogError("Clicked Sprite is not found!");
            return;
        }

        for (int i = 0; i < (int)Buttons.ExitGameButton + 1; i++)
        {
            GetButton(i).AddUIHoverEffect();
            // 3. 불러온 'Sprite'를 넘겨준다.
            GetButton(i).AddUIClickEffect(clickEffectSprite);
        }

        GetButton((int)Buttons.StartNewGameButton).gameObject.BindEvent(OnStartButtonClicked);
        GetButton((int)Buttons.LoadGameButton).gameObject.BindEvent(OnLoadButtonCliked);
    }



    private void OnStartButtonClicked(PointerEventData data)
    {
        Managers.Scene.LoadSceneImmediately(Define.Scene.CharacterScene);
    }


    private void OnLoadButtonCliked(PointerEventData data)
    {
        Managers.Scene.LoadScene(Define.Scene.GameScene);
    }
}



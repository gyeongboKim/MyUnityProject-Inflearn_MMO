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
        StartNewGameButton,        //�� ���� ��ư ( �˾�
        LoadGameButton,       //�ҷ����� ��ư (�˾� UI ����)
        MemorialButton,    //��� ��ư(���� ���� �� ��ϵ�)
        AchievementButton,  //���� ��ư
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
            // 3. �ҷ��� 'Sprite'�� �Ѱ��ش�.
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



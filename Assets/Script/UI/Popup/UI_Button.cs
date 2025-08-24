using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    //UI 요소 이름을 정리하기 위한 열거형들.
    enum Buttons
    {
        PointButton,
    }
    enum Texts
    { 
        PointText,
        ScoreText,
    }
    enum GameObjects
    { 
        TestObject,
    }
    enum Images
    { 
        ItemIcon,
    }

    // Init()은 바인딩 및 이벤트 등록만 담당하게 함.
    public override void Init()
    {
        base.Init();

        //열거형을 기반으로 요소들을 바인딩
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //'ScoreText' 와 연결된 TextMeshProUGUI 컴포넌트를 가져와 텍스트 내용을 설정

        //Extension 메서드 활용
        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        //GameObject targetGameObject = GetImage((int)Images.ItemIcon).gameObject;
        //BindEvent(targetGameObject, ((PointerEventData data) => { targetGameObject.transform.position = data.position; }), Define.UIEvent.Drag);

    }






    int _score = 0;


    public void OnButtonClicked(PointerEventData data)
    {
        ++_score;
        GetText((int)Texts.ScoreText).text = $"Score : {_score}";
    }
}

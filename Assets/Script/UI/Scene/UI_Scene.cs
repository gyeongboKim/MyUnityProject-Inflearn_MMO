using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        //UI의 캔버스의 렌더링 모드, sortingOrder 설정
        Managers.UI.SetCanvas(gameObject, false);
    }

}

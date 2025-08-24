using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }

    // Init()은 바인딩 및 이벤트 등록만 담당하게 함.
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        //인벤토리 초기화
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        //실제 인벤토리 정보를 참고해서 넘겨주기
        for (int i = 0; i < 11; i++)
        {

            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inventory_Item");    <-ver1
            GameObject item = Managers.UI.MakeSubItem<UI_Inventory_Item>(parent: gridPanel.transform).gameObject;
            UI_Inventory_Item inventoryItem = item.GetOrAddComponent<UI_Inventory_Item>();
            inventoryItem.SetInfo($"Bow{i}");
        }
    }
}


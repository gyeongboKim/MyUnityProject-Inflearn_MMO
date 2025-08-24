using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inventory_Item : UI_Base
{
    enum GameObjects
    { 
        ItemIcon,
        ItemNameTMP,
    }

    string _itemName;

    // Init()은 바인딩 및 이벤트 등록만 담당하게 함.
    public override void Init()
    {

        Bind<GameObject>(typeof(GameObjects)); 

        // 아이템 클릭 이벤트를 전용 메서드로 바인딩
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnItemClicked, Define.UIEvent.Click);

        // 마우스 포인터가 아이템 아이콘 위에 진입했을 때 호출되는 이벤트 바인딩
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnPointerEnterItemIcon, Define.UIEvent.Enter);

        // 마우스 포인터가 아이템 아이콘에서 벗어났을 때 호출되는 이벤트 바인딩
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnPointerExitItemIcon, Define.UIEvent.Exit);

        //GetText((int)GameObjects.ItemNameTMP).text = "Bind"; //GetText로 사용 불가 : Bind를 GameObject로 했기 때문
    }

    public void SetInfo(string name)
    {
        _itemName = name;

        if (Get<GameObject>((int)GameObjects.ItemNameTMP) != null)
        {
            Get<GameObject>((int)GameObjects.ItemNameTMP).GetComponent<TextMeshProUGUI>().text = _itemName;
        }
        else
        {
            Debug.LogWarning("ItemNameTMP가 아직 바인딩되지 않음. Init()이 먼저 호출되었는지 확인.");
        }
    }

    private void OnItemClicked(PointerEventData eventData)
    {
        Debug.Log($"아이템 클릭! 이름: {_itemName}");
        // 여기에 아이템 사용, 정보 창 띄우기 등의 로직 넣기
        // 예: Managers.Inventory.UseItem(_itemName);
    }

    // 마우스 포인터가 아이템 아이콘 위에 진입했을 때 호출될 메서드
    private void OnPointerEnterItemIcon(PointerEventData eventData)
    {
        Debug.Log($"아이템 아이콘 위로 마우스 진입: {_itemName}");
        // 툴팁 활성화, 하이라이트 효과 등 넣기
        // 예: Managers.UI.ShowItemTooltip(this, _itemName); // 'this'를 넘겨 위치를 참조할 수도 있다.
    }

    // 마우스 포인터가 아이템 아이콘에서 벗어났을 때 호출될 메서드
    private void OnPointerExitItemIcon(PointerEventData eventData)
    {
        Debug.Log($"아이템 아이콘에서 마우스 이탈: {_itemName}");
        // 툴팁 비활성화, 하이라이트 효과 제거 등 넣기
        // 예: Managers.UI.HideItemTooltip();
    }
}

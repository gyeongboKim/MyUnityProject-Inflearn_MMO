using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        HPBar
    }

    Stat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {

        Transform parent = transform.parent;
        Collider parentCollider = parent.GetComponent<Collider>();


        // 콜라이더의 중앙점 (월드 좌표)
        Vector3 colliderCenter = parentCollider.bounds.center;

        // 콜라이더의 y축 절반 크기 (중앙에서 최상단까지의 거리)
        float halfHeight = parentCollider.bounds.extents.y;
        // 추후 이 값은 몬스터 종류에 따라 유동적으로 변경 가능하도록 
        float verticalOffset = 0.3f;

        //UI의 위치 조정 (부모 오브젝트의 머리 위로 오게끔) : 콜라이더 중앙 Y + 콜라이더 높이의 절반 + 추가 오프셋          y축으로 긴 개체들은 콜라이더 최상단이 높아서 HP 바가 몸통 중간에 박히는 것처럼 보일 수 있음. 
        transform.position = new Vector3(colliderCenter.x, colliderCenter.y + halfHeight + verticalOffset, colliderCenter.z);

        //transform.LookAt(Camera.main.transform); XXXX
        //방향을 그냥 카메라를 바라보도록 설정하면 거울 보듯이 거꾸로됨. 그렇기에 카메라가 보는 방향을 보도록 함

        //( 빌보드 ) 방향은 카메라를 바라보도록 함 (정확히는 카메라가 보는 방향을 바라보도록 함)
        transform.rotation = Camera.main.transform.rotation;

        
        // 현재 HP에서 MaxHP로 나눔
        // 둘 중 하나를 float으로 캐스팅
        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}


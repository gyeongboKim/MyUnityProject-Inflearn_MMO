using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 1. 위치 벡터
// 2. 방향 벡터
//      1.거리 (크기)
//      2.실제 방향
//struct MyVector
//{
//    public float x;
//    public float y;
//    public float z;

//    public float magnitude { get { return  Mathf.Sqrt(x*x + y*y + z*z); } }
//    //방향만 갖는 크기가 1인 벡터
//    public MyVector normalized { get { return new MyVector(x/magnitude, y/magnitude, z/magnitude); } }

//    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

//    public static MyVector operator +(MyVector a, MyVector b)
//    {
//        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
//    }
//    public static MyVector operator -(MyVector a, MyVector b)
//    {
//        return new MyVector(a.x +- b.x, a.y - b.y, a.z - b.z);
//    }
//    public static MyVector operator *(MyVector a, float d)
//    {
//        return new MyVector(a.x * d, a.y * d, a.z * d);
//    }


//방향 벡터
// 1. 거리(크기) magnitude
// 2. 실제 방향  normalized 크기가 1인 방향 벡터

//절대 회전값
//transform.eulerAngles = new Vector3(0.0f, _yAngle, 0.0f);

// +- delta
//transform.Rotate(new Vector3(0.0f, Time.deltaTime* _speed, 0.0f));

//Quaternion
//transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yAngle, 0.0f));


//transform.rotation

public class PlayerController : BaseController
{
    //이후 서버가 생기는 경우
    //자신이 플레이 하는 플레이어인지, 다른 플레이어인지 분기점 필요
    PlayerStat _stat;

    bool _stopSkill = false;

    float _attackRange = 2;

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    public override void Init()
    {
        //옵저버 패턴
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        //HP Bar 체크 및 추가
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);


        
    }

    //protected override void UpdateIdle()
    //{


    //}

    protected override void UpdateMoving()
    {
        if (State != Define.State.Moving)
            return;
        //락온된 몬스터가 내 사정거리보다 가까우면 공격 상태로 변경
        //타겟 체크
        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distanceToLockTarget = (_destPos - transform.position).magnitude;

            if(distanceToLockTarget <=  _attackRange )
            {
                State = Define.State.Skill;
                return;
            }
        }

        //이동
        //방향과 크기를 갖는 벡터 추출
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;          //몬스터 위로 올라가는 버그 방지
        //목적지에 도달한 경우 (float 두 값을 뺄셈을 하기 때문에 오차범위가 항상 존재하여 아주 작은 값을 이용)
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        { //Debug.DrawRay(transform.position + Vector3.up * 0.7f, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.7f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                //마우스를 누르고 있는 경우 Idle로 바뀌지 않게 함
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;

            }
            //moveDist = _speed * Time.deltaTime 값이 dir.magnitude를 넘어버려 목적지 부근에서 버벅임 발생
            //해결 : Clamp -> value가 min보다 작으면 min값을, max보다 크면 max값을 덮어줌.(min 과 max 사이값을 보장해줌)
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            //_destPos 방향을 바라봄
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            //transform.LookAt(_destPos);
        } 
    }

    protected override void UpdateSkill()
    {
        //타겟을 바라보도록 함
        //1. 타겟이 있는지 확인
        if(_lockTarget != null)
        {
            // 타겟 방향 벡터 계산 후 타겟 방향벡터로의 회전값 quat 생성
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            // 타겟을 바라보도록 회전(부드럽게 움직이도록 Lerp 사용)
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    #region UpdateDie()
    //protected override void UpdateDie()
    //{
    //    //HP 개념 추가 시 작성
    //}
    #endregion
    #region OnKeyboard()
    // 키보드 이동
    //void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //        transform.position += Vector3.forward * Time.deltaTime * _speed;
    //        //transform.rotation = Quaternion.LookRotation(Vector3.forward); //월드 기준
    //        //transform.Translate로 할 시 커브를 그리며 이동

    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //        transform.position += Vector3.back * Time.deltaTime * _speed;
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //        transform.position += Vector3.left * Time.deltaTime * _speed;
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //        transform.position += Vector3.right * Time.deltaTime * _speed;
    //    }

    //    _moveToDest = false;
    //}
    #endregion
    #region OnStatechanged()
    //protected override void OnStateChanged(Define.State state)
    //{
    //    base.OnStateChanged(state);
    //}
    #endregion

    public void OnHitEvent()
    {
        
        if (_lockTarget != null)
        {
            // TODO
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    void OnMouseEvent(Define.MouseEvent mouseEvent)
    {
        switch (State)
        {
            //MouseEvent 발생 시 State가 Idle 혹은 Moving인 경우 OnMouseEvent_IdleRun 호출
            case Define.State.Idle:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(mouseEvent);
                break;
            case Define.State.Skill:
                {
                    if (mouseEvent == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }

    }

    void OnMouseEvent_IdleRun(Define.MouseEvent mouseEvent) 
    {
        //마우스 클릭 지점에 ray
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        //LayerMask mask = LayerMask.GetMask("Wall");

        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        //누르는 순간에 몬스터인지 판단. 몬스터의 경우 lockTarget 지정
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point; 
                }
                break;
            case Define.MouseEvent.PointerUp:
                {
                    _stopSkill = true;
                }
                break;
        }
    }

    public override void PlayAnimationForState(Define.State state)
    {
        Animator anim = GetComponent<Animator>();
        if (anim == null) return;

        switch (state)
        {
            case Define.State.Idle:
                anim.CrossFade("Idle", 0.15f);
                break;
            case Define.State.Moving:
                anim.CrossFade("OneHandedRun", 0.15f);
                break;
            case Define.State.Skill:
                anim.CrossFade("OneHandedAttack", 0.15f, -1, 0);
                break;
            case Define.State.GetHit:
                //GetHit 애니메이션 추가
                break;
            case Define.State.Die:
                // Die 애니메이션 추가
                break;
        }
    }
}

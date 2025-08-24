using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 10;

    [SerializeField]
    float _attackRange = 2;


    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();

        if(gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

    }

    

    protected override void UpdateIdle()
    {
     
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if(distanceToPlayer <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;

            return;
        }
        
    }
    protected override void UpdateMoving()
    {
        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        //움직이다가 타겟이 내 사정거리보다 가까우면 공격 시도
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distanceToLockTarget = (_destPos - transform.position).magnitude;

            if (distanceToLockTarget <= _attackRange)
            {   
                //nma를 이용하여 길찾기 
                nma.SetDestination(transform.position);
                
                State = Define.State.Skill;
                return;
            }
        }

        //이동
        //방향과 크기를 갖는 벡터 추출
        Vector3 dir = _destPos - transform.position;
        //목적지에 도달한 경우 (float 두 값을 뺄셈을 하기 때문에 오차범위가 항상 존재하여 아주 작은 값을 이용)
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            //nma를 이용하여 길찾기 
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;
 
            //_destPos 방향을 바라봄
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
            //transform.LookAt(_destPos);
        }
    }
    protected override void UpdateSkill()
    {
        //타겟을 바라보도록 함
        //1. 타겟이 있는지 확인
        if (_lockTarget != null)
        {
            // 타겟 방향 벡터 계산 후 타겟 방향벡터로의 회전값 quat 생성
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            // 타겟을 바라보도록 회전(부드럽게 움직이도록 Lerp 사용)
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    public void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
            
        }
        else
        {
            State = Define.State.Idle;
        }
    }

    //hit과 scan 분리. (맞는 시점에 바로 피가 닳고, 공격이 끝나는 시점에 scan하기 위해서)
    public void OnScanEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            if (targetStat.Hp > 0)
            {
                float distanceToLockTarget = (_lockTarget.transform.position - transform.position).magnitude;
                if (distanceToLockTarget <= _attackRange)
                    State = Define.State.Skill;
                else
                    State = Define.State.Moving;
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
        }

    }

    public override void PlayAnimationForState(Define.State state)
    {
        Animator anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning($"Animator component missing on '{gameObject.name}'.");
            return;
        }

        switch (state)
        {
            case Define.State.Idle:
                anim.CrossFade("Idle", 0.15f);
                break;
            case Define.State.Moving:
                anim.CrossFade("Run", 0.15f);
                break;
            case Define.State.Skill:
                anim.CrossFade("Attack01", 0.15f, -1, 0);
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

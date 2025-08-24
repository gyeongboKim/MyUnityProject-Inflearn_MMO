using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;       //목적지

    [SerializeField]
    protected GameObject _lockTarget;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    
    protected virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            //상태 변환에 따라 애니메이션 재생 메서드 호출
            PlayAnimationForState(_state);

        }
    }

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;

        }
    }

    public virtual void PlayAnimationForState(Define.State state) { }

    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }

    
}

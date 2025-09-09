using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


namespace Data
{
    #region enum: Job, SkillType, TargetingType, TargetType
    public enum Job
    {
        None,       // 주 능력치, 보조 능력치, 명중 보정, 치명타
        Warrior,    //민첩(Agility), 활력(Vitality), 근거리 명중 보정, 치명타
        Mage,       //지능(Intellect), 의지(Wisdom), 조작 간섭 보정, 치명타
        Archer,     //힘(Strength), 민첩(Agility), 원거리 명중 보정, 치명타
        Thief,      //행운(Luck), 민첩(Agility) 근거리 및 원거리 명중 보정, 치명타
        Priest,     //신앙(Faith), 의지(Wisdom), 조작 간섭 보정, 치명타 
    }

    [Flags]
    public enum SkillType
    {
        None = 0,
        //1. 발동 방식
        Active = 1 << 0,  // 1 (00000001) - 직접 사용
        Passive = 1 << 1, // 2 (00000010) - 지속 효과

        //2. 주 효과
        Attack = 1 << 2,  // 4 (00000100) - 공격
        Buff = 1 << 3,    // 8 (00001000) - 아군 강화
        Debuff = 1 << 4,  // 16 (00010000) - 적군 약화
        Heal = 1 << 5,    // 32 (00100000) - 치유

        // 3. 부가 효과 (Sub Effect)
        CrowdControl = 1 << 6, // 64 (01000000) - 군중 제어 (스턴, 속박 등)
    }

    public enum DamageType
    { 
        None,
        Physics,
        Masic,
    }

    //스킬 시전 방식
    public enum SkillCastType
    {
        None,
        //1. 타겟팅, 논타겟팅  
        Targeting,          //대상 직접 클릭
        NonTargeting,       //허공, 바닥 등 아무데나 클릭 시전
        //2. 시전  
        Instant,    //즉발 스킬
        NonInstant, // 기본적인 스킬(즉발은 아니지만 아주 짧은 캐스팅 시간 존재)
        Holding,    //
        Channeling, // 공격, 스킬, 이동 및 CC 상태 시 해제
        Casting,    //정신집중 스킬 (행동 불가 및 CC 상태 시 해제)
    }
    
    //스킬 적용 방식
    public enum SkillEffectLogic
    {
        None,
        Single,     // 단일 대상에게 효과 적용
        Area,       // 특정 지점 주위 원형/사각형 범위에 효과 적용
        Projectile, // 발사체를 날려 처음 충돌하는 대상에게 효과 적용
        Chain,      // 여러 대상에게 연쇄적으로 효과 적용 (나중을 위해)
    }

    public enum SkillTargetType
    {
        None,
        Enemy,
        Ally,
        Self,
    }
    #endregion enum: Job, SkillType, TargetingType, TargetType

    #region Stat
    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int defense;
        public int totalExp;
       
    }
    
    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }
    #endregion Stat

    #region Skill
    [Serializable]
    public class Skill
    {
        public int id;
        public string name;
        public string description;              //설명
        public Job job;
        public SkillType skillType;             //스킬의 종류 (액티브, 패시브, 버프 등)
        public SkillEffectLogic skillEffectLogic;     //스킬 효과 적용 방식
        public SkillTargetType skillTargetType;           //스킬의 대상 (적, 아군, 자신 등)'
        public ProjectileInfo projectile;       //스킬의 타겟팅 방식이 투사체인 경우
        public int powerBasicdamage;          // 스킬의 공격력 (데미지 계산에 사용)
        public int powerBasicHeal;            //스킬의 치유량 (힐 계산에 사용)
        public int manaCost;       //스킬 사용에 필요한 마나 비용
        public float skillMultiplier;   //스킬 계수
        public float cooldown;     //스킬의 재사용 대기시간
        public float activeTime;    //스킬의 지속 시간 (버프, 디버프 등)
        public float range;        //스킬의 사거리
        public float areaOfEffect; //스킬의 효과 범위 (광역 스킬일 경우)
        public string iconPath;     //스킬 아이콘 경로
    }

    [Serializable]
    public class ProjectileInfo
    {
        public string prefabPath;           // 투사체 프리팹 경로
        public float speed;                 // 투사체 이동 속도
        public float maxDistance;           // 최대 비행 거리
        public float radius;                // 충돌 판정 반경
        public bool piercing;               // 관통 여부
        public int maxTargets;              // 최대 타격 대상 수 (관통 시)
        public bool destroyOnHit;           // 충돌 시 파괴 여부
        public string hitEffectPath;        // 충돌 이펙트 경로
        public float gravityScale;          // 중력 영향도 (포물선 투사체용)
        public ProjectileMovementType movementType; // 투사체 이동 방식
    }

    public enum ProjectileMovementType
    {
        Straight,       // 직선 이동
        Homing,         // 유도탄
        Arc,            // 포물선 (화살, 폭탄 등)
        Boomerang,      // 부메랑 (되돌아옴)
    }

    [Serializable]
    public class SkillData : ILoader<int, Skill>
    {
        public List<Skill> skills = new List<Skill>();

        public Dictionary<int, Skill> MakeDict()
        {
            Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
            foreach (Skill skill in skills)
                dict.Add(skill.id, skill);
            return dict;

        }
    }

    #endregion Skill
}

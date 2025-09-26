using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


namespace Data
{
    #region enum: Job, BonusStatType, AttributeType, SkillType, TargetingType, TargetType, ItemType, 
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
    public enum StatType
    {
        None,
        MaxHp,
        MaxEnergyShield,
        MaxMp,
        MaxStamina,

        HpRegen,
        EnergyShieldRegen,
        MpRegen,
        StaminaRegen,

        PhysicalDamage,
        AttackRating,
        AttackSpeed,

        AbilityPower,
        CastSpeed,

        Defense,
        Discernment,           //분별력(마법 피해 감소)
        Toughness,
        ResistanceFire,
        ResistanceCold,
        ResistanceLightning,
        ResistanceChaos,

        MoveSpeed,

    }

    [Flags]
    public enum AttributeType
    {
        None = 0,
        Strength = 1 << 0,     // 1     (00000001) - 힘
        Dexterity = 1 << 1,    // 2     (00000010) - 민첩
        Agility = 1 << 2,      // 4     (00000100) - 활력
        Intellect = 1 << 3,    // 8     (00001000) - 지능
        Wisdom = 1 << 4,       // 16    (00010000) - 의지
        Vitality = 1 << 5,     // 32    (00100000) - 체력
        Faith = 1 << 6,        // 64    (01000000) - 신앙
        Luck = 1 << 7,         //128    (10000000) - 행운
    }

    [Flags]
    public enum SkillType
    {
        None = 0,
        //1. 발동 방식
        Active = 1 << 0,  // 1  (00000001) - 직접 사용
        Passive = 1 << 1, // 2  (00000010) - 지속 효과

        //2. 주 효과
        Attack = 1 << 2,  // 4  (00000100) - 공격
        Buff = 1 << 3,    // 8  (00001000) - 아군 강화
        Debuff = 1 << 4,  // 16 (00010000) - 적군 약화
        Heal = 1 << 5,    // 32 (00100000) - 치유

        // 3. 부가 효과 (Sub Effect)
        CrowdControl = 1 << 6, // 64 (01000000) - 군중 제어 (스턴, 속박 등)
    }

    [Flags]
    public enum DamageType
    {
        None = 0,
        Physics = 1 << 0,   // 1    (000001)    물리
        Spell = 1 << 1,     // 2    (000010)   주문
        Fire = 1 << 2,      // 4    (000100)    화염
        Cold = 1 << 3,      // 8    (001000)    냉기   
        Lightning = 1 << 4, // 16   (010000)    전기
        Chaos = 1 << 5,     // 32   (100000)    카오스, 독
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

    public enum ItemType
    {
        None,

        //Two-hand
        TwoHandedSword,
        TwoHandedAxe,
        Bow,
        Staff,
        //One-hand
        OneHandedSword,
        OneHandedAxe,
        Dagger,
        Mace,
        Wand,
        Shield,
        Orb,
        Spellbook,
        Quiver,

        //Equipment
        Helmet,
        Armor,
        Gloves,
        Boots,

        OneRing,
        TwoRing,
        Necklace,

        Consumable,
        Material,
        Quest,
    }
    #endregion enum: Job, SkillType, TargetingType, TargetType, ItemType

    #region Stat
    [Serializable]
    public class BaseStat
    {
        //레벨에 따른 기본 스탯
        public int level;
        public int baseMaxHp;
        public int baseMaxMp;
        public int baseMaxStamina;
        public int basePhysicalDamage;
        public int baseDefense;

        public int totalExp;
    }
    [Serializable]
    public class BaseStatData : ILoader<int, BaseStat>
    {
        public List<BaseStat> stats = new List<BaseStat>();

        public Dictionary<int, BaseStat> MakeDict()
        {
            Dictionary<int, BaseStat> dict = new Dictionary<int, BaseStat>();
            foreach (BaseStat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }




    [Serializable]
    public class PlayerAttributes
    {
        //플레이어 기본 속성
        public int strength;     // attackDamage 증가
        public int dexterity;    // attackRating, attackSpeed 증가
        public int agility;      // moveSpeed, maxStamina, staminaRegen 증가
        public int intellect;    // abilityPower, castSpeed 증가
        public int wisdom;       // maxMp, mpRegen, resistanceChaos 증가 (한계치 존재)
        public int vitality;     // maxHp, hpRegen, toughness 증가
        public int faith;        // itemFindChance 증가 (한계치 존재)
        public int luck;         // itemFindChance 증가 (한계치 존재)

        public int availablePoints; // 사용 가능한 스탯 포인트
    }
    [Serializable]
    public class StatConstants
    {
        public Dictionary<string, float> statMultipliers = new Dictionary<string, float>();
        public Dictionary<string, int> statCaps = new Dictionary<string, int>();
        // 필요에 따라 추가...
    }
    [Serializable]
    public class  StatConstantsData
    {
        public StatConstants constants;
    }

    [Serializable]
    public class Monster
    {
        public int id;
        public string name;
        public string prefabPath;        //몬스터 프리팹 경로
        public float attackRange;       //몬스터 공격 사거리
        public DamageType damageType;          //고유 피해 종류 (물리, 원소 등)
        public float statMultiplier;    //고유 스탯 배율
        public float expMultiplier;     //고유 경험치 배율
        public int goldReward;
        public float itemDropChance;   //아이템 발견 확률
    }

    [Serializable]
    public class MonsterData : ILoader<int, Monster>
    {
        public List<Monster> Monsters = new List<Monster>();
        public Dictionary<int, Monster> MakeDict()
        {
            Dictionary<int, Monster> dict = new Dictionary<int, Monster>();
            foreach (Monster monster in Monsters)
                dict.Add(monster.id, monster); 
            return dict;
        }
    }
    //public class StatData : ILoader<int, Stat>
    //{
    //    public List<Stat> stats = new List<Stat>();

    //    public Dictionary<int, Stat> MakeDict()
    //    {
    //        Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
    //        foreach (Stat stat in stats)
    //            dict.Add(stat.level, stat);
    //        return dict;
    //    }
    //}
    #endregion Stat

    #region Item, BuffDebuff


    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public string description;
        public ItemType itemType;

        public int requiredLevel;
        public Dictionary<AttributeType, int> requiredAttributes = new Dictionary<AttributeType, int>();

        public Dictionary<StatType, Dictionary<int, bool>> bonusStats = new Dictionary<StatType, Dictionary<int, bool>>();
        public Dictionary<DamageType, Dictionary<int, bool>> bonusDamages = new Dictionary<DamageType, Dictionary<int, bool>>();

        
        public int durability; // 내구도
        public int price;

        public string iconPath;

    }

    [Serializable]
    public class ItemData : ILoader<int, Item>
    {
        public List<Item> Items = new List<Item>();
        public Dictionary<int, Item> MakeDict()
        {
            Dictionary<int, Item> dict = new Dictionary<int, Item>();
            foreach (Item item in Items)
                dict.Add(item.id, item);
            return dict;
        }
    }
    [Serializable]
    public class BuffDebuff
    {
        // 필요에 따라 추가...
        public int id;
        public string name;
        public string description;
        public float activeTime; // 지속 시간
        public bool isDebuff; // 디버프 여부

        public string iconPath;

    }
    #endregion Item, BuffDebuff

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
        public int projectileId;       //스킬의 타겟팅 방식이 투사체인 경우
        public int buffDebuffId;      //스킬이 적용하는 버프/디버프 ID
        public int basicDamage;          // 스킬의 공격력 (데미지 계산에 사용)\
        public DamageType damageType;          // 피해 종류 (물리, 원소 등)
        public int basicHeal;            //스킬의 치유량 (힐 계산에 사용)
        public int manaCost;       //스킬 사용에 필요한 마나 비용
        public float skillMultiplier;   //스킬 계수
        public float cooldown;     //스킬의 재사용 대기시간
        public float range;        //스킬의 사거리
        public float areaOfEffect; //스킬의 효과 범위 (광역 스킬일 경우)

        public string iconPath;     //스킬 아이콘 경로
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

    [Serializable]
    public class Projectile
    {
        public int id;
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
        Arc,            // 포물선 
        Boomerang,      // 부메랑 (되돌아옴)
    }

    [Serializable]
    public class  ProjectileData : ILoader<int, Projectile> 
    {
        public List<Projectile> projectiles = new List<Projectile>();

        public Dictionary<int , Projectile> MakeDict()
        {
            Dictionary<int, Projectile> dict = new Dictionary<int, Projectile>();
            foreach (Projectile projectile in projectiles)
                dict.Add(projectile.id, projectile);
            return dict;
        }
    }
    #endregion Skill
}

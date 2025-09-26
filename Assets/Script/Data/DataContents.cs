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
        None,       // �� �ɷ�ġ, ���� �ɷ�ġ, ���� ����, ġ��Ÿ
        Warrior,    //��ø(Agility), Ȱ��(Vitality), �ٰŸ� ���� ����, ġ��Ÿ
        Mage,       //����(Intellect), ����(Wisdom), ���� ���� ����, ġ��Ÿ
        Archer,     //��(Strength), ��ø(Agility), ���Ÿ� ���� ����, ġ��Ÿ
        Thief,      //���(Luck), ��ø(Agility) �ٰŸ� �� ���Ÿ� ���� ����, ġ��Ÿ
        Priest,     //�ž�(Faith), ����(Wisdom), ���� ���� ����, ġ��Ÿ 
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
        Discernment,           //�к���(���� ���� ����)
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
        Strength = 1 << 0,     // 1     (00000001) - ��
        Dexterity = 1 << 1,    // 2     (00000010) - ��ø
        Agility = 1 << 2,      // 4     (00000100) - Ȱ��
        Intellect = 1 << 3,    // 8     (00001000) - ����
        Wisdom = 1 << 4,       // 16    (00010000) - ����
        Vitality = 1 << 5,     // 32    (00100000) - ü��
        Faith = 1 << 6,        // 64    (01000000) - �ž�
        Luck = 1 << 7,         //128    (10000000) - ���
    }

    [Flags]
    public enum SkillType
    {
        None = 0,
        //1. �ߵ� ���
        Active = 1 << 0,  // 1  (00000001) - ���� ���
        Passive = 1 << 1, // 2  (00000010) - ���� ȿ��

        //2. �� ȿ��
        Attack = 1 << 2,  // 4  (00000100) - ����
        Buff = 1 << 3,    // 8  (00001000) - �Ʊ� ��ȭ
        Debuff = 1 << 4,  // 16 (00010000) - ���� ��ȭ
        Heal = 1 << 5,    // 32 (00100000) - ġ��

        // 3. �ΰ� ȿ�� (Sub Effect)
        CrowdControl = 1 << 6, // 64 (01000000) - ���� ���� (����, �ӹ� ��)
    }

    [Flags]
    public enum DamageType
    {
        None = 0,
        Physics = 1 << 0,   // 1    (000001)    ����
        Spell = 1 << 1,     // 2    (000010)   �ֹ�
        Fire = 1 << 2,      // 4    (000100)    ȭ��
        Cold = 1 << 3,      // 8    (001000)    �ñ�   
        Lightning = 1 << 4, // 16   (010000)    ����
        Chaos = 1 << 5,     // 32   (100000)    ī����, ��
    }

    //��ų ���� ���
    public enum SkillCastType
    {
        None,
        //1. Ÿ����, ��Ÿ����  
        Targeting,          //��� ���� Ŭ��
        NonTargeting,       //���, �ٴ� �� �ƹ����� Ŭ�� ����
        //2. ����  
        Instant,    //��� ��ų
        NonInstant, // �⺻���� ��ų(����� �ƴ����� ���� ª�� ĳ���� �ð� ����)
        Holding,    //
        Channeling, // ����, ��ų, �̵� �� CC ���� �� ����
        Casting,    //�������� ��ų (�ൿ �Ұ� �� CC ���� �� ����)
    }

    //��ų ���� ���
    public enum SkillEffectLogic
    {
        None,
        Single,     // ���� ��󿡰� ȿ�� ����
        Area,       // Ư�� ���� ���� ����/�簢�� ������ ȿ�� ����
        Projectile, // �߻�ü�� ���� ó�� �浹�ϴ� ��󿡰� ȿ�� ����
        Chain,      // ���� ��󿡰� ���������� ȿ�� ���� (������ ����)
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
        //������ ���� �⺻ ����
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
        //�÷��̾� �⺻ �Ӽ�
        public int strength;     // attackDamage ����
        public int dexterity;    // attackRating, attackSpeed ����
        public int agility;      // moveSpeed, maxStamina, staminaRegen ����
        public int intellect;    // abilityPower, castSpeed ����
        public int wisdom;       // maxMp, mpRegen, resistanceChaos ���� (�Ѱ�ġ ����)
        public int vitality;     // maxHp, hpRegen, toughness ����
        public int faith;        // itemFindChance ���� (�Ѱ�ġ ����)
        public int luck;         // itemFindChance ���� (�Ѱ�ġ ����)

        public int availablePoints; // ��� ������ ���� ����Ʈ
    }
    [Serializable]
    public class StatConstants
    {
        public Dictionary<string, float> statMultipliers = new Dictionary<string, float>();
        public Dictionary<string, int> statCaps = new Dictionary<string, int>();
        // �ʿ信 ���� �߰�...
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
        public string prefabPath;        //���� ������ ���
        public float attackRange;       //���� ���� ��Ÿ�
        public DamageType damageType;          //���� ���� ���� (����, ���� ��)
        public float statMultiplier;    //���� ���� ����
        public float expMultiplier;     //���� ����ġ ����
        public int goldReward;
        public float itemDropChance;   //������ �߰� Ȯ��
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

        
        public int durability; // ������
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
        // �ʿ信 ���� �߰�...
        public int id;
        public string name;
        public string description;
        public float activeTime; // ���� �ð�
        public bool isDebuff; // ����� ����

        public string iconPath;

    }
    #endregion Item, BuffDebuff

    #region Skill
    [Serializable]
    public class Skill
    {
        public int id;
        public string name;
        public string description;              //����
        public Job job;
        public SkillType skillType;             //��ų�� ���� (��Ƽ��, �нú�, ���� ��)
        public SkillEffectLogic skillEffectLogic;     //��ų ȿ�� ���� ���
        public SkillTargetType skillTargetType;           //��ų�� ��� (��, �Ʊ�, �ڽ� ��)'
        public int projectileId;       //��ų�� Ÿ���� ����� ����ü�� ���
        public int buffDebuffId;      //��ų�� �����ϴ� ����/����� ID
        public int basicDamage;          // ��ų�� ���ݷ� (������ ��꿡 ���)\
        public DamageType damageType;          // ���� ���� (����, ���� ��)
        public int basicHeal;            //��ų�� ġ���� (�� ��꿡 ���)
        public int manaCost;       //��ų ��뿡 �ʿ��� ���� ���
        public float skillMultiplier;   //��ų ���
        public float cooldown;     //��ų�� ���� ���ð�
        public float range;        //��ų�� ��Ÿ�
        public float areaOfEffect; //��ų�� ȿ�� ���� (���� ��ų�� ���)

        public string iconPath;     //��ų ������ ���
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
        public string prefabPath;           // ����ü ������ ���
        public float speed;                 // ����ü �̵� �ӵ�
        public float maxDistance;           // �ִ� ���� �Ÿ�
        public float radius;                // �浹 ���� �ݰ�
        public bool piercing;               // ���� ����
        public int maxTargets;              // �ִ� Ÿ�� ��� �� (���� ��)
        public bool destroyOnHit;           // �浹 �� �ı� ����
        public string hitEffectPath;        // �浹 ����Ʈ ���
        public float gravityScale;          // �߷� ���⵵ (������ ����ü��)
        public ProjectileMovementType movementType; // ����ü �̵� ���
    }

    public enum ProjectileMovementType
    {
        Straight,       // ���� �̵�
        Homing,         // ����ź
        Arc,            // ������ 
        Boomerang,      // �θ޶� (�ǵ��ƿ�)
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

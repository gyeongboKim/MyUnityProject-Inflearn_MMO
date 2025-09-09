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
        None,       // �� �ɷ�ġ, ���� �ɷ�ġ, ���� ����, ġ��Ÿ
        Warrior,    //��ø(Agility), Ȱ��(Vitality), �ٰŸ� ���� ����, ġ��Ÿ
        Mage,       //����(Intellect), ����(Wisdom), ���� ���� ����, ġ��Ÿ
        Archer,     //��(Strength), ��ø(Agility), ���Ÿ� ���� ����, ġ��Ÿ
        Thief,      //���(Luck), ��ø(Agility) �ٰŸ� �� ���Ÿ� ���� ����, ġ��Ÿ
        Priest,     //�ž�(Faith), ����(Wisdom), ���� ���� ����, ġ��Ÿ 
    }

    [Flags]
    public enum SkillType
    {
        None = 0,
        //1. �ߵ� ���
        Active = 1 << 0,  // 1 (00000001) - ���� ���
        Passive = 1 << 1, // 2 (00000010) - ���� ȿ��

        //2. �� ȿ��
        Attack = 1 << 2,  // 4 (00000100) - ����
        Buff = 1 << 3,    // 8 (00001000) - �Ʊ� ��ȭ
        Debuff = 1 << 4,  // 16 (00010000) - ���� ��ȭ
        Heal = 1 << 5,    // 32 (00100000) - ġ��

        // 3. �ΰ� ȿ�� (Sub Effect)
        CrowdControl = 1 << 6, // 64 (01000000) - ���� ���� (����, �ӹ� ��)
    }

    public enum DamageType
    { 
        None,
        Physics,
        Masic,
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
        public string description;              //����
        public Job job;
        public SkillType skillType;             //��ų�� ���� (��Ƽ��, �нú�, ���� ��)
        public SkillEffectLogic skillEffectLogic;     //��ų ȿ�� ���� ���
        public SkillTargetType skillTargetType;           //��ų�� ��� (��, �Ʊ�, �ڽ� ��)'
        public ProjectileInfo projectile;       //��ų�� Ÿ���� ����� ����ü�� ���
        public int powerBasicdamage;          // ��ų�� ���ݷ� (������ ��꿡 ���)
        public int powerBasicHeal;            //��ų�� ġ���� (�� ��꿡 ���)
        public int manaCost;       //��ų ��뿡 �ʿ��� ���� ���
        public float skillMultiplier;   //��ų ���
        public float cooldown;     //��ų�� ���� ���ð�
        public float activeTime;    //��ų�� ���� �ð� (����, ����� ��)
        public float range;        //��ų�� ��Ÿ�
        public float areaOfEffect; //��ų�� ȿ�� ���� (���� ��ų�� ���)
        public string iconPath;     //��ų ������ ���
    }

    [Serializable]
    public class ProjectileInfo
    {
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
        Arc,            // ������ (ȭ��, ��ź ��)
        Boomerang,      // �θ޶� (�ǵ��ƿ�)
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

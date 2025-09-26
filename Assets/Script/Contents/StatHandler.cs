using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StatHandler : MonoBehaviour
{

    public Data.BaseStat BaseStat { get; protected set; }
    

    [SerializeField]
    protected int _level;

    //현재 HP, MP, EnergyShield
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _energyShield;
    [SerializeField]
    protected int _mp;
    [SerializeField]
    protected int _stamina;


    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _maxEnergyShield;
    [SerializeField]
    protected int _maxMp;
    [SerializeField]
    protected int _maxStamina;

    [SerializeField]
    protected float _hpRegen;
    [SerializeField]
    public float _energyShieldRegen;
    [SerializeField]
    protected float _mpRegen;
    [SerializeField]
    protected float _staminaRegen;

    [SerializeField]
    protected int _attackDamage;
    [SerializeField]
    protected int _attackRating;
    [SerializeField]
    protected float _attackSpeed;

    [SerializeField] 
    protected int _abilityPower;
    [SerializeField]
    protected float _castSpeed;

    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected int _discernment;      
    [SerializeField]
    protected int _toughness;
    [SerializeField] 
    protected int _resistanceFire;
    [SerializeField]
    protected int _resistanceCold;
    [SerializeField] 
    protected int _resistanceLightning;
    [SerializeField]
    protected int _resistanceChaos;

    [SerializeField]
    protected int _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int EnergyShield { get { return _energyShield; } set { _energyShield = value; } }
    public int Mp { get { return _mp; } set { _mp = value; } }
    public int Stamina { get { return _stamina; } set { _stamina = value; } }

    public abstract int MaxHp { get; }
    public abstract int MaxEnergyShield { get; }
    public abstract int MaxMp { get; }
    public abstract int MaxStamina { get; }

    public abstract float HpRegen { get; }
    public abstract float EnergyShieldRegen { get; }
    public abstract float MpRegen { get; }
    public abstract float StaminaRegen { get; }

    public abstract int AttackDamage { get; }
    public abstract int AttackRating { get; }
    public abstract float AttackSpeed { get; }

    public abstract int AbilityPower { get; }
    public abstract float CastSpeed { get; }

    public abstract int Defense { get; }
    public abstract int Discernment { get; }
    public abstract int Toughness { get; }
    public abstract int ResistanceFire { get; }  
    public abstract int ResistanceCold { get; }
    public abstract int ResistanceLightning { get; }

    public abstract int MoveSpeed { get; }

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    public abstract void SetBaseStat(int level);
    

    public virtual void OnAttacked(StatHandler attackerStatHandler)
    {
        int damage = Mathf.Max(0, attackerStatHandler.AttackDamage - Defense);
        if(EnergyShield > 0)
        {
            // 에너지 쉴드가 남아있으면 에너지 쉴드부터 깎임
            int shieldDamage = Mathf.Min(EnergyShield, damage);
            EnergyShield -= shieldDamage;
            return;     // 에너지 쉴드가 남아있으면 HP에 데미지를 주지 않음
        }
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attackerStatHandler);
        }
    }

 

    protected virtual void OnDead(StatHandler attackerStatHandler)
    {
        Managers.Game.Despawn(gameObject);   
    }

    //protected virtual void OnDead(StatHandler attackerStatHandler)
    //{
    //    Managers.Game.Despawn(gameObject);
    //    PlayerStatHandler playerStatHandler = attackerStatHandler as PlayerStatHandler;
    //    if (playerStatHandler != null)
    //    {
    //        playerStatHandler.CurrentExp += 6;
    //    }
    //}


    //// 최종 스탯 계산 (기본값 + 속성보너스 + 아이템보너스 + 버프/디버프)
    //public int CalculateAttackDamage(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.strength * StatConstants.statMultipliers["strengthToAttackDamage"]);
    //    return baseStat.maxHp + attributeBonus + itemBonus.attackDamage + buffDebuff.attackDamage; // baseStat에는 기본 공격력이 없으므로 0으로 시작
    //}

    //public int CalculateAttackRating(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.dexterity * StatConstants.statMultipliers["dexterityToAttackRating"]);
    //    return attributeBonus + itemBonus.attackRating + buffDebuff.attackRating;
    //}

    //public float CalculateAttackSpeed(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    float attributeBonus = attributes.agility * StatConstants.statMultipliers["dexterityToAttackSpeed"];
    //    return 1.0f + attributeBonus + itemBonus.attackSpeed + buffDebuff.attackSpeed; // 기본 공속 1.0
    //}

    //public int CalculateAbilityPower(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.intellect * StatConstants.statMultipliers["intellect_to_abilityPower"]);
    //    return attributeBonus + itemBonus.abilityPower + buffDebuff.abilityPower;
    //}

    //public float CalculateCastSpeed(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    float attributeBonus = attributes.intellect * StatConstants.statMultipliers["intellect_to_castSpeed"];
    //    return 1.0f + attributeBonus + itemBonus.castSpeed + buffDebuff.castSpeed;
    //}

    //public int CalculateMaxHp(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.vitality * StatConstants.statMultipliers["vitality_to_maxHp"]);
    //    return baseStat.maxHp + attributeBonus + itemBonus.maxHp + buffDebuff.maxHp;
    //}

    //public int CalculateMaxMp(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    return baseStat.maxMp + itemBonus.maxMp + buffDebuff.maxMp;
    //}

    //public int CalculateDefense(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    return baseStat.defense + itemBonus.defense + buffDebuff.defense + buffDebuff.defense;
    //}

    //public int CalculateResistanceChaos(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.wisdom * StatConstants.statMultipliers["wisdom_to_resistanceChaos"]);
    //    Mathf.Min(attributeBonus, StatConstants.statCaps["resistance_based_on_attributes"]); // 한계치 적용(30)
    //    int total = attributeBonus + itemBonus.resistanceChaos + buffDebuff.resistanceChaos;
    //    return Mathf.Min(total, StatConstants.statCaps["resistance"]); // 한계치 적용(75)
    //}

    //public int CalculateItemFindChance(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus)
    //{
    //    int faithBonus = Mathf.FloorToInt(attributes.faith * StatConstants.statMultipliers["faith_to_itemFindChance"]);
    //    Mathf.Min(faithBonus, StatConstants.statCaps["itemFindChance_based_on_attributes"]); // 한계치 적용(100)
    //    int luckBonus = Mathf.FloorToInt(attributes.luck * StatConstants.statMultipliers["luck_to_itemFindChance"]);
    //    Mathf.Min(luckBonus, StatConstants.statCaps["itemFindChance_based_on_attributes"]); // 한계치 적용(100)
    //    int total = faithBonus + luckBonus + itemBonus.itemFindChance;
    //    return total;
    //}

    //public int CalculateMoveSpeed(Data.BaseStat baseStat, Data.PlayerAttributes attributes, Data.ItemBonus itemBonus, Data.BuffDebuff buffDebuff)
    //{
    //    int attributeBonus = Mathf.FloorToInt(attributes.agility * StatConstants.statMultipliers["agility_to_moveSpeed"]);
    //    return 100 + attributeBonus + itemBonus.moveSpeed + buffDebuff.moveSpeed; // 기본 이속 100
    //}

    //// 레벨업 시 획득하는 스탯 포인트 계산
    //public static int CalculateStatPointsPerLevel(int level)
    //{
    //    return 5; // 레벨당 5 스탯 포인트 (조정 가능)
    //}
}


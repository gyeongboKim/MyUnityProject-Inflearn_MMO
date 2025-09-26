using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : StatHandler
{
    //���� EXP, Gold
    [Header("Player Specific Stat")]
    [SerializeField]
    protected int _currentExp;
    [SerializeField]
    protected int _gold;
    [SerializeField]
    protected int _itemFindChance; // ������ �߰� Ȯ��

    

    public int CurrentExp
    {
        get { return _currentExp; }
        set
        {

            _currentExp = value;
            CheckLevelUp();
            
        }
    }

    private void CheckLevelUp()
    {         //������ üũ
        int currentLevel = Level;
        while (true)
        {
            Data.BaseStat playerBaseStat;
            //���� ���� ����
            if (Managers.Data.BaseStatDict.TryGetValue(currentLevel + 1, out playerBaseStat) == false)
                break;
            if (_currentExp < playerBaseStat.totalExp)
                break;
            currentLevel++;
        }
        if (currentLevel != Level)
        {
            Debug.Log("Level Up!");
            Level = currentLevel;
            SetBaseStat(Level);
        }
    }
    public int Gold { get { return _gold; } set { _gold = value; } }
    public int itemFindChance { get { return _itemFindChance; } set { _itemFindChance = value; } }

    public override void Init()
    {
        SetBaseStat(1);
    }

    public override void SetBaseStat(int level)
    {
        //Dictionary<int, Data.PlayerStat> dict = Managers.Data.PlayerStatDict;
        //Data.Stat stat = dict[level];
        BaseStat = Managers.Data.BaseStatDict[level];



        _level = level;
        _maxHp = BaseStat.baseMaxHp;
        _hp = _maxHp;
        _maxMp = BaseStat.baseMaxMp;
        _mp = _maxMp;
        _attackDamage = BaseStat.baseAttackDamage;
        _defense = BaseStat.baseDefense;

    }

}

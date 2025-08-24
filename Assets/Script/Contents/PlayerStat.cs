using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    public int Exp { 
        get { return _exp; } 
        set 
        { 
            
            _exp = value;
            //레벨업 체크
            int currentLevel = Level;
            while (true)
            {
                Data.Stat stat;
                //다음 레벨 없음
                if (Managers.Data.StatDict.TryGetValue(currentLevel + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                currentLevel++;
            }

            if(currentLevel != Level)
            {
                Debug.Log("Level Up!");
                Level = currentLevel;
                SetStat(Level);
            }
        } 
    }
    public int Gold { get { return _gold; } set { _gold = value; } }

    private void Start()
    {
        _level = 1;

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[1];
        SetStat(_level);

        _moveSpeed = 5.0f;

        _exp = 0;
        _gold = 500;
    }

    public void SetStat (int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
        _defense = stat.defense;

    }

    protected override void OnDead(Stat attackerStat)
    {
        Debug.Log("Player Dead");

    }
}

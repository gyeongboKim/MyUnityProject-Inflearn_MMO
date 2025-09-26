using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterStatHandler : StatHandler
{
    [SerializeField]
    protected int _goldReward;
    [SerializeField]
    protected float _itemDropChance; // 기본 아이템 드랍 확률
    

    public Data.BaseStat MonsterStatData
    {
        get;
        private set;
    }

    public int GoldReward {get { return _goldReward; } set { _goldReward = value; } }
    public float itemDropChance { get { return _itemDropChance; } set { _itemDropChance = value; } }

    public override void Init()
    {
        _level = 1;
        Dictionary<int, Data.BaseStat> dict = Managers.Data.BaseStatDict;
        MonsterStatData = dict[_level];


        SetBaseStat(_level);

        
    }

    public override void SetBaseStat(int level)
    {
        Dictionary<int, Data.BaseStat> dict = Managers.Data.BaseStatDict;
        MonsterStatData = dict[_level];

        _maxHp = MonsterStatData.baseMaxHp;
        _hp = MonsterStatData.baseMaxHp;
        
        
    }

    protected override void OnDead(StatHandler attackerStatHandler)
    {
        PlayerStatHandler playerStatHandler = attackerStatHandler as PlayerStatHandler;
        if (playerStatHandler != null)
        {

            // 공격자가 플레이어라면, 플레이어의 OnKill 함수를 호출해 경험치를 준다.
            playerStatHandler.CurrentExp += MonsterStatData.totalExp;
            // 아이템 드랍 로직?
            float dropChance = 0;
            dropChance += itemDropChance * (1 + playerStatHandler.itemFindChance/100);  // 플레이어의 아이템 발견 확률에 비례하여 드랍 확률 증가

            if (Random.Range(0, 100) < dropChance)
            {
                Debug.Log("Item Dropped!");
            }
        }

        base.OnDead(attackerStatHandler);
    }
}

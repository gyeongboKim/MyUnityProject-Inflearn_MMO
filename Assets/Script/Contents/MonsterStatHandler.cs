using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterStatHandler : StatHandler
{
    [SerializeField]
    protected int _goldReward;
    [SerializeField]
    protected float _itemDropChance; // �⺻ ������ ��� Ȯ��
    

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

            // �����ڰ� �÷��̾���, �÷��̾��� OnKill �Լ��� ȣ���� ����ġ�� �ش�.
            playerStatHandler.CurrentExp += MonsterStatData.totalExp;
            // ������ ��� ����?
            float dropChance = 0;
            dropChance += itemDropChance * (1 + playerStatHandler.itemFindChance/100);  // �÷��̾��� ������ �߰� Ȯ���� ����Ͽ� ��� Ȯ�� ����

            if (Random.Range(0, 100) < dropChance)
            {
                Debug.Log("Item Dropped!");
            }
        }

        base.OnDead(attackerStatHandler);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager 
{
    public Dictionary<int, Data.BaseStat> BaseStatDict { get; private set; } = new Dictionary<int, Data.BaseStat>();
    public Data.StatConstants StatConstants { get; private set; } = new Data.StatConstants();
    public Dictionary<int, Data.Monster> MonsterDict { get; private set; } = new Dictionary<int, Data.Monster>();
    public Dictionary<int, Data.Skill> PlayerSkillDict { get; private set; } = new Dictionary<int, Data.Skill>();
    public Dictionary<int, Data.Skill> MonsterSkillDict { get; private set; } = new Dictionary<int, Data.Skill>();

    public Dictionary<int, Data.Projectile> ProjectileDict { get; private set; } = new Dictionary<int, Data.Projectile>();

    public string NextSceneName;

    public void Init()
    {
        StatConstants = LoadJson<Data.StatConstants>("StatConstants");
        BaseStatDict = LoadJson<Data.BaseStatData, int, Data.BaseStat>("BaseStatData").MakeDict();
        MonsterDict = LoadJson<Data.MonsterData, int, Data.Monster>("MonsterData").MakeDict();
        PlayerSkillDict = LoadJson<Data.SkillData, int, Data.Skill>("PlayerSkillData").MakeDict();
        MonsterSkillDict = LoadJson<Data.SkillData, int, Data.Skill>("MonsterSkillData").MakeDict();

        ProjectileDict = LoadJson<Data.ProjectileData, int, Data.Projectile>("ProjectileData").MakeDict();
    }

    

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadJson<Loader>(string path)
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    //기능 많아질 시 WorldObjectManager 등으로 분리

    // int <-> GameObject
    GameObject _spawnedPlayer;
    HashSet<GameObject> _spawnedMonsters = new HashSet<GameObject>();
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    //Dictionary<int , GameObject> _monsters = new Dictionary<int, GameObject>();

    //몬스터 수를 매개변수로
    public Action<int> OnSpawnEvent;

    public GameObject GetPlayer() { return _spawnedPlayer;  }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _spawnedMonsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                _spawnedPlayer = go;
                break;
        }
        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController baseController = go.GetComponent<BaseController>();
        if (baseController == null)
            return Define.WorldObject.Unknown;

        return baseController.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject objectType = GetWorldObjectType(go);

        switch (objectType)
        {
            case Define.WorldObject.Monster:
                {
                    if (_spawnedMonsters.Contains(go))
                    {
                        _spawnedMonsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                    }
                        

                }
                break;
            case Define.WorldObject.Player:
                if (_spawnedPlayer == go)
                {
                    _spawnedPlayer = null;
                }
                break;

        }

        Managers.Resource.Destroy(go);
    }
    

}

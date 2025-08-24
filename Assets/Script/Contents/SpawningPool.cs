using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    int _currentMonsterCount = 0;
    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPoint = new Vector3(0,0,-25);

    [SerializeField]
    float _spawnRadius = 15.0f;
    [SerializeField]
    float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _currentMonsterCount += value; }
    public void SetKeepMonsterCount(int count) {  _keepMonsterCount = count; }

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount; 
        
    }

    void Update()
    {
        while (_reserveCount + _currentMonsterCount < _keepMonsterCount)
        {
            StartCoroutine("ReserveSpawn");
            //coroutine
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject spawnedMonster = Managers.Game.Spawn(Define.WorldObject.Monster, "Monsters/ChestMonster");
        NavMeshAgent nma = spawnedMonster.GetComponent<NavMeshAgent>();

        Vector3 randPos;
        while (true)
        {
            Vector3 randomSpawnOffset = Random.insideUnitSphere * Random.Range(0, _spawnRadius);   //구를 그리고 랜덤좌표 생성함.
            randomSpawnOffset.y = 0;                                              //평면상에 배치하기 위해 값을 0으로 함
            randPos = _spawnPoint + randomSpawnOffset;

            //갈 수 있는지 체크'
            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))   //갈 수 있는경우 
                break;
        }
        
        spawnedMonster.transform.position = randPos;
        _reserveCount-- ;
    }
}

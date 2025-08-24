using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    #region CoroutineTest
    //class Test
    //{
    //    public int id = 0;
    //}
    ////연산이 오래걸리거나 복잡한 함수
    ////예시 길찾기
    ////1프레임 안에 안되는 경우 일시 정지 후 다음 프레임에 시작
    ////1. 함수의 상태를 저장/복원 가능
    ////  엄청 오래걸리는 작업을 잠시 끊거나
    ////  원하는 타이밍에 함수를 잠시 Stop/복원하는 경우
    ////2. return -> 원하는 타입으로 가능 (class 도 가능)
    //class CoroutineTest : IEnumerable
    //{
    //    public System.Collections.IEnumerator GetEnumerator()
    //    {
    //        for(int i = 0; i < 1000000; i ++)
    //        {
    //            if(i % 10000 == 0)
    //                yield return null;
    //        }
    //        //yield return new Test() { id = 1 };
    //        //yield return null;
    //        ////yield break;  //아예 빠져 나옴
    //        //yield return new Test() { id = 2 };
    //        //yield return new Test() { id = 3 };
    //        //yield return new Test() { id = 4 };

    //    }

    //    void GenerateItem()
    //    {
    //        //예시
    //        //1. 아이템을 만든다
    //        //2. DB에 저장한다
    //        //1이후 2에서 저장이 되는것을 기다리지 않고 다음 로직이 실행되면 로컬에서의 아이템 상태와 DB에서의 아이템이 맞지 않게됨
    //        //아이템 복사가 일어나거나, 아이템이 이전 상태로 돌아가있거나 하는 문제 발생
    //    }
    //    //시간 같은 경우 중앙에서 공용으로 관리할 수 있는 타임 시스템 필요.
    //    //4초를 세기 위해 매번 체크하고 더하는 방식은 규모가 커지는 경우 큰 리소스 낭비가 됨.
    //    float deltaTime = 0;
    //    void ExplodeAfter4Seconds()
    //    {
    //        deltaTime += Time.deltaTime;
    //        if(deltaTime >= 4)
    //        {
    //            //로직
    //        }
    //    }
    //}
    #endregion

    Coroutine co;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.GameScene;

        //Managers.UI.ShowPopupUI<UI_Button>();

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);


        //Managers.Game.Spawn(Define.WorldObject.Monster, "Monsters/ChestMonster");
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Monsters/Beholder");
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Monsters/Slime");
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Monsters/TurtleShell");

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
    }

    //IEnumerator CoStopExplode(float seconds)
    //{
    //    Debug.Log("Defusing...");
    //    yield return new WaitForSeconds(seconds);
    //    Debug.Log("Defuse Complited!!");
    //    if(co != null)
    //    {
    //        StopCoroutine(co);
    //        co = null;
    //    }
    //}

    //IEnumerator CoExplodeAfterSeconds(float seconds)
    //{
    //    Debug.Log("Tic...Tic....");
    //    yield return new WaitForSeconds(seconds);
    //    Debug.Log("Boooooom!!!!");
    //    co = null;
    //}

    public override void Clear()
    {

    }



     
}

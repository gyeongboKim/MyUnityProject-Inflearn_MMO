using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour 
{
    static Managers s_instance; //유일성이 보장된다
    static Managers Instance { get {  Init(); return s_instance; } }  //유일한 매니저를 갖고온다. 프로퍼티 형식
                                                                      //start 가 시작 된 경우 Init이 실행되고 혹은 start가 호출되기 이전에 Instance 생성 시 Init이 실행됨.
    #region Contetns
    GameManagerEx _game = new GameManagerEx();

    public static GameManagerEx Game  { get { return Instance._game; } }
    #endregion

    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI  { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Init();
    }

    
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        //초기화
        if(s_instance == null)
        {
            GameObject managersRootObject = GameObject.Find("@Managers");
            if (managersRootObject == null)
            {
                managersRootObject = new GameObject { name = "@Managers" };
                managersRootObject.AddComponent<Managers>();
            }

            DontDestroyOnLoad(managersRootObject);
            s_instance = managersRootObject.GetComponent<Managers>();

            s_instance._data.Init();    //data의 경우 clear가 불필요
            s_instance._pool.Init();
            s_instance._sound.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear(); 

        //위의 Clear 함수들 중에서 Pool에 접근하거나 할 수 있기에 마지막에 배치
        Pool.Clear();
    }
}

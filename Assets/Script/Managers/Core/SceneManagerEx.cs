using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    //개선 필요 : FindObjectOfType 비효율적
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    
    //[옵션] 직접적인 동기 씬 로딩이 필요한 경우 사용 (LoadScene에서 LoadingScene을 통하지 않을 때)
    public void LoadSceneImmediately(Define.Scene sceneType)
    {
        Managers.Clear();

        SceneManager.LoadScene(GetSceneName(sceneType));
    }

    // 비동기 로딩을 시작하고 로딩 씬으로 이동하는 역할
    public void LoadScene(Define.Scene sceneType)
    {
        Managers.Clear();

        Managers.Data.NextSceneName = GetSceneName(sceneType);

        SceneManager.LoadScene(GetSceneName(Define.Scene.LoadingScene));
    }

    // Define.Scene string 변환
    string GetSceneName(Define.Scene sceneType)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), sceneType);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}

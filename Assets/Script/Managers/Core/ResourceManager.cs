using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);
            GameObject pooledObject = Managers.Pool.GetOriginal(name);

            if (pooledObject != null)
                return pooledObject as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        
        
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //매번 instantiate 하지 않고 poolManager에 폴링된건지 확인
        // 처음 실행인 경우 풀 생성후 pop될것임
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        //생성 후 (Clone) 문자열 제거
        GameObject instantiatedObject = Object.Instantiate(original, parent);
        instantiatedObject.name = original.name;

        return instantiatedObject;
    }

    public void Destroy(GameObject objectToDestroy)
    {
        if (objectToDestroy == null)
            return;
        // 만약 폴링 대상인경우  -> poolmanager에 위탁(Pool에 push, SetActive 가 false인 상태로 @Pool_Root의 ~~_Root의 아래에 들어감)
        Poolable poolable = objectToDestroy.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(objectToDestroy);
    }
       
}

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Util 
{
    public static T GetOrAddComponent<T> (GameObject targetGameObject) where T : UnityEngine.Component
    {
        T component = targetGameObject.GetComponent<T>();
        if (component == null)
            component = targetGameObject.AddComponent<T>();
        return component;
    }
    //T가 GameObject인 경우
    public static GameObject FindChild(GameObject parentGameObject, string name = null, bool recursive = false) 
    {
        Transform transform = FindChild<Transform>(parentGameObject, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    //T가 Component인 경우
    public static T FindChild<T>(GameObject parentGameObject, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        //부모 오브젝트가 비어있는 경우 null 리턴
        if (parentGameObject == null)
            return null;

        //직속 자식만 찾는 경우
        if(recursive == false)
        {
            for(int i = 0; i < parentGameObject.transform.childCount; i++)
            {
                Transform transform = parentGameObject.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
            
        }//재귀적으로 찾는 버전
        else
        {
            foreach (T component in parentGameObject.GetComponentsInChildren<T>())
            {
                //이름을 입력하지 않은 경우에도 T 타입을 찾으면 리턴
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
     
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);


    Texture2D attackCursor;
    Texture2D moveCursor;

    enum CursorType
    {
        None,
        Attack,
        Move,
    }

    CursorType _cursorType = CursorType.None;
    void Start()
    {
        attackCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Cursor_Attack");
        moveCursor = Managers.Resource.Load<Texture2D>("Textures/Cursor/Cursor_Move");
    }

    void Update()
    {
        //마우스를 누른 상태에서는 커서가 변하지 않도록 함
        if (Input.GetMouseButton(0))
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        //LayerMask mask = LayerMask.GetMask("Wall");

        RaycastHit hit;
        //wall
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(attackCursor, new Vector2(14.25f, 2f), CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }

            }
            else
            {
                if (_cursorType != CursorType.Move)
                {
                    Cursor.SetCursor(moveCursor, new Vector2(19.25f, 6.75f), CursorMode.Auto);
                    _cursorType = CursorType.Move;
                }

            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 5.0f, -5.0f);

    //inspector에서 적용
    [SerializeField]
    GameObject _player = null;

    public void SetPlayer(GameObject player ) {  _player = player; }

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false) 
            {
                return;
            }
            RaycastHit hit;
            //Raycast 가 벽에 부딪힌 경우 
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                //카메라를 캐릭터가 보이게 확대. 
                //dist 는 벽에 부딪힌 지점 좌표와 플레이어 좌표 사이의 벡터 * 0.8
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
                
            }
            else
            {
                //일반적인 경우   
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
            
        }
    }

    //쿼터뷰를 코드상에서 접근하고 싶을 때 사용
    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView; 
        _delta = delta;
    }
}

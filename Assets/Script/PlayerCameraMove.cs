using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ジョイスティックの動きを受け取ってプレイヤーカメラを動かす
/// </summary>
public class PlayerCameraMove : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのObject
    /// </summary>
    [SerializeField] private GameObject _player;

    /// <summary>
    /// 参照をするインプットパラメータ
    /// </summary>
    [SerializeField] private InputAction _moveAction;

    /// <summary>
    /// ジョイスティックの移動位置
    /// </summary>
    private Vector2 _joyStickPostion;

    /// <summary>
    /// 今ジョイスティックが動いてるか
    /// </summary>
    private bool _isJoyStickMove = false;

    void Start()
    {
        //有効化する
        _moveAction.Enable();
        //ジョイスチェックを動かしたとき
        _moveAction.performed += _ =>
        {
            Vector2 value = _moveAction.ReadValue<Vector2>();
            _joyStickPostion = value;
            _isJoyStickMove = true;
        };
        //ジョイスティックが止まった時
        _moveAction.canceled += _ =>
        {
            _isJoyStickMove = false;
        };
    }

    /// <summary>
    /// 常に呼ばれる
    /// </summary>
    void Update()
    {
        if(_isJoyStickMove)
		{
            //プレイヤーの角度を受け取る
            Vector3 angle = _player.transform.localEulerAngles;
            angle.x -= _joyStickPostion.y * Time.deltaTime * 50;
            angle.y += _joyStickPostion.x * Time.deltaTime * 50;
            //編集後の角度を入れる
            _player.transform.localEulerAngles = angle;
        }
    }
}

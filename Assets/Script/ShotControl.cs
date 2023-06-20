using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 射撃の制御をする
/// </summary>
public class ShotControl : MonoBehaviour
{
    /// <summary>
    /// コピー元の弾丸Object
    /// </summary>
    [SerializeField] private GameObject _shotOriginal;

    /// <summary>
    /// 射撃間隔のためのタイマー
    /// </summary>
    private float _timer = 0;

    /// <summary>
    /// 常に呼ばれる
    /// </summary>
    void Update()
    {
        _timer += Time.deltaTime;
        //0.25秒に一回動作させる
        if (_timer <= 0.25f)
		{
            return;
		}
        _timer = 0;

        //レイの作成
        Ray ray = new Ray(Camera.main.transform.localPosition, Camera.main.transform.forward);
        RaycastHit hit;
        //最大距離
        const float maxDistance = 600;
        //レイにヒットしたオブジェクトがあった場合
        if(Physics.Raycast(ray,out hit,maxDistance))
		{
            //レイにヒットしたオブジェクト
            GameObject hitObject = hit.collider.gameObject;
            //存在しないなら処理しない
            if(hitObject == null)
			{
                return;
			}
            //敵以外だったら処理しない
            if(hitObject.tag != "Enemy")
			{
                return;
			}
            //射撃の発生場所
            Vector3 position = Camera.main.transform.localPosition;
            //射撃のクローンを作成
            GameObject shotClone = (GameObject)Instantiate(_shotOriginal, position, Quaternion.identity);
            //AddForceで打ち出す
            Rigidbody shotRigidBody = shotClone.gameObject.GetComponent<Rigidbody>();
            //カメラの向きの方向へパワーを加える
            shotRigidBody.AddForce(Camera.main.transform.forward * 10000);

            //プレイヤー攻撃モーション
            PlayerBase.GetInstance().PlayerAttack();
        }

    }
}

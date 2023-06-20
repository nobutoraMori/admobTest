using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の生成管理をする
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// コピー元の敵Object
    /// </summary>
    [SerializeField] private EnemyBase _originalEnemy;

    /// <summary>
    /// 作成をするまでの時間
    /// </summary>
    private float _createTimer = 0;

    /// <summary>
    /// 現在存在している敵
    /// </summary>
    public List<EnemyBase> EnemyList;

    /// <summary>
    /// 現在存在している敵の射撃
    /// </summary>
    public List<EnemyShotBase> EnemyShotList;

    /// <summary>
    /// 最初に呼ばれる
    /// </summary>
    void Start()
    {
        //リストの初期化
        EnemyList = new List<EnemyBase>();
        EnemyShotList = new List<EnemyShotBase>();
    }

    /// <summary>
    /// 毎回呼ばれる
    /// </summary>
    void Update()
    {
        //5秒経過してないなら先に進まない
        if (_createTimer < 5.0f)
		{
            _createTimer += Time.deltaTime;
            return;
		}
        _createTimer = 0;

        //敵の複製を作る
        EnemyBase enemyBase = (EnemyBase)Instantiate(_originalEnemy,
            new Vector3(UnityEngine.Random.Range(-80.0f, 80.0f),
            UnityEngine.Random.Range(-60.0f, 80.0f),
            UnityEngine.Random.Range(20.0f, 180.0f)),
            Quaternion.identity
            );
        //こちらを向かせる
        enemyBase.gameObject.transform.LookAt(Camera.main.transform.localPosition);
        EnemyList.Add(enemyBase);
        enemyBase.OnDeadEvent = null;
        enemyBase.OnDeadEvent += (EnemyBase enemy) =>
        {
            //死亡時に呼ばれる
            EnemyList.Remove(enemy);
        };
        enemyBase.OnShotEvent = null;
        enemyBase.OnShotEvent += (EnemyShotBase enemyShot) =>
        {
            //射撃をしたら敵射撃リスト追加
            EnemyShotList.Add(enemyShot);
            //射撃が削除
            enemyShot.OnDead += (EnemyShotBase enemyshot) =>
            {
                EnemyShotList.Remove(enemyshot);
            };
        };
    }
}

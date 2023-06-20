using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー挙動管理
/// ステータス管理
/// </summary>
public class PlayerBase : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの体力
    /// </summary>
    public int LifePoint = 0;

    /// <summary>
    /// 点数
    /// </summary>
    private int _scorePoint = 0;

    /// <summary>
    /// 自身のクラスを格納
    /// </summary>
    private static PlayerBase _instance;

    /// <summary>
    /// 体力の書き換えクラス
    /// </summary>
    [SerializeField] private LifeBase _lifeBase;
    
    /// <summary>
    /// 点数の書き換えクラス
    /// </summary>
    [SerializeField] private ScoreBase _scoreBase;

    /// <summary>
    /// プレイヤーキャラのアニメ
    /// </summary>
    [SerializeField] private Animator _playerAnimaton;

    /// <summary>
    /// ゲームオーバーの動作
    /// </summary>
    [SerializeField] private GameOverBase _gameOverBase;

    /// <summary>
    /// ダメージを受けたアニメーション時間
    /// </summary>
    private float _damageTimer = 1.0f;

    /// <summary>
    /// 攻撃アニメーション時間
    /// </summary>
    private float _attackTimer = 1.0f;

    /// <summary>
    /// 最初に呼ばれる
    /// </summary>
    void Start()
    {
        //初期体力
        LifePoint = 5;
        _scorePoint = 0;
        //書き換え
        _lifeBase.SetLife(LifePoint);
        //自身のクラスを入れる
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージアニメーション中
        if(_playerAnimaton.GetBool("Damage"))
		{
            _damageTimer -= Time.deltaTime;
            if(_damageTimer <= 0)
			{
                //時間が０になったらダメージアニメーションフラグをオフにする
                _playerAnimaton.SetBool("Damage", false);
			}
        }
        //攻撃ニメーション中
        if (_playerAnimaton.GetBool("Attack"))
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                //時間が０になったら攻撃アニメーションフラグをオフにする
                _playerAnimaton.SetBool("Attack", false);
            }
        }
    }

    /// <summary>
    /// PlayerBaseクラスを受け取る
    /// </summary>
    /// <returns></returns>
    public static PlayerBase GetInstance()
	{
        return _instance;
	}

    /// <summary>
    /// プレイヤーがダメージを受けた
    /// </summary>
    public void PlayerDamage()
	{
        //体力が０ならダメージ処理をさせない
        if(LifePoint == 0)
		{
            return;
		}

        LifePoint--;
        if (LifePoint == 0)
		{
            //死亡した
            _gameOverBase.GameOverStart();
        }

        //ダメージアニメーション
        _playerAnimaton.SetBool("Damage", true);
        //アニメーション時間
        _damageTimer = 1.0f;

        _lifeBase.SetLife(LifePoint);
    }

    /// <summary>
    /// 点数の追加
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
	{
        _scorePoint += score;
        _scoreBase.SetScore(_scorePoint);
    }

    /// <summary>
    /// 攻撃時
    /// </summary>
    public void PlayerAttack()
	{
        //攻撃アニメをさせる
        _playerAnimaton.SetBool("Attack", true);
        _attackTimer = 1.0f;
    }

}

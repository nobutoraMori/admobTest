using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾丸の処理
/// </summary>
public class ShotMove : MonoBehaviour
{
    /// <summary>
    /// 射撃の生存時間
    /// </summary>
    private float _shotLifeTimer = 2.0f;
    
    //　初回で呼ばれる
    void Start()
    {
        
    }

    //毎回呼ばれる
    void Update()
    {
        //秒数を減らす
        _shotLifeTimer -= Time.deltaTime;
        //生存時間がないなら
        if (_shotLifeTimer <= 0)
		{
            //Objectが削除される
            Destroy(gameObject);
		}
    }

    /// <summary>
    /// 何かにぶつかったら呼び出される処理
    /// </summary>
    /// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //Objectが削除される
            Destroy(gameObject);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の弾丸挙動
/// </summary>
public class EnemyShotBase : MonoBehaviour
{
	/// <summary>
	/// 射撃が消えた時
	/// </summary>
	public Action<EnemyShotBase> OnDead;

	/// <summary>
	/// ぶつかった時
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		//敵以外にぶつかった時
		if (other.tag != "Enemy")
		{
			if (other.tag == "MainCamera")
			{
				//ダメージを受ける
				PlayerBase.GetInstance().PlayerDamage();
			}
			//自身を削除する
			Destroy(gameObject);
			OnDead(this);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 体力の表示
/// </summary>
public class LifeBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lifeText;

	/// <summary>
	/// 体力表示更新
	/// </summary>
	/// <param name="life"></param>
    public void SetLife(int life)
	{
		_lifeText.text = "LIFE:" + life;
	}

}

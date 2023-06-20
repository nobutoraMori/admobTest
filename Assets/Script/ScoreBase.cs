using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 点数の表示
/// </summary>
public class ScoreBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

	/// <summary>
	/// 点数表示更新
	/// </summary>
	/// <param name="score"></param>
    public void SetScore(int score)
	{
		_scoreText.text = "SCORE:" + score;
	}

}

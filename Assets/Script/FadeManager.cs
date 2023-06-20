using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// フェードインアウトの管理
/// </summary>
public class FadeManager : MonoBehaviour
{
	/// <summary>
	/// フェードインアウトをさせるオブジェクト
	/// </summary>
	[SerializeField] private Image _fadeImage;

	/// <summary>
	/// フェードアウトステータス
	/// </summary>
	private enum FADE_MODE
	{
		NONE,
		FADE_IN,
		FADE_OUT,
	}

	private FADE_MODE _fadeMode = FADE_MODE.NONE;

	/// <summary>
	/// 外部からアクセスするためstaticにする
	/// </summary>
	private static FadeManager _main;

	public static FadeManager GetInstance()
	{
		return _main;
	}

	/// <summary>
	/// Startより早く実行される
	/// </summary>
	private void Awake()
	{
		//色を初期化
		_fadeImage.color = new Color(0, 0, 0, 0);
		//オブジェクト無効化
		_fadeImage.gameObject.SetActive(false);
		_main = this;
	}

	private void Update()
	{
		switch (_fadeMode)
		{
			//だんだん暗くなる
			case FADE_MODE.FADE_OUT:
				{
					var color = _fadeImage.color;
					//透明度を濃くしていく
					color.a += Time.deltaTime;
					_fadeImage.color = color;
					if (color.a >= 1.0f)
					{
						color.a = 1.0f;
						_fadeMode = FADE_MODE.NONE;
					}
					break;
				}
			//だんだん明るくなる
			case FADE_MODE.FADE_IN:
				{
					var color = _fadeImage.color;
					//透明度を上げていく
					color.a -= Time.deltaTime;
					_fadeImage.color = color;
					if (color.a <= 0.0f)
					{
						color.a = 0.0f;
						_fadeMode = FADE_MODE.NONE;
						_fadeImage.gameObject.SetActive(false);
					}
					break;
				}
		}

	}

	/// <summary>
	/// フェードイン開始
	/// だんだん暗くなる
	/// </summary>
	public void StartFadeIn()
	{
		_fadeMode = FADE_MODE.FADE_IN;
		_fadeImage.color = new Color(0, 0, 0, 1);
		_fadeImage.gameObject.SetActive(true);
	}

	/// <summary>
	/// フェードアウト開始
	/// だんだん明るく
	/// </summary>
	public void StartFadeOut()
	{
		_fadeMode = FADE_MODE.FADE_OUT;
		_fadeImage.color = new Color(0, 0, 0, 0);
		_fadeImage.gameObject.SetActive(true);
	}
}

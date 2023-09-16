using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトルのUI制御　主にmainシーンへの遷移
/// </summary>
public class TitleUI : MonoBehaviour
{
	/// <summary>
	/// スタートボタン
	/// </summary>
	[SerializeField] private Button _gameStartButton;

	/// <summary>
	/// リワードボタン
	/// </summary>
	[SerializeField] private Button _rewardButton;

	/// <summary>
	/// GCコインの数
	/// </summary>
	[SerializeField] private TextMeshProUGUI _moneyText;

	/// <summary>
	/// プレイヤーキャラのアニメ
	/// </summary>
	[SerializeField] private Animator _playerAnimaton;

	[SerializeField] private GameObject _view;
	
	/// <summary>
	/// PlayerPrefsKey 前回Title遷移時広告を見たか
	/// PlayerPrefsではBoolで管理できないため、ここでは0をfalse, 1をtrueとしてIntで管理する。
	/// </summary>
	private const string PlayerPrefsKeyIsSeeAdLastTimeLoadTitle = "IsSeeAdLastTimeLoadTitle";

	/// <summary>
	/// PlayerPrefsKey ユーザーマネー
	/// PlayerPrefsで管理すると端末側に保存される。そのため、チートされたり、端末移行時に復帰できなくなってしまう恐れがある。
	/// 本来はアカウントに紐付けてサーバー側で管理する必要がある。
	/// </summary>
	private const string PlayerPrefsKeyUserMoney = "UserMoney";

	private bool _isInterstitial = false;
	private int _rewardValue = 0;
	void Awake()
	{
		PlayerPrefsInitialize();
		//リワードの返り
		AdmobLibrary.OnReward = (double value) =>
		{
			_rewardValue += (int)value;
		};

		//インターステイシャル
		AdmobLibrary.OnLoadedInterstitial = () =>
		{
			_isInterstitial = true;
		};
		
		AdmobLibrary.FirstSetting();

		//Admobバナー作成
		AdmobLibrary.RequestBanner(GoogleMobileAds.Api.AdSize.IABBanner, GoogleMobileAds.Api.AdPosition.Bottom);
		_playerAnimaton.SetBool("Opening", true);

		//ボタンを押したとき
		_gameStartButton.onClick.AddListener(OnGameStartButton);
		_rewardButton.onClick.AddListener(OnRewardButton);

		UpdateMoney();
		
		//リワード読み込み
		AdmobLibrary.LoadReward();
		//UMPを入れたい場合はコメントを外す
		//AdmobUMP.FirstSetting();
	}

	private void Update()
	{
		//リワードボタンの表示切替
		//リワードが読み込めないなら表示しない
		_rewardButton.gameObject.SetActive(AdmobLibrary.IsActiveReward());
		
		if (_isInterstitial)
		{
			//UMP開始
			// 前回起動時広告を見ていないなら広告を出す
			if (PlayerPrefs.GetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle) > 0)
			{
				// 広告を見ていたら0代入
				PlayerPrefs.SetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle, 0);
				AdmobLibrary.PlayInterstitial();
			}
			else
			{
				// 広告を見ていなかったら1代入
				PlayerPrefs.SetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle, 1);
			}

			_isInterstitial = false;
		}

		if (_rewardValue >= 1)
		{
			int money = PlayerPrefs.GetInt(PlayerPrefsKeyUserMoney);
			//報酬を加算する
			money += _rewardValue;
			PlayerPrefs.SetInt(PlayerPrefsKeyUserMoney, money);
			UpdateMoney();
			_rewardValue = 0;
		}
	}

	/// <summary>
	/// PlayerPrefs(内部保存データ)の初期化
	/// </summary>
	private void PlayerPrefsInitialize()
	{
		// PlayerPrefsにキーが設定されていない場合、初期化する
		if (!PlayerPrefs.HasKey(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle))
		{
			PlayerPrefs.SetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle, 0);
		}
		if (!PlayerPrefs.HasKey(PlayerPrefsKeyUserMoney))
		{
			PlayerPrefs.SetInt(PlayerPrefsKeyUserMoney, 0);
		}
	}

	/// <summary>
	/// GCマネーの表示更新
	/// </summary>
	private void UpdateMoney()
	{
		_moneyText.text = PlayerPrefs.GetInt(PlayerPrefsKeyUserMoney).ToString();
	}

	/// <summary>
	/// ゲームスタートボタンのコールバック
	/// </summary>
	private void OnGameStartButton()
	{
		//バナー削除
		AdmobLibrary.DestroyBanner();

		//インタースティシャル削除
		AdmobLibrary.DestroyInterstitial();

		//リワード削除
		AdmobLibrary.DestroyReward();

		//mainシーンに遷移
		SceneManager.LoadScene("main");
	}

	/// <summary>
	/// 報酬ボタンのコールバック
	/// </summary>
	private void OnRewardButton()
	{
		AdmobLibrary.ShowReward();
	}
}

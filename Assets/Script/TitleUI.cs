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

	void Start()
	{
		PlayerPrefsInitialize();

		AdmobLibrary.FirstSetting();

		//Admobバナー作成
		AdmobLibrary.RequestBanner(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Bottom);
		_playerAnimaton.SetBool("Opening", true);

		//ボタンを押したとき
		//ToDo : 個人的にイベント登録は、中に処理を書くよりはコールバックにした方が見やすいかなと感じました。
		_gameStartButton.onClick.AddListener(OnGameStartButton);
		_rewardButton.onClick.AddListener(OnRewardButton);

		AdShow();

		//リワードの返り
		AdmobLibrary.OnReward += (double value) =>
		{
			int money = PlayerPrefs.GetInt(PlayerPrefsKeyUserMoney);
			//報酬を加算する
			money += (int)value;
			PlayerPrefs.SetInt(PlayerPrefsKeyUserMoney, money);
			UpdateMoney();
		};

		UpdateMoney();
	}

	/// <summary>
	/// GCマネーの表示更新
	/// </summary>
	private void UpdateMoney()
	{
		_moneyText.text = PlayerPrefs.GetInt(PlayerPrefsKeyUserMoney).ToString();
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
	/// 広告の表示処理
	/// </summary>
	private void AdShow()
	{
		// 前回起動時広告を見ていないなら広告を出す
		if (PlayerPrefs.GetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle) > 0)
		{
			AdmobLibrary.PlayInterstitial();

			// 広告を見ていたら0代入
			PlayerPrefs.SetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle, 0);
		}
		else
		{
			// 広告を見ていなかったら1代入
			PlayerPrefs.SetInt(PlayerPrefsKeyIsSeeAdLastTimeLoadTitle, 1);
		}
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

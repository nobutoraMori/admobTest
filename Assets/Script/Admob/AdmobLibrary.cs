using GoogleMobileAds.Api;
using System;
using UnityEngine;

/// <summary>
/// adMobを使用するためのクラス
/// </summary>
public class AdmobLibrary
{
	private static BannerView _bannerView;
	private static InterstitialAd _interstitialAd;
	private static RewardedAd _rewardedAd;

	public static Action<double> OnReward;

	public static Action OnLoadedInterstitial;

	/// <summary>
	/// ゲーム起動　初回に一度だけ呼ぶ
	/// </summary>
	public static void FirstSetting()
	{
		//13歳以下を対象と「する」場合はtrue
		RequestConfiguration request = new RequestConfiguration
		{
			TagForChildDirectedTreatment = TagForChildDirectedTreatment.False
		};


		MobileAds.SetRequestConfiguration(request);

		MobileAds.Initialize((InitializationStatus initStatus) =>
		{
			// This callback is called once the MobileAds SDK is initialized.
			InitInterstitial();

		});
	}


	/// <summary>
	/// バナー広告を生成
	/// </summary>
	/// <param name="size"></param>
	/// <param name="position"></param>
	public static void RequestBanner(AdSize size, AdPosition position, bool collapsible)
	{
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
		string adUnitId = "unexpected_platform";
#endif

		AdSize adaptiveSize =
					AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

		_bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);

		//セーフエリアを考慮
		// var area = Screen.safeArea;
		// _bannerView = new BannerView(adUnitId, size, Screen.width/4 ,Screen.height /10);

		// Create an empty ad request.

		var adRequest = new AdRequest();

		if (collapsible)
		{
			//折り畳みバナー設定
			adRequest.Extras.Add("collapsible", "bottom");
		}

		// Load the banner with the request.
		_bannerView.LoadAd(adRequest);
		Debug.Log($"ロード完了、アダプティブバナーサイズ: {_bannerView.GetHeightInPixels()} {_bannerView.GetWidthInPixels()}");
	}

	/// <summary>
	/// バナー広告削除
	/// </summary>
	public static void DestroyBanner()
	{
		if (_bannerView != null)
		{
			_bannerView.Destroy();
		}
	}

	/// <summary>
	/// インタースティシャル読み込み
	/// </summary>
	private static void InitInterstitial()
	{
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/3419835294";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
		string adUnitId = "unexpected_platform";
#endif
		// Initialize an InterstitialAd.

		var adRequest = new AdRequest();
		if (_interstitialAd != null)
		{
			_interstitialAd.Destroy();
			_interstitialAd = null;
		}

		Debug.Log("InitInterstitial");
		// send the request to load the ad.
		InterstitialAd.Load(adUnitId, adRequest,
			(InterstitialAd ad, LoadAdError error) =>
			{
				// if error is not null, the load request failed.
				if (error != null || ad == null)
				{
					Debug.LogError("interstitial ad failed to load an ad " +
					               "with error : " + error);
					return;
				}

				Debug.Log("Interstitial ad loaded with response : "
				          + ad.GetResponseInfo());

				// Raised when the ad is estimated to have earned money.
				ad.OnAdPaid += (AdValue adValue) =>
				{
					Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
						adValue.Value,
						adValue.CurrencyCode));
				};
				// Raised when an impression is recorded for an ad.
				ad.OnAdImpressionRecorded += () => { Debug.Log("Interstitial ad recorded an impression."); };
				// Raised when a click is recorded for an ad.
				ad.OnAdClicked += () => { Debug.Log("Interstitial ad was clicked."); };
				// Raised when an ad opened full screen content.
				ad.OnAdFullScreenContentOpened += () => { Debug.Log("Interstitial ad full screen content opened."); };
				// Raised when the ad closed full screen content.
				ad.OnAdFullScreenContentClosed += () => { Debug.Log("Interstitial ad full screen content closed."); };
				// Raised when the ad failed to open full screen content.
				ad.OnAdFullScreenContentFailed += (AdError error) =>
				{
					Debug.LogError("Interstitial ad failed to open full screen content " +
					               "with error : " + error);
				};
				_interstitialAd = ad;
				OnLoadedInterstitial?.Invoke();
			});
	}

	/// <summary>
	/// インタースティシャルを出す
	/// </summary>
	public static void PlayInterstitial()
	{
		Debug.Log("PlayInterstitial");
		if (_interstitialAd != null && _interstitialAd.CanShowAd())
		{
			Debug.Log("Showing interstitial ad.");
			_interstitialAd.Show();
		}
		else
		{
			Debug.LogError("Interstitial ad is not ready yet.");
		}
	}

	/// <summary>
	/// インタースティシャル削除
	/// </summary>
	public static void DestroyInterstitial()
	{
		if (_interstitialAd != null)
		{
			Debug.Log("DestroyInterstitial");
			_interstitialAd.Destroy();
		}
	}

	/// <summary>
	/// リワード広告
	/// </summary>
	public static void LoadReward()
	{
		string adUnitId;
#if UNITY_ANDROID
		adUnitId = "ca-app-pub-3940256099942544/1712485313";
#elif UNITY_IPHONE
		adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
		adUnitId = "unexpected_platform";
#endif
		var adRequest = new AdRequest();
		_rewardedAd = null;
		RewardedAd.Load(adUnitId, adRequest,
			(RewardedAd ad, LoadAdError error) =>
			{
				// if error is not null, the load request failed.
				if (error != null || ad == null)
				{
					Debug.LogError("rewarded ad failed to load an ad " +
					               "with error : " + error);
					return;
				}

				Debug.Log("Rewarded ad loaded with response : "
				          + ad.GetResponseInfo());
				// Raised when the ad is estimated to have earned money.
				ad.OnAdPaid += (AdValue adValue) =>
				{
					Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
						adValue.Value,
						adValue.CurrencyCode));
				};
				// Raised when an impression is recorded for an ad.
				ad.OnAdImpressionRecorded += () => { Debug.Log("Rewarded ad recorded an impression."); };
				// Raised when a click is recorded for an ad.
				ad.OnAdClicked += () => { Debug.Log("Rewarded ad was clicked."); };
				// Raised when an ad opened full screen content.
				ad.OnAdFullScreenContentOpened += () => { Debug.Log("Rewarded ad full screen content opened."); };
				// Raised when the ad closed full screen content.
				ad.OnAdFullScreenContentClosed += () => { Debug.Log("Rewarded ad full screen content closed."); };
				// Raised when the ad failed to open full screen content.
				ad.OnAdFullScreenContentFailed += (AdError error) =>
				{
					Debug.LogError("Rewarded ad failed to open full screen content " +
					               "with error : " + error);
				};
				_rewardedAd = ad;
			});
	}

	/// <summary>
	/// リワード広告を作成
	/// </summary>
	public static void ShowReward()
	{
		if (_rewardedAd != null && _rewardedAd.CanShowAd())
		{
			_rewardedAd.Show((Reward reward) =>
			{
				// TODO: Reward the user.
				Debug.Log(String.Format("Reward ", reward.Type, reward.Amount));
				OnReward?.Invoke(reward.Amount);
				_rewardedAd.Destroy();
				LoadReward();
			});
		}
	}

	/// <summary>
	/// リワード削除
	/// </summary>
	public static void DestroyReward()
	{
		if (_rewardedAd != null)
		{
			_rewardedAd.Destroy();
		}
	}

	/// <summary>
	/// リワード
	/// </summary>
	/// <returns></returns>
	public static bool IsActiveReward()
	{
		return _rewardedAd != null;
	}
}

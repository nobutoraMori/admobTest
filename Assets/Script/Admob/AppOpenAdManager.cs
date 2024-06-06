using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AppOpenAdManager
{
#if UNITY_ANDROID
    private const string _adUnitID = "ca-app-pub-3940256099942544/9257395921";
#elif UNITY_IOS
    private const string _adUnitID = "ca-app-pub-3940256099942544/9257395921";
#else
    private const string _adUnitID = "unexpected_platform";
#endif

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private bool isShowingAd = false;

    // COMPLETE: Add loadTime field
    private DateTime loadTime;

    public Action OnLoaded;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable
    {
        get
        {
            // COMPLETE: Consider ad expiration
            return ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
        }
    }

    public void LoadAd()
    {
        var adRequest = new AdRequest();

        // Load an app open ad for portrait orientation      
        AppOpenAd.Load(_adUnitID, adRequest, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.GetMessage());
                return;
            }

            // App open ad is loaded
            ad = appOpenAd;
            Debug.Log("App open ad loaded");

            // COMPLETE: Keep track of time when the ad is loaded.
            loadTime = DateTime.UtcNow;

            if(OnLoaded != null)
			{
                OnLoaded();
            }
        }));
    }

    public void Show()
    {
        if (!IsAdAvailable || isShowingAd)
        {
            return;
        }

        ad.OnAdFullScreenContentOpened += HandleAdDidDismissFullScreenContent;
        ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdImpressionRecorded += HandleAdDidRecordImpression;

        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent()
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError error)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", error.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
    }

    private void HandleAdDidRecordImpression()
    {
        Debug.Log("Recorded ad impression");
    }
}

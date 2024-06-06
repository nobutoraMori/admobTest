using System;
using GoogleMobileAds.Api;
using UnityEngine;


[AddComponentMenu("GoogleMobileAds/Samples/NativeOverlayAdController")]
public class AdmobNative
{
#if UNITY_ANDROID
    private const string _adUnitID = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IOS                       
    private const string _adUnitID = "ca-app-pub-3940256099942544/3986624511";
                                      
#else
    private const string _adUnitID = "unexpected_platform";
#endif

    public static NativeOverlayAd NativeAd { get; private set; }
    private static AdmobNative instance;

    /// <summary>
    /// Define our native ad advanced options.
    /// </summary>
    public static NativeAdOptions  Option = new NativeAdOptions
    {
        AdChoicesPlacement = AdChoicesPlacement.TopRightCorner,
        MediaAspectRatio = MediaAspectRatio.Any,
    };

    public static AdmobNative Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AdmobNative();
            }

            return instance;
        }
    }

    public static void LoadAd()
    {
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        NativeOverlayAd.Load(_adUnitID, adRequest, Option,
            (NativeOverlayAd ad, LoadAdError error) =>
            {
                // If the operation failed with a reason.
                if (error != null)
                {
                    Debug.LogError("Native Overlay ad failed to load an ad with error : " + error);
                    return;
                }
                // If the operation failed for unknown reasons.
                // This is an unexpected error, please report this bug if it happens.
                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Native Overlay ad load event fired with " +
                    " null ad and null error.");
                    return;
                }

                // The operation completed successfully.
                Debug.Log("Native Overlay ad loaded with response : " + ad.GetResponseInfo());
                NativeAd = ad;

                // Register to ad events to extend functionality.
                RegisterEventHandlers(ad);

                // Inform the UI that the ad is ready.
               // AdLoadedStatus?.SetActive(true);
            });

    }


    private static void RegisterEventHandlers(NativeOverlayAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Native Overlay ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Native Overlay ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Native Overlay ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Native Overlay ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Native Overlay ad full screen content closed.");
        };
    }



}

using System;
using System.Collections.Generic;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

public class AdmobUMP 
{
    private static ConsentForm _consentForm;
    public static Action OnShowStart;
    public static Action OnShowEnd;

    public static void FirstSetting()
    {
        //デバック用　これを外すと一回のみの表示となる==================
        //※※※これ以降は本番ではからなず外す！！！！
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.Log("AdmobUMP TestSetting");
        ConsentInformation.Reset();
        //デバッグ用デバイスで、地域が EEA として表示されます。
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            // この値を設定してない状態で
            // 実機起動してログを見るとHashIDが表示されるのでそれを記入
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                //実機ログでHashedIDを確認して入れる
                "7F6A181E291975C983AE4D7FDE0D114B"
            }
        };
#endif
        //======================================================
        Debug.Log("AdmobUMP FirstSetting");

        // Set tag for under age of consent.
        // Here false means users are not under age.
        ConsentRequestParameters consentRequestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false
            //本番の場合は外す
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            ,
            ConsentDebugSettings = debugSettings,
#endif
        };

        // Check the current consent information status.
        ConsentInformation.Update(consentRequestParameters, OnConsentInfoUpdated);
    }

    private static void OnConsentInfoUpdated(FormError error)
    {
        Debug.Log("AdmobUMP OnConsentInfoUpdated");
        if (error != null)
        {
            
            UnityEngine.Debug.LogError("AdmobUMP ErrorCode"+error.ErrorCode);
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadConsentForm();
        }

    }
	
    private static void LoadForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }

    private static void LoadConsentForm()
    {
        Debug.Log("AdmobUMP LoadConsentForm");
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }

    private static void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        Debug.Log("AdmobUMP OnLoadConsentForm");
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;

        // You are now ready to show the form.
        if(ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            OnShowStart?.Invoke();
            _consentForm.Show(OnShowForm);
        }
    }
    
    private static void OnShowForm(FormError error)
    {
        Debug.Log("AdmobUMP OnShowForm");
        if (error != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(error);
            return;
        }

        // Handle dismissal by reloading form.
        LoadForm();
        OnShowEnd?.Invoke();
    }
}

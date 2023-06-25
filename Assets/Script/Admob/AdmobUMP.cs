using System.Collections.Generic;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

public class AdmobUMP 
{
    private static ConsentForm _consentForm;

    public static void FirstSetting()
    {
        ConsentInformation.Reset();

        //デバッグ用デバイスで、地域が EEA として表示されます。
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                "7F6A181E291975C983AE4D7FDE0D114B"
            }
        };
        
        Debug.Log("AdmobUMP FirstSetting");
        
        // Set tag for under age of consent.
        // Here false means users are not under age.
        ConsentRequestParameters consentRequestParameters = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };

        // Check the current consent information status.
        ConsentInformation.Update(consentRequestParameters, OnConsentInfoUpdated);
    }
    
    private static void OnConsentInfoUpdated(FormError error)
    {
        Debug.Log("AdmobUMP OnConsentInfoUpdated");
        if (error != null)
        {
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
    }
}

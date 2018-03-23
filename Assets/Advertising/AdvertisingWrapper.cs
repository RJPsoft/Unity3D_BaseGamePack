using GoogleMobileAds.Api;
using System;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Wraps Vungle funcionality
/// </summary>
/// <remarks>
/// https://support.vungle.com/hc/en-us/articles/115000468731-Get-Started-with-Vungle-SDK-v-5-1-and-higher-Unity
/// https://github.com/unity-plugins/Unity-Admob/wiki/How-to-Use-Admob-Plugin-for-Unity
/// </remarks>
public static partial class AdvertisingWrapper
{
    static AdvertisingWrapper()
    {
        Vungle.onLogEvent += OnLogEvent;
        Vungle.adPlayableEvent += OnAdPlayableEvent;
        Vungle.onAdFinishedEvent += OnAdFinishedEvent;
        Vungle.onAdStartedEvent += OnAdStartedEvent;
        Vungle.onInitializeEvent += OnInitializeEvent;
        OnLogEvent("AdvertisingWrapper Instantiated");
    }

    /// <summary>
    /// Initializes the specified on vungle initialized.
    /// </summary>
    /// <param name="onVungleInitialized">Action that will be invoked when Vungles finishes initialization.</param>
    public static void Init(Action onVungleInitialized)
    {
        InitVungle(onVungleInitialized);
        InitAdmob();
    }

    public static bool IsRewadedAdPlayable()
    {
        var adPlayable = false;
#if UNITY_IPHONE || UNITY_ANDROID
        adPlayable = AdmobIsRewardedAdLoded();
#else
        adPlayable = IsAdPlayable(VunglePlacementsIds.DEFAULT);
#endif
        return adPlayable;
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    static void InitAdmob()
    {
        MobileAds.Initialize(AdMobConfigurations.APP_ID);
        _request = new AdRequest.Builder().Build();
        _bannerView = new BannerView(AdMobConfigurations.BANNER_ID, AdSize.SmartBanner, AdPosition.Bottom);
        _bannerView.LoadAd(_request);
        _interstitial = new InterstitialAd(AdMobConfigurations.INERSTITIAL_ID);
    }

    [Conditional("UNITY_WSA_10_0")]
    [Conditional("UNITY_WINRT_8_1")]
    [Conditional("UNITY_METRO")]
    static void InitVungle(Action onVungleInitialized)
    {
        if (!IsVungleInitialized)
        {
            Vungle.init(VungleConfigurations.APP_ID, VungleConfigurations.PLACEMENTS);
            _onVungleInitialized = onVungleInitialized;
            if (Application.isEditor)
            {
                OnInitializeEvent();
            }
        }
    }
}
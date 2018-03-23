using GoogleMobileAds.Api;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Wraps Vungle funcionality
/// </summary>
/// <remarks>
/// https://support.vungle.com/hc/en-us/articles/115000468731-Get-Started-with-Vungle-SDK-v-5-1-and-higher-Unity
/// https://github.com/unity-plugins/Unity-Admob/wiki/How-to-Use-Admob-Plugin-for-Unity
/// </remarks>
public static partial class AdvertisingWrapper
{
    /// <summary>
    /// Action Invoked when the inertitial ad closes or fail to load
    /// </summary>
    static UnityAction _onInerstitialClosedOrFailed;
    static BannerView _bannerView;
    static AdRequest _request;
    static InterstitialAd _interstitial;
    static Action<bool> _onRewardedAdLoadedOrFailed;
    static Action<bool> _onRewardedAdFinished;

    static bool NotEditor
    {
        get
        {
            return !Application.isEditor;
        }
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobShowDefaultBanner()
    {
        ExetueIfNoInEditor(_bannerView.Show);
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobRemoveBanner()
    {
        ExetueIfNoInEditor(_bannerView.Hide);
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobDestroyBanner()
    {
        ExetueIfNoInEditor(_bannerView.Destroy);
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobLoadInerstitial()
    {
        ExetueIfNoInEditor(() =>
        {
            if (!_interstitial.IsLoaded())
            {
                LogManager.Log("******* Loading AdMob Inerstitial **********");
                _interstitial.OnAdLoaded += OnAdMobInertitialLoaded;
                _interstitial.LoadAd(_request);
            }
        });
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobLoadRewarded(Action<bool> onRewardedAdLoadedOrFailed)
    {
        ExetueIfNoInEditor(() =>
        {
            if (!RewardBasedVideoAd.Instance.IsLoaded())
            {
                LogManager.Log("******* Loading AdMob Rewarded **********");
                RewardBasedVideoAd.Instance.LoadAd(_request, AdMobConfigurations.REWARDED_ID);
                _onRewardedAdLoadedOrFailed = onRewardedAdLoadedOrFailed;
                RewardBasedVideoAd.Instance.OnAdLoaded += OnRewardeAdLoaded;
                RewardBasedVideoAd.Instance.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
            }
        });
    }

    [Conditional("UNITY_IPHONE")]
    [Conditional("UNITY_ANDROID")]
    public static void AdMobPlayRewarded(Action<bool> onRewardedAdFinished)
    {
        if (Application.isEditor)
        {
            ShowAdMobMockModalDialog(onRewardedAdFinished);
        }
        else
        {
            _onRewardedAdFinished = onRewardedAdFinished;
            if (RewardBasedVideoAd.Instance.IsLoaded())
            {
                RewardBasedVideoAd.Instance.OnAdClosed += OnRewardedAdClosed;
                RewardBasedVideoAd.Instance.OnAdRewarded += OnRewardedAdRewarded;
                RewardBasedVideoAd.Instance.Show();
                LogManager.Log("******* AdMob Rewarded Displayed **********");
            }
            else
            {
                LogManager.Log(string.Format("******* AdMob Rewarded Not Loaded, invoking  {0}**********", "onRewardedAdFinished"));
                InvokeRewardedAdFinished(true);
            }
        }
    }

    public static bool AdMobShowInerstitial(UnityAction onInerstitialClosedOrFailed)
    {
        var loaded = false;
#if UNITY_IPHONE || UNITY_ANDROID
        ExetueIfNoInEditor(() =>
        {
            if (_interstitial != null && _interstitial.IsLoaded())
            {
                _onInerstitialClosedOrFailed = onInerstitialClosedOrFailed;
                loaded = true;
                _interstitial.OnAdClosed += OnAdClosed;
                _interstitial.OnAdFailedToLoad += OnAdClosed;
                _interstitial.Show();
                LogManager.Log("******* AdMob Inerstitial Displayed **********");
            }
        });
#endif
        return loaded;
    }

    static bool AdmobIsRewardedAdLoded()
    {
        return Application.isEditor ? true : RewardBasedVideoAd.Instance.IsLoaded();
    }

    static void ShowAdMobMockModalDialog(Action<bool> onAdFinishedCallback)
    {
        MockUiCreator.CreateUI(
            () => onAdFinishedCallback.Invoke(true),
            () => onAdFinishedCallback.Invoke(false)
        );
    }

    private static void OnRewardedAdRewarded(object sender, Reward e)
    {
        InvokeRewardedAdFinished(true);
    }

    private static void OnRewardedAdClosed(object sender, EventArgs e)
    {
        InvokeRewardedAdFinished(false);
    }

    private static void InvokeRewardedAdFinished(bool fullView)
    {
        AdMobLoadRewarded(null);
        if (_onRewardedAdFinished != null)
        {
            _onRewardedAdFinished.Invoke(fullView); 
        }
        _onRewardedAdFinished = null;
    }

    private static void OnRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        LogManager.Log("******* AdMob Rewarded Failed To Load**********");
        if (_onRewardedAdLoadedOrFailed != null)
        {
            _onRewardedAdLoadedOrFailed.Invoke(false); 
        }
        _onRewardedAdLoadedOrFailed = null;
    }

    private static void OnRewardeAdLoaded(object sender, EventArgs e)
    {
        LogManager.Log("******* AdMob Rewarded Loaded**********");
        if (_onRewardedAdLoadedOrFailed != null)
        {
            _onRewardedAdLoadedOrFailed.Invoke(true); 
        }
        _onRewardedAdLoadedOrFailed = null;
    }

    static void OnAdClosed(object sender, EventArgs e)
    {
        _interstitial.OnAdClosed -= OnAdClosed;
        _interstitial.OnAdFailedToLoad -= OnAdClosed;
        AdMobLoadInerstitial();
        if (_onInerstitialClosedOrFailed != null)
        {
            _onInerstitialClosedOrFailed.Invoke(); 
        }
        _onInerstitialClosedOrFailed = null;
        LogManager.Log("******* AdMob Inerstitial Closed **********");
    }

    static void ExetueIfNoInEditor(Action actionToExecute)
    {
        if (NotEditor && actionToExecute != null)
        {
            actionToExecute.Invoke();
        }
    }

    static void OnAdMobInertitialLoaded(object sender, EventArgs e)
    {
        _interstitial.OnAdLoaded -= OnAdMobInertitialLoaded;
        LogManager.Log("******* AdMob Inerstitial Loaded **********");
    }
}

static class AdMobConfigurations
{
    internal static readonly string APP_ID;
    //test id
    internal static readonly string BANNER_ID = "ca-app-pub-3940256099942544/6300978111";
    //test id
    internal static readonly string INERSTITIAL_ID = "ca-app-pub-3940256099942544/1033173712";
    //test di
    internal static readonly string REWARDED_ID = "ca-app-pub-3940256099942544/5224354917";

    static AdMobConfigurations()
    {
#if UNITY_IPHONE
        APP_ID = "dfgdhfghfdgh";
        BANNER_ID = "yurtyutryu";
        INERSTITIAL_ID = "rtyutryu";
#elif UNITY_ANDROID
        APP_ID = "dfhfdghdfhg";

        if (!UnityEngine.Debug.isDebugBuild)
        {
            BANNER_ID = "fghfjgj";
            INERSTITIAL_ID = "fdgfgjgh";
            REWARDED_ID = "fghfgjf";
        }
#endif
    }
}
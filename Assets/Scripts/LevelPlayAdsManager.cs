using System;
using UnityEngine;
using TMPro;
using Unity.Services.LevelPlay;

public class LevelPlayAdsManager : MonoBehaviour
{
    [Header("App Keys")] [Space(2)] [SerializeField]
    private string androidAppKey;
    [SerializeField] private string iOSAppKey;

    [Header("Banner Ad Unit ID")] [Space(2)] [SerializeField]
    private string androidBannerAdUnitId;
    [SerializeField] private string iOSBannerAdUnitId;

    [Header("Interstitial Ad Unit ID")] [Space(2)] [SerializeField]
    private string androidInterstitialAdUnitId;
    [SerializeField] private string iOSInterstitialAdUnitId;

    [Header("Rewarded Ad Unit ID")] [Space(2)] [SerializeField]
    private string androidRewardedAdUnitId;
    [SerializeField] private string iOSRewardedAdUnitId;
    
    [Header("Coin Text UI")]
    [SerializeField] private TextMeshProUGUI CoinText;

    private LevelPlayBannerAd _bannerAd;
    private LevelPlayInterstitialAd _interstitialAd;
    private LevelPlayRewardedAd _rewardedAd;
    
    public string appKey
    {
        get
        {
#if UNITY_ANDROID
            return androidAppKey;
#elif UNITY_IOS
            return iOSAppKey;
#else
            return string.Empty;
#endif
        }
    }

    public string bannerAdUnityKey
    {
        get
        {
#if UNITY_ANDROID
            return androidBannerAdUnitId;
#elif UNITY_IOS
            return iOSBannerAdUnitId;
#else
            return string.Empty;
#endif
        }
    }

    public string interstitialAdUnityKey
    {
        get
        {
#if UNITY_ANDROID
            return androidInterstitialAdUnitId;
#elif UNITY_IOS
            return iOSInterstitialAdUnitId;
#else
            return string.Empty;
#endif
        }
    }

    public string rewardedAdUnityKey
    {
        get
        {
#if UNITY_ANDROID
            return androidRewardedAdUnitId;
#elif UNITY_IOS
            return iOSRewardedAdUnitId;
#else
            return string.Empty;
#endif
        }
    }

    public int Coins
    {
        get=>PlayerPrefs.GetInt("PlayerCoins",0);
        set
        {
            PlayerPrefs.SetInt("PlayerCoins",value);
            PlayerPrefs.Save();

            UpdateCoinUI();
        }
    }

    private void UpdateCoinUI()
    {
        CoinText.text = Coins.ToString();
    }

    public void Start()
    {
        LevelPlay.ValidateIntegration();
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        
        LevelPlay.SetMetaData("is_test_suite", "enable"); 
        LevelPlay.Init(appKey);
        UpdateCoinUI();
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError obj)
    {
        Debug.LogError("SDK Initialization Failed");
        Debug.LogError(obj.ToString());
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration obj)
    {
        Debug.LogFormat("SDK Initialization Completed",Color.green);
        CreateBannerAd();
        CreateInterstitialAd();
        CreateRewardedAd();
    }

    public void LaunchTestSuite()
    {
        LevelPlay.LaunchTestSuite();
    }

    #region Banner Ads~

    private void CreateBannerAd()
    {
        var adConfig = new LevelPlayBannerAd.Config.Builder()
            .SetPosition(LevelPlayBannerPosition.BottomCenter).Build();

        _bannerAd = new LevelPlayBannerAd(bannerAdUnityKey, adConfig);
        
        
        _bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        _bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        _bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        _bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        _bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        _bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        _bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        _bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
    }

    public void ShowBannerAd()
    {
        _bannerAd.LoadAd();
    }

    public void HideBannerAd()
    {
        _bannerAd.HideAd();
    }

    public void DestroyBannerAd()
    {
        _bannerAd.DestroyAd();
    }


    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Loaded");
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError)
    {
        Debug.Log("Banner Ad Load Failed");
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Clicked");
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Displayed");
    }

    void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.Log("Banner Ad Display Failed");
    }

    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Collapsed");
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Left Application");
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Banner Ad Expanded");
    }

    #endregion
    
    #region Interstitial Ads~

    private void CreateInterstitialAd()
    {
        _interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnityKey);
        
        _interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        _interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        _interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        _interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        _interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        _interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        _interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }

    public void LoadInterstitialAd()
    {
        _interstitialAd.LoadAd();
        Debug.Log("Interstitial Ad Loaded");
    }
    
    public void ShowInterstitialAd()
    {
        if (_interstitialAd.IsAdReady())
        {
            _interstitialAd.ShowAd();
            Debug.Log("Interstitial Ad Show");
        }
    }

    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log("Interstitial Ad Load Failed");
    }

    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Interstitial Ad Displayed");
    }

    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        Debug.Log("Interstitial Ad Display Failed");
    }

    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Interstitial Ad Clicked");
    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Interstitial Ad Closed");
    }

    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Interstitial Ad Info Changed");
    }

    #endregion

    #region Rewarded Ads~

    private void CreateRewardedAd()
    {
        _rewardedAd = new LevelPlayRewardedAd(rewardedAdUnityKey);
        
        
        // Register to Rewarded events
        _rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        _rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        _rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        _rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        _rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        _rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
// Optional
        _rewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
        _rewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;
    }
    
    public void LoadRewardedAd()
    {
        _rewardedAd.LoadAd();
        Debug.Log("Rewarded Ad Loaded");
    }

    public void ShowRewardedAd()
    {
        if (_rewardedAd.IsAdReady())
        {
            _rewardedAd.ShowAd();
            Debug.Log("Rewarded Ad Loaded");
        }
    }

    // Implement the events
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) {}
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error) {}
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) {}
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) {}

    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
    {
        string rewardName = adReward.Name;
        int rewardValue = adReward.Amount;
        Coins+=rewardValue;
        Debug.Log($"Rewarded Ad Reward Name: {rewardName} and Amount: {rewardValue} ");
    }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) {}
    void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) {}
    void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) {}

    #endregion
}
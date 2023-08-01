using Services;
using UnityEngine;

namespace ServicesImpls.DummyAds
{
    public class DummyAdvertisement : IAdvertisement
    {
        public float LastAdRequestTime { get; private set; }
        public bool IsAdShowing => false;
        
        public event InterstitialAdOpenDelegate OnInterstitialAdOpen;
        public event InterstitialAdvCloseDelegate OnInterstitialAdClose;
        public event RewardedAdOpenDelegate OnRewardedAdOpen;
        public event RewardedAdCloseDelegate OnRewardedAdClose;

        public void ShowInterstitialAd()
        {
            LastAdRequestTime = Time.time;
            OnInterstitialAdOpen?.Invoke();
            OnInterstitialAdClose?.Invoke();
        }

        public void ShowRewardedAd()
        {
            LastAdRequestTime = Time.time;
            OnRewardedAdOpen?.Invoke();
            OnRewardedAdClose?.Invoke(true);
        }
    }
}
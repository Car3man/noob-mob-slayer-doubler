namespace Services
{
    public interface IAdvertisement
    {
        public event InterstitialAdOpenDelegate OnInterstitialAdOpen;
        public event InterstitialAdvCloseDelegate OnInterstitialAdClose;
        public event RewardedAdOpenDelegate OnRewardedAdOpen;
        public event RewardedAdCloseDelegate OnRewardedAdClose;
        float LastAdRequestTime { get; }
        bool IsAdShowing { get; }
        void ShowInterstitialAd();
        void ShowRewardedAd();
    }
    
    public delegate void InterstitialAdOpenDelegate();
    public delegate void InterstitialAdvCloseDelegate();
    public delegate void RewardedAdOpenDelegate();
    public delegate void RewardedAdCloseDelegate(bool rewarded);
}
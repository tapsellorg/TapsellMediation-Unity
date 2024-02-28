using System;
using JetBrains.Annotations;

namespace Tapsell.Mediation.Show
{
    public interface IAdStateListener
    {
        public void OnAdImpression();
        public void OnAdClicked();
        public void OnAdFailed(string message);

        public interface IClosableAd : IAdStateListener
        {
            public void OnAdClosed(ShowCompletionState completionState);
        }
        
        public interface IRewarded : IClosableAd
        {
            public void OnRewarded();
        }

        public interface IInterstitial : IClosableAd {}

        public interface IBanner : IAdStateListener {}

        public interface INative : IAdStateListener {}
    }

    internal abstract class AdStateListenerImpl : IAdStateListener
    {
        [CanBeNull] private readonly Action _onImpression;
        [CanBeNull] private readonly Action _onClicked;
        [CanBeNull] private readonly Action<string> _onFailed;

        protected AdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<string> onFailed)
        {
            _onImpression = onImpression;
            _onClicked = onClicked;
            _onFailed = onFailed;
        }

        public void OnAdImpression() { _onImpression?.Invoke(); }

        public void OnAdClicked() { _onClicked?.Invoke(); }

        public void OnAdFailed(string message) { _onFailed?.Invoke(message); }
    }

    internal abstract class ClosableAdStateListenerImpl : AdStateListenerImpl,
        IAdStateListener.IClosableAd
    {
        [CanBeNull] private readonly Action<ShowCompletionState> _onClosed;

        protected ClosableAdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<ShowCompletionState> onClosed, [CanBeNull] Action<string> onFailed) : base(onImpression, onClicked, onFailed)
        {
            _onClosed = onClosed;
        }

        public void OnAdClosed(ShowCompletionState completionState)
        {
            _onClosed?.Invoke(completionState);
        }
    } 

    internal class RewardedAdStateListenerImpl : ClosableAdStateListenerImpl, IAdStateListener.IRewarded
    {
        [CanBeNull] private readonly Action _onRewarded;

        public RewardedAdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<ShowCompletionState> onClosed, [CanBeNull] Action<string> onFailed, Action onRewarded) : base(onImpression, onClicked, onClosed, onFailed)
        {
            _onRewarded = onRewarded;
        }

        public void OnRewarded() { _onRewarded?.Invoke(); }
    }
    
    internal class InterstitialAdStateListenerImpl : ClosableAdStateListenerImpl, IAdStateListener.IInterstitial
    {
        public InterstitialAdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<ShowCompletionState> onClosed, [CanBeNull] Action<string> onFailed) : base(onImpression, onClicked, onClosed, onFailed) {}
    }
    
    internal class BannerAdStateListenerImpl : AdStateListenerImpl, IAdStateListener.IBanner
    {
        public BannerAdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<string> onFailed) : base(onImpression, onClicked, onFailed) {}
    }
    
    internal class NativeAdStateListenerImpl : AdStateListenerImpl, IAdStateListener.INative
    {
        public NativeAdStateListenerImpl([CanBeNull] Action onImpression, [CanBeNull] Action onClicked, [CanBeNull] Action<string> onFailed) : base(onImpression, onClicked, onFailed) {}
    }
}
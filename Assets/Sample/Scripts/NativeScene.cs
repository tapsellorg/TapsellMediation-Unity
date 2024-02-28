using Sample.Scripts;
using Tapsell.Mediation.Show.Native;
using UnityEngine;

public class NativeScene : MonoBehaviour
{
    private const string ZoneID = TapsellMediationKeys.NATIVE;
    private static string _adId;
    private static int _viewType = 0;

    public void Request()
    {
        Tapsell.Mediation.Tapsell.RequestNativeAd(ZoneID,
            adId =>
            {
                Debug.Log("onNativeAd requestSuccess");
                _adId = adId;
            },
            () =>
            {
                Debug.Log("onNativeAd requestFailed");
            }
        );
    }

    public void Show()
    {
        GameObject advertiser;
        GameObject banner;
        GameObject description;
        GameObject icon;
        GameObject title;
        GameObject adChoices;
        GameObject cta;

        if (_viewType == 0)
        {
            advertiser = GameObject.Find("Advertiser");
            banner = GameObject.Find("Banner");
            description = GameObject.Find("Description");
            icon = GameObject.Find("Icon");
            title = GameObject.Find("Title");
            adChoices = GameObject.Find("AdChoices");
            cta = GameObject.Find("CTA");
        }
        else
        {
            advertiser = GameObject.Find("AdvertiserText");
            banner = GameObject.Find("BannerRawImage");
            description = GameObject.Find("DescriptionText");
            icon = GameObject.Find("IconRawImage");
            title = GameObject.Find("TitleText");
            adChoices = GameObject.Find("AdChoicesRawImage");
            cta = GameObject.Find("CTALegacy");
        }

        advertiser.AddComponent<BoxCollider>();
        banner.AddComponent<BoxCollider>();
        description.AddComponent<BoxCollider>();
        icon.AddComponent<BoxCollider>();
        title.AddComponent<BoxCollider>();
        adChoices.AddComponent<BoxCollider>();
        cta.AddComponent<BoxCollider>();

        var nativeAdView = new NativeAdView.Builder()
            .WithAdvertiserText(advertiser)
            .WithBannerImage(banner)
            .WithDescriptionText(description)
            .WithIconImage(icon)
            .WithTitleText(title)
            .WithAdChoicesImage(adChoices)
            .WithCtaButton(cta)
            .Build();

        Tapsell.Mediation.Tapsell.ShowNativeAd(
            _adId, nativeAdView,
            () => { Debug.Log("onNativeAd impression"); },
            () => { Debug.Log("onNativeAd click"); },
            message => { Debug.Log("onNativeAd showFailed: " + message); }
        );
    }

    public void Destroy()
    {
        Tapsell.Mediation.Tapsell.DestroyNativeAd(_adId);
    }
    
    public void OnViewTypeChanged(int newType)
    {
        _viewType = newType;
    }
}
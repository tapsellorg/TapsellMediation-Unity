using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tapsell.Mediation.Utils
{
    internal static class ViewHelper
    {
        internal static void SetImage(Texture2D texture, GameObject imageObject)
        {
            var image = imageObject.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                var rawImage = imageObject.GetComponent<RawImage>();
                if (rawImage != null)
                {
                    rawImage.texture = texture;
                }
            }
        }

        internal static void SetText(string text, GameObject textObject)
        {
            var legacy = textObject.GetComponent<Text>();
            if (legacy != null)
            {
                legacy.text = text;
            }
            else
            {
                var tmp = textObject.GetComponent<TMP_Text>();
                if (tmp != null)
                {
                    tmp.text = text;
                }
            }
        }

        internal static void SetButtonText(string text, GameObject buttonObject)
        {
            var legacy = buttonObject.GetComponentInChildren<Text>();
            if (legacy != null)
            {
                legacy.text = text;
            }
            else
            {
                var tmp = buttonObject.GetComponentInChildren<TMP_Text>();
                if (tmp != null)
                {
                    tmp.text = text;
                }
            }
        }
    }
}
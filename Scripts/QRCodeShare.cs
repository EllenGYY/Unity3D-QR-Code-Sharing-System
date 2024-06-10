using UnityEngine;
using ZXing;
using ZXing.QrCode;
using ZXing.Unity;

namespace QRCodeShareMain
{
    public static class QRCodeShare
    {
        public static string ReadQRCodeImage(Texture2D qrCodeImage)
        {
            if (qrCodeImage == null)
            {
                Debug.LogError("Invalid Image for extraction!");
                return null;
            }

            // create a reader with a custom luminance source
            var reader = new BarcodeReader();
            var result = reader.Decode(qrCodeImage.GetPixels32(), qrCodeImage.width, qrCodeImage.height);
            if (result != null)
            {
                return result.Text;
            }

            Debug.LogError("No Result Extracted from Image!");
            return null;
        }
        
        public static Texture2D CreateQRCodeImage(string content)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Margin = 0
                }
            };
            Color32Image color32Image = writer.Write(content);
            Color32[] colors = color32Image.Pixels;
            Texture2D texture = new Texture2D(color32Image.Width, color32Image.Height, TextureFormat.RGBA32, false);
            texture.SetPixels32(colors);
            texture.Apply();
            return texture;
        }
        
        public static Texture2D CreateQRCodeImage(string content, QRImageProperties properties)
        {
            int minSize = QRCodeMinimumSize(content).x;
            int shorterEdge = Mathf.Min(properties.Width, properties.Height); // the shorter in width and height
            int targetSize = (Mathf.Max(properties.Width, properties.Width) / minSize + 1) * minSize; // ZXing.net doesn't support non-integer resizing
            // Give out warning for fallback to min size QR Code
            if (shorterEdge < minSize)
            {
                Debug.LogWarning("Width or height is smaller than minimum QR Code Size. Size Setting ignored. Fallback to Min Size QR Code.");
            }
            
            // Create a target size no margin QR Code
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = shorterEdge >= minSize ? targetSize : 0, // if shorter edge smaller than min size, fallback to min size QR Code
                    Height = shorterEdge >= minSize ? targetSize : 0,
                    Margin = 0
                }
            };
            ((Color32Renderer)writer.Renderer).Foreground = properties.ForegroundColor;
            ((Color32Renderer)writer.Renderer).Background = properties.BackgroundColor;
            Color32Image color32Image = writer.Write(content);
            Color32[] colors = color32Image.Pixels;
            Texture2D texture = new Texture2D(color32Image.Width, color32Image.Height, TextureFormat.RGBA32, false);
            texture.SetPixels32(colors);
            texture.Apply();
            
            // Resize the QR Code to the desired size
            if (shorterEdge >= minSize)
            {
                texture = ImageProcessing.ResizeTexture(texture, properties.Width, properties.Height);
            }

            Texture2D paddedTexture = ImageProcessing.AddPadding(texture, properties.TopMargin, properties.BottomMargin,
                properties.LeftMargin, properties.RightMargin, properties.PaddingColor);
            return paddedTexture;
        }

        // Get the minimum size for the QR Code based on the content string
        public static Vector2Int QRCodeMinimumSize(string content)
        {
            Texture2D t = CreateQRCodeImage(content);
            return new Vector2Int(t.width, t.height);
        }
    }
}

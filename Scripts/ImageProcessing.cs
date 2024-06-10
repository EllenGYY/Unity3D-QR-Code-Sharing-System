using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QRCodeShareMain
{
    public static class ImageProcessing
    {
        public static Texture2D ResizeTexture(Texture2D source, int width, int height)
        {
            Texture2D result = new Texture2D(width, height, source.format, false);
            Color[] resizedPixels = new Color[width * height];
            float xRatio = (float)source.width / width;
            float yRatio = (float)source.height / height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float px = x * xRatio;
                    float py = y * yRatio;
                    resizedPixels[y * width + x] = source.GetPixelBilinear(px / source.width, py / source.height);
                }
            }

            result.SetPixels(resizedPixels);
            result.Apply();

            return result;
        }
        
        public static Texture2D AddPadding(Texture2D original, int top, int bottom, int left, int right, Color paddingColor)
        {
            int newWidth = original.width + left + right;
            int newHeight = original.height + top + bottom;
            
            Texture2D paddedTexture = new Texture2D(newWidth, newHeight);
            
            // Filling the background padding color
            Color[] paddingColors = new Color[newWidth * newHeight];
            System.Array.Fill(paddingColors, paddingColor);
            paddedTexture.SetPixels(paddingColors);

            // Copy the original texture into the desired position
            for (int y = 0; y < original.height; y++)
            {
                for (int x = 0; x < original.width; x++)
                {
                    paddedTexture.SetPixel(x + left, y + bottom, original.GetPixel(x, y));
                }
            }
            
            paddedTexture.Apply();
            return paddedTexture;
        }
        
        public static Texture2D CropTexture(Texture2D originalTexture,  int top, int bottom, int left, int right)
        {
            int newWidth = originalTexture.width - left - right;
            int newHeight = originalTexture.height - top - bottom;

            if (newWidth <= 0 || newHeight <= 0)
            {
                Debug.LogError("Invalid margins provided. The resulting cropped texture would have non-positive dimensions.");
                return null;
            }

            Color[] pixels = originalTexture.GetPixels(left, bottom, newWidth, newHeight);

            Texture2D croppedTexture = new Texture2D(newWidth, newHeight);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            return croppedTexture;
        }

        
        // The BlendImage function only support front image that has an alpha value of 1, it doesn't support semi-transparent pixels for now
        // center is the center position of the front image on the background image, the image would get cropped if it is outside the boundary
        public static Texture2D BlendImages(Texture2D frontImage, Texture2D backgroundImage, Vector2Int center)
        {
            Texture2D blendedTexture = new Texture2D(backgroundImage.width, backgroundImage.height);

            int offsetX = center.x - (frontImage.width / 2);
            int offsetY = center.y - (frontImage.height / 2);

            Texture2D frontImageCropped = CropTexture(frontImage, offsetY < 0 ? -offsetY : 0, 0, offsetX < 0 ? -offsetX : 0, 0);
            offsetX = Mathf.Max(0, offsetX);
            offsetY = Mathf.Max(0, offsetY);
            for (int x = 0; x < backgroundImage.width; x++)
            {
                for (int y = 0; y < backgroundImage.height; y++)
                {
                    Color backgroundPixel = backgroundImage.GetPixel(x, y);
                    Color blendedPixel = backgroundPixel;
                    if (x > offsetX && x < offsetX + frontImageCropped.width && y > offsetY && y < offsetY + frontImageCropped.height)
                    {
                        Color frontPixel = frontImageCropped.GetPixel(x - offsetX, y - offsetY);
                        blendedPixel = (frontPixel.a > 0.99f) ? frontPixel : blendedPixel;
                    }
                    blendedTexture.SetPixel(x, y, blendedPixel);
                }
            }
            blendedTexture.Apply();
            return blendedTexture;
        }
        
        
        public static Sprite ConvertTexture2DToSprite(Texture2D texture)
        {
            return Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        
        
    }
}

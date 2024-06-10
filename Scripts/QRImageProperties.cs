
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QRCodeShareMain
{
    public class QRImageProperties
    {
        private int _width; // without margin
        private int _height; // without margin
        private int _topMargin;
        private int _bottomMargin;
        private int _leftMargin;
        private int _rightMargin;
        
        private Color32 _foregroundColor;
        private Color32 _backgroundColor;
        private Color32 _paddingColor;
        

        public QRImageProperties()
        {
            _width = 0;
            _height = 0;
            _topMargin = 0;
            _bottomMargin = 0;
            _leftMargin = 0;
            _rightMargin = 0;
            _foregroundColor = Color.black;
            _backgroundColor = Color.white;
            _paddingColor = Color.white;
        }
        
        public QRImageProperties(int width, int height, int topMargin, int bottomMargin, int leftMargin,
            int rightMargin, Color32 foregroundColor, Color32 backgroundColor, Color32 paddingColor)
        {
            if (width < 0 || height < 0 || topMargin < 0 || bottomMargin < 0 || leftMargin < 0 || rightMargin < 0)
                throw new ArgumentException("All dimensions and margins must be non-negative.");

            if (width > height * 1.5f || height > width * 1.5f)
            {
                Debug.LogWarning("One edge of the QR Code would be greater than 1.5 times the other edge. it is not recommended because it may cause problems when scanning.");
            }
            _width = width;
            _height = height;
            _topMargin = topMargin;
            _bottomMargin = bottomMargin;
            _leftMargin = leftMargin;
            _rightMargin = rightMargin;
            _foregroundColor = foregroundColor;
            _backgroundColor = backgroundColor;
            _paddingColor = paddingColor;
        }
        
        public QRImageProperties(int width, int height, int margin)
        {
            if (width < 0 || height < 0 || margin < 0)
                throw new ArgumentException("All dimensions and margins must be non-negative.");
            if (width > height * 1.5f || height > width * 1.5f)
            {
                Debug.LogWarning("One edge of the QR Code would be greater than 1.5 times the other edge. it is not recommended because it may cause problems when scanning.");
            }
            _width = width;
            _height = height;
            _topMargin = margin;
            _bottomMargin = margin;
            _leftMargin = margin;
            _rightMargin = margin;
            _foregroundColor = Color.black;
            _backgroundColor = Color.white;
            _paddingColor = Color.white;
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Width must be positive");
                _width = value;
                if (_width > _height * 1.5f || _height > _width * 1.5f)
                {
                    Debug.LogWarning("One edge of the QR Code would be greater than 1.5 times the other edge. it is not recommended because it may cause problems when scanning.");
                }
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Height must be positive");
                _height = value;
                if (_width > _height * 1.5f || _height > _width * 1.5f)
                {
                    Debug.LogWarning("One edge of the QR Code would be greater than 1.5 times the other edge. it is not recommended because it may cause problems when scanning.");
                }
            }
        }

        public int TopMargin
        {
            get => _topMargin;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TopMargin cannot be negative");
                _topMargin = value;
            }
        }

        public int BottomMargin
        {
            get => _bottomMargin;
            set
            {
                if (value < 0)
                    throw new ArgumentException("BottomMargin cannot be negative");
                _bottomMargin = value;
            }
        }

        public int LeftMargin
        {
            get => _leftMargin;
            set
            {
                if (value < 0)
                    throw new ArgumentException("LeftMargin cannot be negative");
                _leftMargin = value;
            }
        }

        public int RightMargin
        {
            get => _rightMargin;
            set
            {
                if (value < 0)
                    throw new ArgumentException("RightMargin cannot be negative");
                _rightMargin = value;
            }
        }

        // Outer width with padding
        public int OuterWidth
        {
            get => _width + _leftMargin + _rightMargin;
            set
            {
                if (value - _leftMargin - _rightMargin < 0)
                    throw new ArgumentException("Width cannot be negative");
                
                _width = value - _leftMargin - _rightMargin;
            }
        }
        
        // Outer height with padding
        public int OuterHeight
        {
            get => _height + _topMargin + _bottomMargin;
            set
            {
                if (value - _topMargin - _bottomMargin < 0)
                    throw new ArgumentException("Height cannot be negative");
                
                _height = value - _topMargin - _bottomMargin;
            }
        }
        
        public Color32 ForegroundColor
        {
            get => _foregroundColor;
            set => _foregroundColor = value;
        }

        public Color32 BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public Color32 PaddingColor
        {
            get => _paddingColor;
            set => _paddingColor = value;
        }
        
        // Method to set uniform margin
        public void SetUniformMargin(int margin)
        {
            _topMargin = margin;
            _bottomMargin = margin;
            _leftMargin = margin;
            _rightMargin = margin;
        }
        
        // Method to set all margins
        public void SetAllMargins(int topMargin, int bottomMargin, int leftMargin,
            int rightMargin)
        {
            _topMargin = topMargin;
            _bottomMargin = bottomMargin;
            _leftMargin = leftMargin;
            _rightMargin = rightMargin;
        }
        
        // Method to set all colors
        public void SetAllColors(Color32 foregroundColor, Color32 backgroundColor, Color32 paddingColor)
        {
            _foregroundColor = foregroundColor;
            _backgroundColor = backgroundColor;
            _paddingColor = paddingColor;
        }

        public Vector2Int GetQRCodeCenter()
        {
            return new Vector2Int(LeftMargin + Width / 2, BottomMargin + Height / 2);
        }
        
        public override string ToString()
        {
            return
                $"Width: {Width}, Height: {Height}, " +
                $"Top Margin: {TopMargin}, Bottom Margin: {BottomMargin}, Left Margin: {LeftMargin}, Right Margin: {RightMargin}, " +
                $"Outer Width: {OuterWidth}, Outer Height: {OuterHeight}" +
                $"Foreground Color: {ForegroundColor}, Background Color: {BackgroundColor}, Padding Color: {PaddingColor}";
            
        }
    }
}
using System;
using System.Runtime.InteropServices;
using AOT;
using QRCodeShareMain;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

namespace QRCodeShareDemo
{
    public class UIInteractionDemo : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void DownloadFile(string fileName, string content);
        [DllImport("__Internal")]
        private static extern void UploadFile(Action<string> callbackMethodName);
        
        [Header("Tabs References")]
        [SerializeField] private Toggle tabGenerate;
        [SerializeField] private Toggle tabRead;
        [Header("Panels References")]
        [SerializeField] private GameObject panelGenerate;
        [SerializeField] private GameObject panelRead;
        [Header("Style Settings References")] 
        [SerializeField] private Toggle styleBasic;
        [SerializeField] private Toggle styleMilkTea;
        [SerializeField] private Toggle stylePolaroid;
        [Header("Generate QR Code References")]
        [SerializeField] private TMP_InputField contentEnterTextField;
        [SerializeField] private Image showImageGenerate;
        [SerializeField] private Button generateQRCode;
        [SerializeField] private Button downloadImage;
        [Header("Read QR Code References")] 
        [SerializeField] private TMP_InputField contentShowText;
        [SerializeField] private Image showImageRead;
        [SerializeField] private Button uploadImage;
        [SerializeField] private Button readQRCode;
        [Header("Sprite Resources")]
        [SerializeField] private Texture2D milkTeaLogo;
        [SerializeField] private Texture2D flightLogo;
        [SerializeField] private Texture2D polaroidBackground;

        private enum Style
        {
            basic,
            milktea,
            polaroid
        }
        private Style currentStyle = Style.basic;
        private Texture2D currentQRCodeGenerate = null;
        private Texture2D currentQRCodeRead = null;
        
        void Start()
        {
            // assign switch tabs
            tabGenerate.onValueChanged.AddListener(OnSwitchTabs);
            // assign style change
            styleBasic.onValueChanged.AddListener(OnChangeStyle);
            styleMilkTea.onValueChanged.AddListener(OnChangeStyle);
            // assign buttons
            generateQRCode.onClick.AddListener(OnClickGenerateQRCode);
            downloadImage.onClick.AddListener(OnDownloadButtonClicked);
            uploadImage.onClick.AddListener(OnUploadButtonClicked);
            readQRCode.onClick.AddListener(OnClickReadQRCode);
        }
        
        private Texture2D HelloWorldQRCode(string content)
        {
            QRImageProperties properties = new QRImageProperties(500, 500,50);
            Texture2D QRCodeImage = QRCodeShare.CreateQRCodeImage(content, properties);
            return QRCodeImage;
        }

        private Texture2D MilkTeaQRCode(string content)
        {
            QRImageProperties properties = new QRImageProperties(600, 600, 50);
            properties.SetAllColors(Color.black, Color.white, new Color(0.8f,1,1,1f));
            Texture2D QRCodeImage = QRCodeShare.CreateQRCodeImage(content, properties);
            Texture2D milkTeaResized = ImageProcessing.ResizeTexture(milkTeaLogo, 150, 150);
            QRCodeImage = ImageProcessing.BlendImages(milkTeaResized, QRCodeImage, properties.GetQRCodeCenter());
            return QRCodeImage;
        }

        private Texture2D PolaroidQRCode(string content)
        {
            // first padding 
            QRImageProperties properties = new QRImageProperties(400, 400, 40);
            properties.SetAllColors(Color.black, Color.white, Color.white);
            Texture2D QRCodeImage = QRCodeShare.CreateQRCodeImage(content, properties);
            Texture2D flightResized = ImageProcessing.ResizeTexture(flightLogo, 100, 100);
            QRCodeImage = ImageProcessing.BlendImages(flightResized, QRCodeImage, properties.GetQRCodeCenter());
            // second padding
            Texture2D background = ImageProcessing.ResizeTexture(polaroidBackground, 600, 900);
            QRCodeImage = ImageProcessing.BlendImages(QRCodeImage, background, new Vector2Int(60 + QRCodeImage.width / 2, 840 - QRCodeImage.height / 2));
            return QRCodeImage;
        }
        
        private void ShowImage(Image showImage, Texture2D image)
        {
            showImage.sprite = ImageProcessing.ConvertTexture2DToSprite(image);
            float imageSize = Mathf.Max(showImage.GetComponent<RectTransform>().sizeDelta.x, showImage.GetComponent<RectTransform>().sizeDelta.y);

            showImage.GetComponent<RectTransform>().sizeDelta = image.width <= image.height ? 
                new Vector2(imageSize / image.height * image.width, imageSize) : 
                new Vector2(imageSize, imageSize * image.height / image.width);
        }

        private void OnSwitchTabs(bool isOn)
        {
            panelGenerate.SetActive(isOn);
            panelRead.SetActive(!isOn);
            tabGenerate.GetComponent<Image>().color = isOn ? Color.cyan : Color.white;
            tabRead.GetComponent<Image>().color = isOn ? Color.white : Color.cyan;
        }

        private void OnChangeStyle(bool isOn)
        {
            if (styleBasic.isOn)
            {
                currentStyle = Style.basic;
                contentEnterTextField.placeholder.GetComponent<TextMeshProUGUI>().text = "Hello World";
            }
            else if (styleMilkTea.isOn)
            {
                currentStyle = Style.milktea;
                contentEnterTextField.placeholder.GetComponent<TextMeshProUGUI>().text = "I love milk tea!";
            }
            else
            {
                currentStyle = Style.polaroid;
                contentEnterTextField.placeholder.GetComponent<TextMeshProUGUI>().text = "Enjoy your trip!";
            }
        }

        private void OnClickGenerateQRCode()
        {
            string content = contentEnterTextField.text;
            if (content == "")
            {
                content = contentEnterTextField.placeholder.GetComponent<TextMeshProUGUI>().text;
            }
            
            switch (currentStyle)
            {
                case Style.basic:
                    currentQRCodeGenerate = HelloWorldQRCode(content);
                    break;
                case Style.milktea:
                    currentQRCodeGenerate = MilkTeaQRCode(content);
                    break;
                case Style.polaroid:
                    currentQRCodeGenerate = PolaroidQRCode(content);
                    break;
                default:
                    break;
            }

            if (currentQRCodeGenerate != null)
            {
                ShowImage(showImageGenerate, currentQRCodeGenerate);
                // Enable the download image button is the texture is not null
                downloadImage.interactable = true;
            }
            
        }

        private void OnClickReadQRCode()
        {
            if (currentQRCodeRead != null)
            {
                contentShowText.text = QRCodeShare.ReadQRCodeImage(currentQRCodeRead);
            }
            else
            {
                Debug.LogError("There is no uploaded image to read.");
            }
        }
        private void OnDownloadButtonClicked()
        {
            if (currentQRCodeGenerate != null)
            {
                string fileName = "QRCode.png";
                string content = Convert.ToBase64String(currentQRCodeGenerate.EncodeToPNG());
                DownloadFile(fileName, content);
            }
            else
            {
                Debug.LogError("There is no generated QR code to download.");
            }
        }
    
        private void OnUploadButtonClicked()
        {
            UploadFile(OnFileUploaded);
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void OnFileUploaded(string base64Data)
        {
            // remove the beginning "data:image/png;base64," part
            base64Data = base64Data.Substring(22);
            byte[] imageBytes = Convert.FromBase64String(base64Data);
            Texture2D texture = new Texture2D(2, 2); 
            if (texture.LoadImage(imageBytes))
            {
                // To access a non-static method, you need an instance of this class
                UIInteractionDemo instance = FindObjectOfType<UIInteractionDemo>();
                if (instance != null)
                {
                    instance.currentQRCodeRead = texture;
                    // Show the uploaded image
                    instance.ShowImage(instance.showImageRead, texture);
                    // Enable the read QR code button if the texture is not null
                    instance.readQRCode.interactable = true;
                }
            }
        }
        
    }
}
    

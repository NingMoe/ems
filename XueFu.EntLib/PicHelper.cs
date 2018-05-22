using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace XueFu.EntLib
{
    /**/
    /// <summary>
    /// WaterMark
    /// </summary>
    public class Watermark
    {
        private int _width;
        private int _height;
        private string _fontFamily;
        private int _fontSize;
        private bool _adaptable;
        private FontStyle _fontStyle;
        private bool _shadow;
        private string _backgroundImage;
        private Color _bgColor;
        private int _left;
        private string _resultImage;
        private string _text;
        private string _datetext;
        private string _PostName;
        private string _PostText;
        private int _top;
        private int _alpha;
        private int _red;
        private int _green;
        private int _blue;
        private long _quality;



        public Watermark()
        {
            //
            // TODO: Add constructor logic here
            //
            _width = 460;
            _height = 30;
            _fontFamily = "�����п�";
            _fontSize = 20;
            _fontStyle = FontStyle.Regular;
            _adaptable = true;
            _shadow = false;
            _left = 0;
            _top = 0;
            _alpha = 255;
            _red = 0;
            _green = 0;
            _blue = 0;
            _backgroundImage = "";
            _quality = 100;
            _bgColor = Color.FromArgb(255, 229, 229, 229);

        }

        /**/
        /// <summary>
        /// ����
        /// </summary>
        public string FontFamily
        {
            set { this._fontFamily = value; }
        }

        /**/
        /// <summary>
        /// ���ִ�С
        /// </summary>
        public int FontSize
        {
            set { this._fontSize = value; }
        }

        /**/
        /// <summary>
        /// ���ַ��
        /// </summary>
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        /**/
        /// <summary>
        /// ͸����0-255,255��ʾ��͸��
        /// </summary>
        public int Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        /**/
        /// <summary>
        /// ˮӡ�����Ƿ�ʹ����Ӱ
        /// </summary>
        public bool Shadow
        {
            get { return _shadow; }
            set { _shadow = value; }
        }

        public int Red
        {
            get { return _red; }
            set { _red = value; }
        }

        public int Green
        {
            get { return _green; }
            set { _green = value; }
        }

        public int Blue
        {
            get { return _blue; }
            set { _blue = value; }
        }

        /**/
        /// <summary>
        /// ��ͼ
        /// </summary>
        public string BackgroundImage
        {
            set { this._backgroundImage = value; }
        }

        /**/
        /// <summary>
        /// ˮӡ���ֵ���߾�
        /// </summary>
        public int Left
        {
            set { this._left = value; }
        }


        /**/
        /// <summary>
        /// ˮӡ���ֵĶ��߾�
        /// </summary>
        public int Top
        {
            set { this._top = value; }
        }

        /**/
        /// <summary>
        /// ���ɺ��ͼƬ
        /// </summary>
        public string ResultImage
        {
            set { this._resultImage = value; }
        }

        /**/
        /// <summary>
        /// ˮӡ�ı�
        /// </summary>
        public string Text
        {
            set { this._text = value; }
        }

        /**/
        /// <summary>
        /// ˮӡ�ı�
        /// </summary>
        public string DateText
        {
            set { this._datetext = value; }
        }

        /**/
        /// <summary>
        /// ְλ����
        /// </summary>
        public string PostName
        {
            set { this._PostName = value; }
        }

        /**/
        /// <summary>
        /// ְλ����
        /// </summary>
        public string PostText
        {
            set { this._PostText = value; }
        }


        /**/
        /// <summary>
        /// ����ͼƬ�Ŀ��
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /**/
        /// <summary>
        /// ����ͼƬ�ĸ߶�
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /**/
        /// <summary>
        /// ������̫���Ƿ���ݱ���ͼ���������ִ�С��Ĭ��Ϊ��Ӧ
        /// </summary>
        public bool Adaptable
        {
            get { return _adaptable; }
            set { _adaptable = value; }
        }

        public Color BgColor
        {
            get { return _bgColor; }
            set { _bgColor = value; }
        }

        /**/
        /// <summary>
        /// ���ͼƬ������������Χ0-100,����Ϊlong
        /// </summary>
        public long Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        /**/
        /// <summary>
        /// ��������ˮӡЧ��ͼ
        /// </summary>
        /// <returns>���ɳɹ�����true,���򷵻�false</returns>
        public bool Create()
        {
            try
            {
                Bitmap bitmap;
                Graphics g;

                //ʹ�ô�����ɫ
                if (this._backgroundImage.Trim() == "")
                {
                    bitmap = new Bitmap(this._width, this._height, PixelFormat.Format64bppArgb);
                    g = Graphics.FromImage(bitmap);
                    g.Clear(this._bgColor);
                }
                else
                {
                    bitmap = new Bitmap(Image.FromFile(this._backgroundImage));
                    g = Graphics.FromImage(bitmap);
                }
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;

                Font f = new Font(_fontFamily, _fontSize, _fontStyle);
                Font f1 = new Font("����", 12, FontStyle.Bold);
                Font f2 = new Font("������κ", 20, _fontStyle);
                Font f3 = new Font("������κ", 22, _fontStyle);
                SizeF size = g.MeasureString(_text, f);

                // �������ִ�Сֱ������ӦͼƬ�ߴ�
                while (_adaptable == true && size.Width > bitmap.Width)
                {
                    _fontSize--;
                    f = new Font(_fontFamily, _fontSize, _fontStyle);
                    size = g.MeasureString(_text, f);
                }

                Brush b = new SolidBrush(Color.FromArgb(_alpha, _red, _green, _blue));
                StringFormat StrFormat = new StringFormat();
                StrFormat.Alignment = StringAlignment.Near;

                if (this._shadow)
                {
                    Brush b2 = new SolidBrush(Color.FromArgb(90, 0, 0, 0));
                    g.DrawString(_text, f, b2, _left + 2, _top + 1);
                    g.DrawString(_datetext, f1, b2, 592, 500);
                }
                g.DrawString(_text, f, b, new PointF(_left, _top), StrFormat);
                g.DrawString(_datetext, f1, new SolidBrush(Color.FromArgb(255, 148, 114, 52)), new PointF(600, 499), StrFormat);
                g.DrawString(_PostName, f2, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new PointF((220 + (190 - 25 * _PostName.Length) / 2), 310), StrFormat);
                PointF ThirdPointF = new PointF(140 + (590 - 100 - (28 * _PostText.Length) - 50) / 2, 358);
                g.DrawString("�ر�����", f2, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), ThirdPointF, StrFormat);
                ThirdPointF = PointF.Add(ThirdPointF, new Size(100, -2));
                g.DrawString(_PostText, f3, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), ThirdPointF, StrFormat);
                ThirdPointF = PointF.Add(ThirdPointF, new Size(28 * _PostText.Length, 2));
                g.DrawString("����", f2, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), ThirdPointF, StrFormat);

                bitmap.Save(this._resultImage, ImageFormat.Jpeg);
                bitmap.Dispose();
                g.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


}

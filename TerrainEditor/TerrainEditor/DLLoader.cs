using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace TerrainEditor
{
    class XmlLoader
    {
        public static void Load<T>(string file, out T data)
        {
            try
            {
                XmlSerializer myXmlSerializer = new XmlSerializer(typeof(T));

                using (Stream reader = new FileStream(file, FileMode.Open))
                {
                    data = (T)myXmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                data = default(T);
            }
        }

        public static void Save<T>(string file, T data)
        {
            try
            {
                XmlSerializer myXmlSerializer = new XmlSerializer(typeof(T));

                using (Stream writer = new FileStream(file, FileMode.Create))
                {
                    myXmlSerializer.Serialize(writer, data);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                data = default(T);
            }
        }
    }

    class BmpLoader
    {
        public static void Load(Uri file, out int w, out int h, out int s, out byte[] data)
        {
            try
            {
                BitmapImage myBitmapImage = new BitmapImage(file);

                w = myBitmapImage.PixelWidth;
                h = myBitmapImage.PixelHeight;
                s = w + w % 4;

                data = new byte[s * h];
                myBitmapImage.CopyPixels(data, s, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                w = 0;
                h = 0;
                s = 0;
                data = null;
            }
        }
    }
}
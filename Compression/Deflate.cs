using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Compression
{
    public class Deflate
    {
        private string _text;
        private string _result;


        public Deflate(string text)
        {
            _text = text;
        }

        public Deflate()
        { 
        }

        public static string FastCompress(string text)
        {
            Deflate deflate = new Deflate();
            deflate.Text = text;
            deflate.Compress();
            
            return deflate.Result;
        }

        public static string FastDecompress(string bytes)
        {
            Deflate deflate = new Deflate(bytes);
            deflate.Decompress();
            return deflate.Result;
        }
        
        public void Compress()
        {
            byte[] buffer = Encoding.UTF8.GetBytes(_text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream , CompressionMode.Compress , true))
            {
                gZipStream.Write(buffer , 0 , buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData , 0 , compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData , 0 , gZipBuffer , 4 , compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length) , 0 , gZipBuffer , 0 , 4);
            _result = Convert.ToBase64String(gZipBuffer);
        }

        public void Decompress()
        {
            try
            {
                byte[] gZipBuffer = Convert.FromBase64String(_text);
                using (var memoryStream = new MemoryStream())
                {
                    int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                    memoryStream.Write(gZipBuffer , 4 , gZipBuffer.Length - 4);

                    var buffer = new byte[dataLength];

                    memoryStream.Position = 0;
                    using (var gZipStream = new GZipStream(memoryStream , CompressionMode.Decompress))
                    {
                        gZipStream.Read(buffer , 0 , buffer.Length);
                    }

                    _result = Encoding.UTF8.GetString(buffer);
                }
            }
            catch
            {
                _result = "Данные повреждены";
            }
        }

        /// <summary>
        /// Данные до процесса
        /// </summary>
        public string Text
        { 
            get => _text; 
            set => _text = value; 
        }

        /// <summary>
        /// Данные после процесса
        /// </summary>
        public string Result => _result;
    }
}

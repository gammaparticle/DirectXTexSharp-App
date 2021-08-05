using System;
using System.IO;
using DirectXTexSharp;


namespace DirectXTexSharpApp
{
    class Client
    {
        private byte[] _buffer = new byte[3000000];
        
        unsafe private void TCConvertAndSaveDdsImage(string ddsFilePath, ESaveFileTypes filetype, DXGI_FORMAT_WRAPPED format)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            _buffer = File.ReadAllBytes(ddsFilePath);
            

            fixed (byte* ptr = _buffer)
            {
                var ddsFileOutDir = Path.Combine(new FileInfo(ddsFilePath).Directory.FullName, "out");
                Directory.CreateDirectory(ddsFileOutDir);
                                                
                var fileName = Path.GetFileNameWithoutExtension(ddsFilePath);
                var saveFileExtension = filetype.ToString().ToLower();
                var saveFilePath = Path.Combine(ddsFileOutDir, $"{fileName}.{saveFileExtension}");
                var len = _buffer.Length;
                
                // test direct saving
                DirectXTexSharp.Texconv.ConvertAndSaveDdsImage(ptr, len, saveFilePath, filetype, false, false);

                // test buffer saving
                // var buffer = DirectXTexSharp.Texconv.ConvertDdsImageToArray(ptr, len, filetype, false, false);
                var mbuffer = DirectXTexSharp.Texconv.ConvertFromDdsArray(ptr, len, filetype, false, false);
                var newSaveFilePath = Path.Combine(ddsFileOutDir, $"{fileName}.2.{saveFileExtension}");
                File.WriteAllBytes(newSaveFilePath, mbuffer);
            }

        }
        static void Main(string[] args)
        {
            string ddsFilePath = @"D:\CyberPunkAssets\chicken_brown_d.dds";
            Client TexClient = new();
            foreach (int ft in Enum.GetValues(typeof(ESaveFileTypes)))
            {                
                Console.WriteLine("File type is: {0}", (ESaveFileTypes) ft);
                ESaveFileTypes saveFiletype = (ESaveFileTypes) ft;
                DXGI_FORMAT_WRAPPED dxgiFormat = DXGI_FORMAT_WRAPPED.DXGI_FORMAT_UNKNOWN;
                TexClient.TCConvertAndSaveDdsImage(ddsFilePath, saveFiletype, dxgiFormat);               
            }                 
        }
    }
}

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Ico
{
    /// <summary>     
    /// ICON 控制类，用于后续改进参考。
    /// </summary>
    public class IconDir
    {
        /*            if (openFileDialog1.ShowDialog() == DialogResult.OK)
         *            {
         *            IconDir T = new IconDir(openFileDialog1.FileName);
         *            T.GetImage(0);
         *            //获取一个ICO图形
         *            int Temp =T.ImageCount;//ICO数量               
         *            T.DelImage(0);   //删除
         *            T.SaveData(@"C:/1.ico"); //保存成文件
         *            }
         *            
         *            //添加一个ICO AddImage参数Rectangle 宽高不能超过255
         *            IconDir MyIcon = new IconDir();
         *            Image TempImage =Image.FromFile(@"c:/bfx/T5.BMP");
         *            MyIcon.AddImage(TempImage,new Rectangle(0,0,32,32))
         *            ;            MyIcon.SaveData(@"C:/1.ico");             
         */

        private ushort _IdReserved = 0;
        private ushort _IdType = 1;
        private ushort _IdCount = 1;
        private IList<IcoDirEntry> _Identries = new List<IcoDirEntry>();

        public IconDir()
        {
        }

        public IconDir(string IconFile)
        {
            System.IO.FileStream _FileStream = new System.IO.FileStream(IconFile, System.IO.FileMode.Open);
            byte[] IconData = new byte[_FileStream.Length];
            _FileStream.Read(IconData, 0, IconData.Length);
            _FileStream.Close();
            LoadData(IconData);
        }

        /// <summary>         
        /// 读取ICO         
        /// </summary>         
        /// <param name="IconData"></param>
        private void LoadData(byte[] IconData)
        {
            _IdReserved = BitConverter.ToUInt16(IconData, 0);
            _IdType = BitConverter.ToUInt16(IconData, 2);
            _IdCount = BitConverter.ToUInt16(IconData, 4);
            if (_IdType != 1 || _IdReserved != 0)
            {
                return;
            }

            int ReadIndex = 6;
            for (ushort i = 0; i != _IdCount; i++)
            {
                _Identries.Add(new IcoDirEntry(IconData, ref ReadIndex));
            }
        }

        /// <summary>         
        /// 保存ICO         
        /// </summary>         
        /// <param name="FileName"></param>
        public void SaveData(string FileName)
        {
            if (ImageCount == 0)
            {
                return;
            }

            var _FileStream = new FileStream(FileName, System.IO.FileMode.Create);

            byte[] Temp = BitConverter.GetBytes(_IdReserved);
            _FileStream.Write(Temp, 0, Temp.Length);
            Temp = BitConverter.GetBytes(_IdType);
            _FileStream.Write(Temp, 0, Temp.Length);
            Temp = BitConverter.GetBytes((ushort)ImageCount);
            _FileStream.Write(Temp, 0, Temp.Length);
            for (int i = 0; i != ImageCount; i++)
            {
                _FileStream.WriteByte(_Identries[i].Width);
                _FileStream.WriteByte(_Identries[i].Height);
                _FileStream.WriteByte(_Identries[i].ColorCount);
                _FileStream.WriteByte(_Identries[i].Breserved);
                Temp = BitConverter.GetBytes(_Identries[i].Planes);
                _FileStream.Write(Temp, 0, Temp.Length);
                Temp = BitConverter.GetBytes(_Identries[i].Bitcount);
                _FileStream.Write(Temp, 0, Temp.Length);
                Temp = BitConverter.GetBytes(_Identries[i].ImageSize);
                _FileStream.Write(Temp, 0, Temp.Length);
                Temp = BitConverter.GetBytes(_Identries[i].ImageRVA);
                _FileStream.Write(Temp, 0, Temp.Length);
            }
            for (int i = 0; i != ImageCount; i++)
            {
                _FileStream.Write(_Identries[i].Data, 0, _Identries[i].Data.Length);
            }
            _FileStream.Close();
        }

        /// <summary>        
        /// 根据索引返回图形         
        /// </summary>         
        /// <param name="Index"></param>        
        /// <returns></returns>        
        public SKBitmap GetImage(int Index)
        {
            BitmapInfo MyBitmap = new BitmapInfo();
            MyBitmap.Decode(_Identries[Index].Data);
            return MyBitmap.IconBmp;
        }

        public void AddImage(SKBitmap SetBitmap, Rectangle SetRectangle)
        {
            if (SetRectangle.Width > 255 || SetRectangle.Height > 255)
            {
                return;
            }

            var BmpMemory = SetBitmap.Encode(SKEncodedImageFormat.Bmp, 100).AsStream();
            BmpMemory.Position = 14;
            var bytes = new byte[BmpMemory.Length - 14 + 128];
            BmpMemory.Read(bytes, 0, bytes.Length);

            IcoDirEntry NewIconDirentry = new IcoDirEntry();
            //只使用13位后的数字 40开头
            NewIconDirentry.Data = bytes;
            NewIconDirentry.Width = (byte)SetRectangle.Width;
            NewIconDirentry.Height = (byte)SetRectangle.Height;
            //BMP图形和ICO的高不一样  ICO的高是BMP的2倍
            byte[] Height = BitConverter.GetBytes((uint)NewIconDirentry.Height * 2);
            NewIconDirentry.Data[8] = Height[0];
            NewIconDirentry.Data[9] = Height[1];
            NewIconDirentry.Data[10] = Height[2];
            NewIconDirentry.Data[11] = Height[3];
            NewIconDirentry.ImageSize = (uint)bytes.Length;
            _Identries.Add(NewIconDirentry);
            uint offset = 6 + (uint)(_Identries.Count * 16);
            for (int i = 0; i != _Identries.Count; i++)
            {
                _Identries[i].DwImageOffset = offset;
                offset += _Identries[i].ImageSize;
            }
        }

        public void DelImage(int Index)
        {
            _Identries.RemoveAt(Index);

            uint offset = 6 + (uint)(_Identries.Count * 16);
            for (int i = 0; i != _Identries.Count; i++)
            {
                _Identries[i].DwImageOffset = offset;
                offset += _Identries[i].ImageSize;
            }
        }

        /// <summary>         
        /// 返回图形数量         
        /// </summary>        
        public int ImageCount { get { return _Identries.Count; } }
    }
}

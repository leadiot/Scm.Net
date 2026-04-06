using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Scm.Image.SkiaSharp.Formats.Ico
{
    /// <summary>
    /// 图像信息
    /// </summary>
    public class BitmapInfo
    {
        private uint biSize = 40;
        private uint biWidth;
        private uint biHeight;
        private ushort biPlanes = 1;
        private ushort biBitCount;
        private uint biCompression = 0;
        private uint biSizeImage;
        private uint biXPelsPerMeter;
        private uint biYPelsPerMeter;
        private uint biClrUsed;
        private uint biClrImportant;
        public IList<SKColor> ColorTable = new List<SKColor>();

        /// <summary>             
        /// 占4位，位图信息头(Bitmap Info Header)的长度,一般为$28               
        /// </summary>            
        public uint InfoSize { get { return biSize; } set { biSize = value; } }
        /// <summary>             
        /// 占4位，位图的宽度，以象素为单位             
        /// </summary>            
        public uint Width { get { return biWidth; } set { biWidth = value; } }
        /// <summary>             
        /// 占4位，位图的高度，以象素为单位              
        /// </summary>            
        public uint Height { get { return biHeight; } set { biHeight = value; } }
        /// <summary>             
        /// 占2位，位图的位面数（注：该值将总是1）              
        /// </summary>        
        public ushort Planes { get { return biPlanes; } set { biPlanes = value; } }
        /// <summary>             
        /// 占2位，每个象素的位数，设为32(32Bit位图)               
        /// </summary>          
        public ushort BitCount { get { return biBitCount; } set { biBitCount = value; } }
        /// <summary>             
        /// 占4位，压缩说明，设为0(不压缩)               
        /// </summary>         
        public uint Compression { get { return biCompression; } set { biCompression = value; } }
        /// <summary>             
        /// 占4位，用字节数表示的位图数据的大小。该数必须是4的倍数               
        /// </summary>            
        public uint SizeImage { get { return biSizeImage; } set { biSizeImage = value; } }
        /// <summary>             
        /// 占4位，用象素/米表示的水平分辨率              
        /// </summary>            
        public uint XPelsPerMeter { get { return biXPelsPerMeter; } set { biXPelsPerMeter = value; } }
        /// <summary>             
        /// 占4位，用象素/米表示的垂直分辨率               
        /// </summary>      
        public uint YPelsPerMeter { get { return biYPelsPerMeter; } set { biYPelsPerMeter = value; } }
        /// <summary>             
        /// 占4位，位图使用的颜色数               
        /// </summary>            
        public uint ClrUsed { get { return biClrUsed; } set { biClrUsed = value; } }
        /// <summary>             
        /// 占4位，指定重要的颜色数(到此处刚好40个字节，$28)               
        /// </summary>         
        public uint ClrImportant { get { return biClrImportant; } set { biClrImportant = value; } }
        private SKBitmap _IconBitMap;
        /// <summary>             
        /// 图形            
        /// </summary>          
        public SKBitmap IconBmp { get { return _IconBitMap; } set { _IconBitMap = value; } }

        public BitmapInfo()
        {

        }

        public bool Decode(byte[] bytes)
        {
            var ReadIndex = 0;

            #region 基本数据    
            biSize = BitConverter.ToUInt32(bytes, ReadIndex);
            if (biSize != 40)
            {
                return false;
            }

            ReadIndex += 4;
            biWidth = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biHeight = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biPlanes = BitConverter.ToUInt16(bytes, ReadIndex);

            ReadIndex += 2;
            biBitCount = BitConverter.ToUInt16(bytes, ReadIndex);

            ReadIndex += 2;
            biCompression = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biSizeImage = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biXPelsPerMeter = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biYPelsPerMeter = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biClrUsed = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            biClrImportant = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            #endregion

            int ColorCount = RgbCount();
            if (ColorCount == -1)
            {
                return false;
            }

            for (int i = 0; i != ColorCount; i++)
            {
                byte Blue = bytes[ReadIndex];
                byte Green = bytes[ReadIndex + 1];
                byte Red = bytes[ReadIndex + 2];
                byte Reserved = bytes[ReadIndex + 3];
                ColorTable.Add(new SKColor(Red, Green, Blue, Reserved));
                ReadIndex += 4;
            }

            var res = (biBitCount * biWidth) / 8D;       // 象素的大小*象素数 /字节数
            int Size = (int)res;
            //如果是 宽19*4（16色）/8 =9.5 就+1;
            if (res < 9.5)
            {
                Size += 1;
            }

            if (Size < 4)
            {
                Size = 4;
            }

            var width = (int)biWidth;
            var height = (int)biHeight / 2;
            byte[] WidthByte = new byte[Size];
            _IconBitMap = new SKBitmap(width, height);
            var canvas = new SKCanvas(_IconBitMap);
            canvas.DrawRect(0, 0, width, height, new SKPaint { Color = SKColors.Transparent });

            for (int i = height; i != 0; i--)
            {
                for (int z = 0; z != Size; z++)
                {
                    WidthByte[z] = bytes[ReadIndex + z];
                }
                ReadIndex += Size;
                IconSet(_IconBitMap, i - 1, WidthByte);
            }
            //取掩码
            int MaskSize = (int)(biWidth / 8);
            if ((double)MaskSize < biWidth / 8) MaskSize++;       //如果是 宽19*4（16色）/8 =9.5 就+1;
            if (MaskSize < 4) MaskSize = 4;
            byte[] MashByte = new byte[MaskSize];
            for (int i = height; i != 0; i--)
            {
                for (int z = 0; z != MaskSize; z++)
                {
                    MashByte[z] = bytes[ReadIndex + z];
                }
                ReadIndex += MaskSize;
                IconMask(_IconBitMap, i - 1, MashByte);
            }

            return true;
        }

        private int RgbCount()
        {
            switch (biBitCount)
            {
                case 1:
                    return 2;
                case 4:
                    return 16;
                case 8:
                    return 256;
                case 24:
                    return 0;
                case 32:
                    return 0;
                default:
                    return -1;
            }
        }

        private void IconSet(SKBitmap image, int rowIdx, byte[] bytes)
        {
            int index = 0;
            switch (biBitCount)
            {
                case 1:
                    #region 一次读8位 绘制8个点  
                    for (int i = 0; i != bytes.Length; i++)
                    {
                        var array = new BitArray(new byte[] { bytes[i] });
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[7])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[6])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[5])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[4])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[3])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[2])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[1])]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[GetBitNumb(array[0])]);
                        index++;
                    }
                    #endregion
                    break;
                case 4:
                    #region 一次读8位 绘制2个点                
                    for (int i = 0; i != bytes.Length; i++)
                    {
                        int High = bytes[i] >> 4;  //取高4位
                        int Low = bytes[i] - (High << 4); //取低4位
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[High]);
                        index++;
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[Low]);
                        index++;
                    }
                    #endregion
                    break;
                case 8:
                    #region 一次读8位 绘制一个点            
                    for (int i = 0; i != bytes.Length; i++)
                    {
                        if (index >= image.Width) return;
                        image.SetPixel(index, rowIdx, ColorTable[bytes[i]]);
                        index++;
                    }
                    #endregion
                    break;
                case 24:
                    #region 一次读24位 绘制一个点       
                    for (int i = 0; i != bytes.Length / 3; i++)
                    {
                        if (i >= image.Width) return;
                        image.SetPixel(i, rowIdx, new SKColor(bytes[index + 2], bytes[index + 1], bytes[index]));
                        index += 3;
                    }
                    #endregion
                    break;
                case 32:
                    #region 一次读32位 绘制一个点             
                    for (int i = 0; i != bytes.Length / 4; i++)
                    {
                        if (i >= image.Width) return;
                        image.SetPixel(i, rowIdx, new SKColor(bytes[index + 2], bytes[index + 1], bytes[index]));
                        index += 4;
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        private void IconMask(SKBitmap IconImage, int rowIndex, byte[] bytes)
        {
            var Set = new BitArray(bytes);
            int ReadIndex = 0;
            for (int i = Set.Count; i != 0; i--)
            {
                if (ReadIndex >= IconImage.Width)
                    return;

                var SetColor = IconImage.GetPixel(ReadIndex, rowIndex);

                if (!Set[i - 1])
                    IconImage.SetPixel(ReadIndex, rowIndex, new SKColor(SetColor.Red, SetColor.Green, SetColor.Blue, 255));

                ReadIndex++;
            }
        }

        private int GetBitNumb(bool BitArray)
        {
            if (BitArray)
                return 1;
            return 0;
        }
    }
}

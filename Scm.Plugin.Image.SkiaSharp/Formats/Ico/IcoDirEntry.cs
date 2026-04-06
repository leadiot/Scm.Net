using System;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Ico
{
    /// <summary>
    /// DirEntry
    /// </summary>
    public class IcoDirEntry
    {
        public IcoDirEntry()
        {
        }

        private byte bWidth = 16;
        private byte bHeight = 16;
        private byte bColorCount = 0;
        private byte bReserved = 0;        //4
        private UInt16 wPlanes = 1;
        private UInt16 wBitCount = 32;      //8

        private UInt32 dwBytesInRes = 0;
        private UInt32 dwImageOffset = 0;         //16

        private byte[] _ImageData;
        /// <summary>             
        /// 图像宽度，以象素为单位。一个字节             
        /// </summary>           
        public byte Width { get { return bWidth; } set { bWidth = value; } }
        /// <summary>             
        /// 图像高度，以象素为单位。一个字节              
        /// </summary>            
        public byte Height { get { return bHeight; } set { bHeight = value; } }
        /// <summary>            
        /// 图像中的颜色数（如果是>=8bpp的位图则为0）            
        /// </summary>         
        public byte ColorCount { get { return bColorCount; } set { bColorCount = value; } }
        /// <summary>             
        /// 保留字必须是0             
        /// </summary>           
        public byte Breserved { get { return bReserved; } set { bReserved = value; } }
        /// <summary>             
        /// 为目标设备说明位面数，其值将总是被设为1             
        /// </summary>           
        public UInt16 Planes { get { return wPlanes; } set { wPlanes = value; } }
        /// <summary>             
        /// 每象素所占位数。             
        /// </summary>             
        public UInt16 Bitcount { get { return wBitCount; } set { wBitCount = value; } }
        /// <summary>             
        /// 字节大小。             
        /// </summary>            
        public uint ImageSize { get { return dwBytesInRes; } set { dwBytesInRes = value; } }
        public UInt32 DwBytesInRes { get { return dwBytesInRes; } set { dwBytesInRes = value; } }
        /// <summary>             
        /// /// 起点偏移位置。             
        /// </summary>            
        public uint ImageRVA { get { return dwImageOffset; } set { dwImageOffset = value; } }
        public UInt32 DwImageOffset { get { return dwImageOffset; } set { dwImageOffset = value; } }
        /// <summary>            
        /// 图形数据             
        /// </summary>            
        public byte[] Data { get { return _ImageData; } set { _ImageData = value; } }

        public IcoDirEntry(byte[] bytes, ref int ReadIndex)
        {
            bWidth = bytes[ReadIndex];

            ReadIndex++;
            bHeight = bytes[ReadIndex];

            ReadIndex++;
            bColorCount = bytes[ReadIndex];

            ReadIndex++;
            bReserved = bytes[ReadIndex];

            ReadIndex++;
            wPlanes = BitConverter.ToUInt16(bytes, ReadIndex);

            ReadIndex += 2;
            wBitCount = BitConverter.ToUInt16(bytes, ReadIndex);

            ReadIndex += 2;
            dwBytesInRes = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            dwImageOffset = BitConverter.ToUInt32(bytes, ReadIndex);

            ReadIndex += 4;
            System.IO.MemoryStream MemoryData = new System.IO.MemoryStream(bytes, (int)dwImageOffset, (int)dwBytesInRes);

            _ImageData = new byte[dwBytesInRes];
            MemoryData.Read(_ImageData, 0, _ImageData.Length);
        }

        public bool FromStream(Stream stream)
        {
            bWidth = (byte)stream.ReadByte();
            bHeight = (byte)stream.ReadByte();
            bColorCount = (byte)stream.ReadByte();
            bReserved = (byte)stream.ReadByte();

            UInt16 val16 = 0;
            val16 = Convert.ToUInt16(val16 + stream.ReadByte());
            val16 = Convert.ToUInt16(val16 + (stream.ReadByte() << 8));
            wPlanes = val16;

            val16 = 0;
            val16 = Convert.ToUInt16(val16 + stream.ReadByte());
            val16 = Convert.ToUInt16(val16 + (stream.ReadByte() << 8));
            wBitCount = val16;

            UInt32 val32 = 0;
            val32 = Convert.ToUInt32(val32 + stream.ReadByte());
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 8));
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 16));
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 24));
            dwBytesInRes = val32;

            val32 = 0;
            val32 = Convert.ToUInt32(val32 + stream.ReadByte());
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 8));
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 16));
            val32 = Convert.ToUInt32(val32 + (stream.ReadByte() << 24));
            dwImageOffset = val32;

            return true;
        }

        public void ToStream(Stream stream)
        {
            stream.WriteByte(bWidth);
            stream.WriteByte(bHeight);
            stream.WriteByte(bColorCount);
            stream.WriteByte(bReserved);

            stream.WriteByte(Convert.ToByte(wPlanes & 0xff));
            stream.WriteByte(Convert.ToByte((wPlanes >> 8) & 0xff));

            stream.WriteByte(Convert.ToByte(wBitCount & 0xff));
            stream.WriteByte(Convert.ToByte(wBitCount >> 8));

            stream.WriteByte(Convert.ToByte(dwBytesInRes & 0xff));
            stream.WriteByte(Convert.ToByte((dwBytesInRes >> 8) & 0xff));
            stream.WriteByte(Convert.ToByte((dwBytesInRes >> 16) & 0xff));
            stream.WriteByte(Convert.ToByte((dwBytesInRes >> 24) & 0xff));

            stream.WriteByte(Convert.ToByte(dwImageOffset & 0xff));
            stream.WriteByte(Convert.ToByte((dwImageOffset >> 8) & 0xff));
            stream.WriteByte(Convert.ToByte((dwImageOffset >> 16) & 0xff));
            stream.WriteByte(Convert.ToByte((dwImageOffset >> 24) & 0xff));
        }
    }
}

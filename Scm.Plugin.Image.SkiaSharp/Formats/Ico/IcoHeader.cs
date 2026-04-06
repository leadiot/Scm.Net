using System;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image.SkiaSharp.Formats.Ico
{
    /// <summary>
    /// 目录头，6字节
    /// </summary>
    class IcoHeader
    {
        public UInt32 idReserved = 0;  // 保留位必须为0
        public UInt32 idType = 1;      // 类型 (1=icon,2=光标)，也就是必须为1
        public UInt32 idCount = 0;     // 包含多少张图（icon可以含多张图，分别在不同尺寸下展示）
        public IList<IcoDirEntry> _Identries = new List<IcoDirEntry>();

        public void ToStream(Stream stream)
        {
            stream.WriteByte(Convert.ToByte(idReserved & 0xff));
            stream.WriteByte(Convert.ToByte(idReserved >> 8));

            stream.WriteByte(Convert.ToByte(idType & 0xff));
            stream.WriteByte(Convert.ToByte(idType >> 8));

            stream.WriteByte(Convert.ToByte(idCount & 0xff));
            stream.WriteByte(Convert.ToByte(idCount >> 8));
        }

        public bool FromStream(Stream stream)
        {
            UInt32 val = 0;
            val = Convert.ToUInt32(val + stream.ReadByte());
            val = Convert.ToUInt32(val + (stream.ReadByte() << 8));
            if (val != idReserved)
            {
                return false;
            }

            val = 0;
            val = Convert.ToUInt32(val + stream.ReadByte());
            val = Convert.ToUInt32(val + (stream.ReadByte() << 8));
            if (val != idType)
            {
                return false;
            }

            val = 0;
            val = Convert.ToUInt32(val + stream.ReadByte());
            val = Convert.ToUInt32(val + (stream.ReadByte() << 8));
            idCount = val;

            return true;
        }
    }
}

namespace Com.Scm.Image.Exif
{
    public class ImageExif
    {
        /// <summary>
        /// Exif版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// 拍摄时间
        /// </summary>
        public string CamearTime { get; set; }
        /// <summary>
        /// 程序名称
        /// </summary>
        public string Software { get; set; }
        /// <summary>
        /// 版权
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// 图像宽度
        /// </summary>
        public string ImageWidth { get; set; }
        /// <summary>
        /// 图像高度
        /// </summary>
        public string ImageHeight { get; set; }
        /// <summary>
        /// 水平分辨率
        /// </summary>
        public string XResolution { get; set; }
        /// <summary>
        /// 垂直分辨率
        /// </summary>
        public string YResolution { get; set; }
        /// <summary>
        /// 分辨率单位
        /// </summary>
        public string ResolutionUnit { get; set; }
        /// <summary>
        /// 色彩空间
        /// </summary>
        public string ColorSpace { get; set; }
        /// <summary>
        /// 压缩率
        /// </summary>
        public string CompressedBitsPerPixel { get; set; }
        /// <summary>
        /// 相机厂商
        /// </summary>
        public string Make { get; set; }
        /// <summary>
        /// 相机型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 光圈
        /// </summary>
        public string ApertureValue { get; set; }
        /// <summary>
        /// 最大光圈
        /// </summary>
        public string MaxApertureValue { get; set; }
        /// <summary>
        /// 曝光时间
        /// </summary>
        public string ExposureTime { get; set; }
        /// <summary>
        /// 曝光补偿
        /// </summary>
        public string ExposureBiasValue { get; set; }
        /// <summary>
        /// ISO 速度
        /// </summary>
        public string ISOSpeed { get; set; }
        /// <summary>
        /// ISO 感光
        /// </summary>
        public string ISOSpeedRatings { get; set; }
        /// <summary>
        /// 测光模式
        /// </summary>
        public string MeteringMode { get; set; }
        /// <summary>
        /// 焦距
        /// </summary>
        public string FocalLength { get; set; }
        /// <summary>
        /// 目标距离
        /// </summary>
        public string SubjectDistance { get; set; }
        /// <summary>
        /// 闪光灯模式
        /// </summary>
        public string Flash { get; set; }
        /// <summary>
        /// 闪光灯能量
        /// </summary>
        public string FlashEnergy { get; set; }
        /// <summary>
        /// 快门速度
        /// </summary>
        public string ShutterSpeedValue { get; set; }
        /// <summary>
        /// 拍摄方向
        /// </summary>
        public string Orientation { get; set; }
        /// <summary>
        /// 镜头厂商
        /// </summary>
        public string LensMake { get; set; }
        /// <summary>
        /// 镜头型号
        /// </summary>
        public string LensModel { get; set; }
        /// <summary>
        /// 镜头编码
        /// </summary>
        public string LensSerialNumber { get; set; }
        /// <summary>
        /// 曝光程序
        /// </summary>
        public string ExposureProgram { get; set; }
        /// <summary>
        /// 光源
        /// </summary>
        public string LightSource { get; set; }
        /// <summary>
        /// 亮度
        /// </summary>
        public string BrightnessValue { get; set; }
        /// <summary>
        /// 白平衡
        /// </summary>
        public string WhiteBalance { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public string GPSAltitude { get; set; }
        /// <summary>
        /// 高度参照
        /// </summary>
        public string GPSAltitudeRef { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string GPSLongitude { get; set; }
        /// <summary>
        /// 经度参照
        /// </summary>
        public string GPSLongitudeRef { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string GPSLatitude { get; set; }
        /// <summary>
        /// 纬度参照
        /// </summary>
        public string GPSLatitudeRef { get; set; }


        //var meta = image.Metadata;
        //var exif = meta.ExifProfile;
        //if (exif != null)
        //{
        //    // 基础
        //    var info = new FileInfo(file);
        //    Console.WriteLine("=================基础================");
        //    Console.WriteLine("文件名称：" + info.Name);
        //    Console.WriteLine("文件路径：" + info.DirectoryName);
        //    Console.WriteLine("文件类型：" + info.Extension);
        //    Console.WriteLine("MIME类型：" + "image/jpg");
        //    Console.WriteLine("创建时间：" + info.CreationTime);
        //    Console.WriteLine("更新时间：" + info.LastWriteTime);
        //    Console.WriteLine("文件大小：" + info.Length);
        //    Console.WriteLine("文件来源：" + Environment.MachineName);
        //    Console.WriteLine("所有者：" + "Owner");

        //    // Exif
        //    Console.WriteLine("=================Exif================");
        //    Console.WriteLine("Exif版本：" + GetValue(exif, ExifTag.ExifVersion));
        //    Console.WriteLine("备注：" + GetValue(exif, ExifTag.ImageDescription));

        //    // 来源
        //    Console.WriteLine("=================来源================");
        //    Console.WriteLine("作者：" + GetValue(exif, ExifTag.Artist));
        //    Console.WriteLine("拍摄时间：" + GetValue(exif, ExifTag.DateTimeOriginal));
        //    Console.WriteLine("程序名称：" + GetValue(exif, ExifTag.Software));
        //    Console.WriteLine("版权：" + GetValue(exif, ExifTag.Copyright));

        //    // 图像
        //    Console.WriteLine("=================图像================");
        //    Console.WriteLine("图像宽度：" + GetValue(exif, ExifTag.ImageWidth));
        //    Console.WriteLine("图像高度：" + GetValue(exif, ExifTag.ImageLength));
        //    Console.WriteLine("水平分辨率：" + GetValue(exif, ExifTag.XResolution));
        //    Console.WriteLine("垂直分辨率：" + GetValue(exif, ExifTag.YResolution));
        //    Console.WriteLine("分辨率单位：" + GetValue(exif, ExifTag.ResolutionUnit));
        //    Console.WriteLine("色彩空间：" + GetValue(exif, ExifTag.ColorSpace));
        //    Console.WriteLine("压缩率：" + GetValue(exif, ExifTag.CompressedBitsPerPixel));

        //    // 相机
        //    Console.WriteLine("=================相机================");
        //    Console.WriteLine("相机厂商：" + GetValue(exif, ExifTag.Make));
        //    Console.WriteLine("相机型号：" + GetValue(exif, ExifTag.Model));
        //    Console.WriteLine("光圈：" + GetValue(exif, ExifTag.ApertureValue));
        //    Console.WriteLine("最大光圈：" + GetValue(exif, ExifTag.MaxApertureValue));
        //    Console.WriteLine("曝光时间：" + GetValue(exif, ExifTag.ExposureTime));
        //    Console.WriteLine("曝光补偿：" + GetValue(exif, ExifTag.ExposureBiasValue));
        //    Console.WriteLine("ISO 速度：" + GetValue(exif, ExifTag.ISOSpeed));
        //    Console.WriteLine("ISO 感光：" + GetValue(exif, ExifTag.ISOSpeedRatings));
        //    Console.WriteLine("测光模式：" + GetValue(exif, ExifTag.MeteringMode));
        //    Console.WriteLine("焦距：" + GetValue(exif, ExifTag.FocalLength));
        //    Console.WriteLine("目标距离：" + GetValue(exif, ExifTag.SubjectDistance));
        //    //Console.WriteLine("目标距离：" + GetValue(exif, ExifTag.SubjectDistanceRange));
        //    Console.WriteLine("闪光灯模式：" + GetValue(exif, ExifTag.Flash));
        //    Console.WriteLine("闪光灯能量：" + GetValue(exif, ExifTag.FlashEnergy));
        //    Console.WriteLine("快门速度：" + GetValue(exif, ExifTag.ShutterSpeedValue));
        //    Console.WriteLine("拍摄方向：" + GetValue(exif, ExifTag.Orientation));

        //    // 镜头
        //    Console.WriteLine("=================镜头================");
        //    Console.WriteLine("镜头厂商：" + GetValue(exif, ExifTag.LensMake));
        //    Console.WriteLine("镜头型号：" + GetValue(exif, ExifTag.LensModel));
        //    Console.WriteLine("镜头编码：" + GetValue(exif, ExifTag.LensSerialNumber));
        //    Console.WriteLine("曝光程序：" + GetValue(exif, ExifTag.ExposureProgram));
        //    Console.WriteLine("光源：" + GetValue(exif, ExifTag.LightSource));
        //    Console.WriteLine("亮度：" + GetValue(exif, ExifTag.BrightnessValue));
        //    Console.WriteLine("白平衡：" + GetValue(exif, ExifTag.WhiteBalance));

        //    // 位置
        //    Console.WriteLine("=================位置================");
        //    Console.WriteLine("高度：" + GetValue(exif, ExifTag.GPSAltitude));
        //    Console.WriteLine("高度参照：" + GetValue(exif, ExifTag.GPSAltitudeRef));
        //    Console.WriteLine("经度：" + GetValue(exif, ExifTag.GPSLongitude));
        //    Console.WriteLine("经度参照：" + GetValue(exif, ExifTag.GPSLongitudeRef));
        //    Console.WriteLine("纬度：" + GetValue(exif, ExifTag.GPSLatitude));
        //    Console.WriteLine("纬度参照：" + GetValue(exif, ExifTag.GPSLatitudeRef));

        //    // 日志
        //    Console.WriteLine("=================日志================");
        //}
    }
}

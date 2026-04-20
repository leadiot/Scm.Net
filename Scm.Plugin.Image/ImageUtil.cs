using MetadataExtractor;
using System.Collections.Generic;
using System.IO;

namespace Com.Scm.Image
{
    public class ImageUtil
    {
        #region EXIF处理
        public static Dictionary<string, string> GetExifInfo(Stream stream)
        {
            var imr = ImageMetadataReader.ReadMetadata(stream);
            if (imr == null)
            {
                return null;
            }

            var dict = new Dictionary<string, string>();
            foreach (var im in imr)
            {
                foreach (var tag in im.Tags)
                {
                    var temp = EngToChs(tag.Name);
                    if (temp == "其他")
                    {
                        continue;
                    }

                    dict[temp] = tag.Description;
                }
            }
            return dict;

            //var exifInfo = new ImageExif();

            //// Exif
            //exifInfo.Version = GetValue(exifMeta, ExifTag.ExifVersion);
            //exifInfo.Comment = GetValue(exifMeta, ExifTag.ImageDescription);

            //// 来源
            //exifInfo.Artist = GetValue(exifMeta, ExifTag.Artist);
            //exifInfo.CamearTime = GetValue(exifMeta, ExifTag.DateTimeOriginal);
            //exifInfo.Software = GetValue(exifMeta, ExifTag.Software);
            //exifInfo.Copyright = GetValue(exifMeta, ExifTag.Copyright);

            //// 图像
            //exifInfo.ImageWidth = GetValue(exifMeta, ExifTag.ImageWidth);
            //exifInfo.ImageHeight = GetValue(exifMeta, ExifTag.ImageLength);
            //exifInfo.XResolution = GetValue(exifMeta, ExifTag.XResolution);
            //exifInfo.YResolution = GetValue(exifMeta, ExifTag.YResolution);
            //exifInfo.ResolutionUnit = exifInfo.ResolutionUnit = GetValue(exifMeta, ExifTag.ResolutionUnit);
            //exifInfo.ColorSpace = GetValue(exifMeta, ExifTag.ColorSpace);
            //exifInfo.CompressedBitsPerPixel = exifInfo.CompressedBitsPerPixel = GetValue(exifMeta, ExifTag.CompressedBitsPerPixel);

            //// 相机
            //exifInfo.Make = GetValue(exifMeta, ExifTag.Make);
            //exifInfo.Model = exifInfo.Model = GetValue(exifMeta, ExifTag.Model);
            //exifInfo.ApertureValue = GetValue(exifMeta, ExifTag.ApertureValue);
            //exifInfo.MaxApertureValue = GetValue(exifMeta, ExifTag.MaxApertureValue);
            //exifInfo.ExposureTime = GetValue(exifMeta, ExifTag.ExposureTime);
            //exifInfo.ExposureBiasValue = GetValue(exifMeta, ExifTag.ExposureBiasValue);
            //exifInfo.ISOSpeed = GetValue(exifMeta, ExifTag.ISOSpeed);
            //exifInfo.ISOSpeedRatings = GetValue(exifMeta, ExifTag.ISOSpeedRatings);
            //exifInfo.MeteringMode = GetValue(exifMeta, ExifTag.MeteringMode);
            //exifInfo.FocalLength = GetValue(exifMeta, ExifTag.FocalLength);
            //exifInfo.SubjectDistance = GetValue(exifMeta, ExifTag.SubjectDistance);
            ////Console.WriteLine("目标距离：" + GetValue(exif, ExifTag.SubjectDistanceRange));
            //exifInfo.Flash = GetValue(exifMeta, ExifTag.Flash);
            //exifInfo.FlashEnergy = GetValue(exifMeta, ExifTag.FlashEnergy);
            //exifInfo.ShutterSpeedValue = GetValue(exifMeta, ExifTag.ShutterSpeedValue);
            //exifInfo.Orientation = GetValue(exifMeta, ExifTag.Orientation);

            //// 镜头
            //exifInfo.LensMake = GetValue(exifMeta, ExifTag.LensMake);
            //exifInfo.LensModel = GetValue(exifMeta, ExifTag.LensModel);
            //exifInfo.LensSerialNumber = GetValue(exifMeta, ExifTag.LensSerialNumber);
            //exifInfo.ExposureProgram = GetValue(exifMeta, ExifTag.ExposureProgram);
            //exifInfo.LightSource = GetValue(exifMeta, ExifTag.LightSource);
            //exifInfo.BrightnessValue = GetValue(exifMeta, ExifTag.BrightnessValue);
            //exifInfo.WhiteBalance = GetValue(exifMeta, ExifTag.WhiteBalance);

            //// 位置
            //exifInfo.GPSAltitude = GetValue(exifMeta, ExifTag.GPSAltitude);
            //exifInfo.GPSAltitudeRef = GetValue(exifMeta, ExifTag.GPSAltitudeRef);
            //exifInfo.GPSLongitude = GetValue(exifMeta, ExifTag.GPSLongitude);
            //exifInfo.GPSLongitudeRef = GetValue(exifMeta, ExifTag.GPSLongitudeRef);
            //exifInfo.GPSLatitude = GetValue(exifMeta, ExifTag.GPSLatitude);
            //exifInfo.GPSLatitudeRef = GetValue(exifMeta, ExifTag.GPSLatitudeRef);

            //return exifInfo;
        }

        private static Dictionary<string, string> _TagNames;
        /// <summary>筛选参数并将其名称转换为中文
        /// </summary>
        /// <param name="str">参数名称</param>
        /// <returns>参数中文名</returns>
        private static string EngToChs(string str)
        {
            if (_TagNames == null)
            {
                _TagNames = new Dictionary<string, string>();
                _TagNames["Exif Version"] = "Exif版本";
                _TagNames["Model"] = "相机型号";
                _TagNames["Lens Model"] = "镜头类型";
                _TagNames["File Name"] = "文件名";
                _TagNames["File Size"] = "文件大小";
                _TagNames["Date/Time"] = "拍摄时间";
                _TagNames["File Modified Date"] = "修改时间";
                _TagNames["Image Height"] = "照片高度";
                _TagNames["Image Width"] = "照片宽度";
                _TagNames["X Resolution"] = "水平分辨率";
                _TagNames["Y Resolution"] = "垂直分辨率";
                _TagNames["Color Space"] = "色彩空间";
                _TagNames["Shutter Speed Value"] = "快门速度";
                _TagNames["F-Number"] = "光圈";
                _TagNames["ISO Speed Ratings"] = "ISO";
                _TagNames["Exposure Bias Value"] = "曝光补偿";
                _TagNames["Focal Length"] = "焦距";
                _TagNames["Exposure Program"] = "曝光程序";
                _TagNames["Metering Mode"] = "测光模式";
                _TagNames["Flash Mode"] = "闪光灯";
                _TagNames["White Balance Mode"] = "白平衡";
                _TagNames["Exposure Mode"] = "曝光模式";
                _TagNames["Continuous Drive Mode"] = "驱动模式";
                _TagNames["Focus Mode"] = "对焦模式";
            }

            if (_TagNames.ContainsKey(str))
            {
                return _TagNames[str];
            }
            return "";
        }
        #endregion
    }
}

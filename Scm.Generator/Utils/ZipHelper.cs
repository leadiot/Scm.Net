using Com.Scm.Generator.Config;
using System.IO.Compression;

namespace Com.Scm.Utils
{
    public class ZipHelper : GenHelper
    {
        private GeneratorConfig _Config;
        private MemoryStream _Stream;
        private ZipArchive _Archive;
        private byte[] _Bytes;

        public override void Prepare(GeneratorConfig config)
        {
            _Config = config;
        }

        public override void SaveFile(string path, string file, string content)
        {
            if (_Config.GenFiles)
            {
                var tmp = Path.Combine(_Config.GeneratorDir, path);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                tmp = Path.Combine(tmp, file);
                using (var writer = new StreamWriter(tmp))
                {
                    writer.Write(content);
                }
            }

            if (_Config.Download)
            {
                if (_Stream == null)
                {
                    _Stream = new MemoryStream();
                }
                if (_Archive == null)
                {
                    _Archive = new ZipArchive(_Stream, ZipArchiveMode.Create, true);
                }

                var entry = _Archive.CreateEntry(path + "/" + file);
                using (var writer = new StreamWriter(entry.Open()))
                {
                    writer.Write(content);
                }
            }
        }

        public override void Close()
        {
            if (_Archive != null)
            {
                _Archive.Dispose();
            }

            if (_Stream != null)
            {
                _Stream.Flush();
                _Stream.Close();
                _Bytes = _Stream.ToArray();
            }
        }

        public override byte[] Bytes
        {
            get
            {
                return _Bytes;
            }
        }
    }
}

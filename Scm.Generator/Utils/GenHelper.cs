using Com.Scm.Generator.Config;

namespace Com.Scm.Utils
{
    public abstract class GenHelper
    {
        public abstract void Prepare(GeneratorConfig config);

        public abstract void SaveFile(string path, string file, string content);

        public abstract void Close();

        public abstract byte[] Bytes { get; }
    }
}

using ID3;

namespace Com.Scm.Audio
{
    public class AudioUtil
    {
        public static ID3Info GetID3Info(string file)
        {
            var id3Info = new ID3Info(file, false);
            id3Info.Load();
            return id3Info;
        }
    }
}

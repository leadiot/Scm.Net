namespace Com.Scm.Plugin.Image
{
    public class ImageColor
    {
        /// <summary>
        /// 
        /// </summary>
        public int Color { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Amount { get; private set; }

        public ImageColor(int Color, int Amount)
        {
            this.Color = Color;
            this.Amount = Amount;
        }
    }
}

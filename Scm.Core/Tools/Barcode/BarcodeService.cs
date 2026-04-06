using Com.Scm.Dvo;
using Com.Scm.Filters;
using Com.Scm.Image.Barcode;
using Com.Scm.Image.SkiaSharp;
using Com.Scm.Plugin.Image;
using Com.Scm.Service;
using Com.Scm.Tools.Barcode.Dvo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Tools.Barcode
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class BarcodeService : ApiService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<BarcodeInfo> GetOption()
        {
            return ImageEngine.GetBarcodeInstance().Options;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ResOptionDvo> GetFonts()
        {
            var fonts = GetFonts1();
            var list = new List<ResOptionDvo>();
            for (var i = 0; i < fonts.Length; i++)
            {
                list.Add(new ResOptionDvo() { id = i + 1, label = fonts[i], value = i + 1 });
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous, NoJsonResult]
        public IActionResult GetImageAsync(CreateRequest request)
        {
            var text = request.text;
            if (string.IsNullOrEmpty(text))
            {
                text = "12345678";
            }

            var code = text;
            var idx = code.IndexOf('/');
            if (idx >= 0)
            {
                code = code.Substring(0, idx);
            }

            var fonts = GetFonts1();
            var fontName = fonts[request.fontName];
            if (string.IsNullOrEmpty(fontName))
            {
                fontName = "Arial Black";
            }
            if (request.fontSize < 8)
            {
                request.fontSize = 8;
            }
            if (request.position <= 0)
            {
                request.position = PositionEnum.BottomCenter;
            }

            var factory = new ImageEngine();
            var image = (ScmImage)factory.GenBarcode(code, request.format, request.width, request.height);
            if (request.position != PositionEnum.Hidden)
            {
                image = (ScmImage)factory.GenBarcode(image, text, request.position, fontName, request.fontSize);
            }

            var frame = (ScmFrame)image.GetFirstFrame();
            return new FileContentResult(frame.ToBytes(ScmImageFormat.Png), "image/png");
        }

        private string[] GetFonts1()
        {
            return new string[] { "","Agency FB", "Algerian", "Arial", "Arial Black", "Arial Narrow", "Arial Rounded MT Bold", "Bahnschrift",
                "Bahnschrift Condensed", "Bahnschrift Light", "Bahnschrift Light Condensed", "Bahnschrift Light SemiCondensed", "Bahnschrift SemiBold",
                "Bahnschrift SemiBold Condensed", "Bahnschrift SemiBold SemiConden", "Bahnschrift SemiCondensed", "Bahnschrift SemiLight",
                "Bahnschrift SemiLight Condensed", "Bahnschrift SemiLight SemiConde", "Baskerville Old Face", "Bauhaus 93", "Bell MT", "Berlin Sans FB",
                "Berlin Sans FB Demi", "Bernard MT Condensed", "Blackadder ITC", "Bodoni MT", "Bodoni MT Black", "Bodoni MT Condensed",
                "Bodoni MT Poster Compressed", "Book Antiqua", "Bookman Old Style", "Bookshelf Symbol 7", "Bradley Hand ITC", "Britannic Bold", "Broadway",
                "Brush Script MT", "Calibri", "Calibri Light", "Californian FB", "Calisto MT", "Cambria", "Cambria Math", "Candara", "Candara Light",
                "Cascadia Code", "Cascadia Code ExtraLight", "Cascadia Code Light", "Cascadia Code SemiBold", "Cascadia Code SemiLight", "Cascadia Mono",
                "Cascadia Mono ExtraLight", "Cascadia Mono Light", "Cascadia Mono SemiBold", "Cascadia Mono SemiLight", "Castellar", "Centaur", "Century",
                "Century Gothic", "Century Schoolbook", "Chiller", "Colonna MT", "Comic Sans MS", "Consolas", "Constantia", "Cooper Black",
                "Copperplate Gothic Bold", "Copperplate Gothic Light", "Corbel", "Corbel Light", "Courier New", "Curlz MT", "Dubai", "Dubai Light",
                "Dubai Medium", "Ebrima", "Edwardian Script ITC", "Elephant", "Engravers MT", "Eras Bold ITC", "Eras Demi ITC", "Eras Light ITC",
                "Eras Medium ITC", "Felix Titling", "Footlight MT Light", "Forte", "Franklin Gothic Book", "Franklin Gothic Demi", "Franklin Gothic Demi Cond",
                "Franklin Gothic Heavy", "Franklin Gothic Medium", "Franklin Gothic Medium Cond", "Freestyle Script", "French Script MT", "Gabriola", "Gadugi",
                "Garamond", "Georgia", "Gigi", "Gill Sans MT", "Gill Sans MT Condensed", "Gill Sans MT Ext Condensed Bold", "Gill Sans Ultra Bold",
                "Gill Sans Ultra Bold Condensed", "Gloucester MT Extra Condensed", "Goudy Old Style", "Goudy Stout", "Haettenschweiler", "Harlow Solid Italic",
                "Harrington", "High Tower Text", "HoloLens MDL2 Assets", "Impact", "Imprint MT Shadow", "Informal Roman", "Ink Free", "Javanese Text", "Jokerman",
                "Juice ITC", "Kristen ITC", "Kunstler Script", "Leelawadee UI", "Leelawadee UI Semilight", "Lucida Bright", "Lucida Calligraphy", "Lucida Console",
                "Lucida Fax", "Lucida Handwriting", "Lucida Sans", "Lucida Sans Typewriter", "Lucida Sans Unicode", "Magneto", "Maiandra GD", "Malgun Gothic",
                "Malgun Gothic Semilight", "Marlett", "Matura MT Script Capitals", "Microsoft Himalaya", "Microsoft JhengHei UI", "Microsoft JhengHei UI Light",
                "Microsoft New Tai Lue", "Microsoft PhagsPa", "Microsoft Sans Serif", "Microsoft Tai Le", "Microsoft YaHei UI", "Microsoft YaHei UI Light",
                "Microsoft Yi Baiti", "Mistral", "Modern No. 20", "Mongolian Baiti", "Monotype Corsiva", "MS Gothic", "MS Outlook", "MS PGothic", "MS Reference Sans Serif",
                "MS Reference Specialty", "MS UI Gothic", "MT Extra", "MV Boli", "Myanmar Text", "Niagara Engraved", "Niagara Solid", "Nirmala UI", "Nirmala UI Semilight",
                "OCR A Extended", "Old English Text MT", "Onyx", "Palace Script MT", "Palatino Linotype", "Papyrus", "Parchment", "Perpetua", "Perpetua Titling MT", "Playbill",
                "Poor Richard", "Pristina", "Rage Italic", "Ravie", "Rockwell", "Rockwell Condensed", "Rockwell Extra Bold", "Sans Serif Collection", "Script MT Bold",
                "Segoe Fluent Icons", "Segoe MDL2 Assets", "Segoe Print", "Segoe Script", "Segoe UI", "Segoe UI Black", "Segoe UI Emoji", "Segoe UI Historic", "Segoe UI Light",
                "Segoe UI Semibold", "Segoe UI Semilight", "Segoe UI Symbol", "Segoe UI Variable Display", "Segoe UI Variable Display Light", "Segoe UI Variable Display Semib",
                "Segoe UI Variable Display Semil", "Segoe UI Variable Small", "Segoe UI Variable Small Light", "Segoe UI Variable Small Semibol", "Segoe UI Variable Small Semilig",
                "Segoe UI Variable Text", "Segoe UI Variable Text Light", "Segoe UI Variable Text Semibold", "Segoe UI Variable Text Semiligh", "Showcard Gothic", "SimSun-ExtB",
                "Sitka Banner", "Sitka Banner Semibold", "Sitka Display", "Sitka Display Semibold", "Sitka Heading", "Sitka Heading Semibold", "Sitka Small", "Sitka Small Semibold",
                "Sitka Subheading", "Sitka Subheading Semibold", "Sitka Text", "Sitka Text Semibold", "Snap ITC", "Stencil", "Sylfaen", "Symbol", "Tahoma", "Tempus Sans ITC",
                "Times New Roman", "Trebuchet MS", "Tw Cen MT", "Tw Cen MT Condensed", "Tw Cen MT Condensed Extra Bold", "Verdana", "Viner Hand ITC", "Vivaldi", "Vladimir Script",
                "Webdings", "Wide Latin", "Wingdings", "Wingdings 2", "Wingdings 3", "Yu Gothic", "Yu Gothic Light", "Yu Gothic Medium", "Yu Gothic UI", "Yu Gothic UI Light",
                "Yu Gothic UI Semibold", "Yu Gothic UI Semilight", "仿宋", "华文中宋", "华文仿宋", "华文宋体", "华文彩云", "华文新魏", "华文楷体", "华文琥珀", "华文细黑",
                "华文行楷", "华文隶书", "宋体", "幼圆", "微软雅黑", "微软雅黑 Light", "新宋体", "方正姚体", "方正舒体", "楷体", "等线", "等线 Light", "隶书", "黑体" };
        }
    }
}

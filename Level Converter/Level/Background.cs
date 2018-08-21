using Tilemap_Converter.Base;

namespace Tilemap_Converter
{
    public class Background : Layer
    {
        public Background() : base("background")
        {

        }

        public override void Order()
        {
            parent.InnerXml = "";
        }
    }
}

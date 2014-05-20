using Funq.Fast;
using Microsoft.Xna.Framework;
using MouseLib.Api;
using TextLib.Api;

namespace SharedLib.Mouse
{
    public static class LibContent
    {
        public static void SetContents()
        {
            SetTextContent();
            SetMouseContent();
        }

        private static void SetTextContent()
        {
            var textContent = DependencyInjection.Resolve<ITextContent>();
            textContent.Font = SharedArt.Arial;
            textContent.Size = 0.8f;
        }

        private static void SetMouseContent()
        {
            var mouseDrawer = DependencyInjection.Resolve<IMouseDrawer>();
            mouseDrawer.Texture = SharedArt.Icons;
            mouseDrawer.Source = new Rectangle(0, 0, 32, 32);
            mouseDrawer.Origin = Vector2.Zero;
        }
    }
}
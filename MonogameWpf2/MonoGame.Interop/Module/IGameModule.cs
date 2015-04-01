using System.Windows.Input;
using Microsoft.Xna.Framework;
using MonoGame.Interop.Arguments;

namespace MonoGame.Interop.Module
{
    public interface IGameModule
    {
        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw();

        // events
        void OnMouseMove(GameMouseEventArgs e);
        void OnMouseDown(GameMouseButtonEventArgs e);
        void OnKeyDown(KeyEventArgs e);
    }
}
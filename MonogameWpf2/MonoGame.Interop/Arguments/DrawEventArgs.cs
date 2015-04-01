using Microsoft.Xna.Framework.Graphics;
using MonoGame.Interop.Controls;

namespace MonoGame.Interop.Arguments
{
    /// <summary>
    /// Provides data for the Draw event.
    /// </summary>
    internal sealed class DrawEventArgs : GraphicsDeviceEventArgs
    {
        private readonly DrawingSurface _drawingSurface;
	    
        public DrawEventArgs(DrawingSurface drawingSurface, GraphicsDevice graphicsDevice)
			: base(graphicsDevice)
        {
	        _drawingSurface = drawingSurface;
        }

	    public void InvalidateSurface()
        {
            _drawingSurface.Invalidate();
        }
    }
}
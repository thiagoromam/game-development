using Microsoft.Xna.Framework;

namespace CharacterEditor.Character
{
    public class Part
    {
        public Vector2 Location;
        public float Rotation;
        public Vector2 Scaling;
        public int Index;
        public int Flip;

        public Part()
        {
            Index = -1;
            Scaling = Vector2.One;
        }
    }
}
using Microsoft.Xna.Framework;

namespace CharacterEditor.Character
{
    internal class Part
    {
        public Vector2 Location;
        public float Rotation;
        public Vector2 Scaling;
        public int Index;
        public int Flip;

        public Part()
        {
            Index = -1;
            Scaling = new Vector2(1f, 1f);
        }
    }
}
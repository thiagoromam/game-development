using System;
using Microsoft.Xna.Framework;

namespace NeonVectorShooter.Helpers
{
    public class ColorUtil
    {
        public static Color HsvToColor(float h, float s, float v)
        {
            if (h == 0 && s == 0)
                return new Color(v, v, v);

            var c = s * v;
            var x = c * (1 - Math.Abs(h % 2 - 1));
            var m = v - c;

            if (h < 1) return new Color(c + m, x + m, m);
            if (h < 2) return new Color(x + m, c + m, m);
            if (h < 3) return new Color(m, c + m, x + m);
            if (h < 4) return new Color(m, x + m, c + m);
            if (h < 5) return new Color(x + m, m, c + m);
            return new Color(c + m, m, x + m);
        }
    }
}
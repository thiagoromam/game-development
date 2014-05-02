using System;
using System.IO;
using MapEditor.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.MapClasses
{
    public class Map
    {
        private readonly SegmentDefinition[] _definitions;
        private readonly MapSegment[,] _segments;
        private readonly int _segmentsDimension0Length;
        private readonly int _segmentsDimension1Length;
        private readonly float[] _scales = { 0.75f, 1, 1.25f };
        private readonly Color[] _colors = { Color.Gray, Color.White, Color.DarkGray };

        public Map()
        {
            _definitions = new SegmentDefinition[512];
            _segments = new MapSegment[3, 64];
            _segmentsDimension0Length = _segments.GetLength(0);
            _segmentsDimension1Length = _segments.GetLength(1);

            ReadDefinitions();
        }

        public SegmentDefinition[] Definitions
        {
            get { return _definitions; }
        }
        public MapSegment[,] Segments
        {
            get { return _segments; }
        }

        public int AddSegment(int layer, int index)
        {
            for (var i = 0; i < _segmentsDimension1Length; i++)
            {
                if (_segments[layer, i] != null) continue;

                _segments[layer, i] = new MapSegment { Index = index };
                return i;
            }

            return -1;
        }

        private void ReadDefinitions()
        {
            var reader = new StreamReader(@"Content/maps.zdx");
            var currentTex = 0;
            var curDef = -1;
            var tRect = new Rectangle();

            reader.ReadLine();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                string[] split;
                // ReSharper disable PossibleNullReferenceException
                if (line.StartsWith("#"))
                {
                    if (line.StartsWith("#src"))
                    {
                        split = line.Split(' ');
                        if (split.Length > 1)
                            currentTex = split[1].ToInt() - 1;
                    }
                }
                else
                {
                    curDef++;
                    var name = line;
                    split = reader.ReadLine().Split(' ');

                    if (split.Length > 3)
                    {
                        tRect.X = split[0].ToInt();
                        tRect.Y = split[1].ToInt();
                        tRect.Width = split[2].ToInt() - tRect.X;
                        tRect.Height = split[3].ToInt() - tRect.Y;
                    }
                    else
                        Console.WriteLine("read fail: " + name);

                    var flags = Convert.ToInt32(reader.ReadLine());
                    _definitions[curDef] = new SegmentDefinition(name, currentTex, tRect, flags);
                }
                // ReSharper restore PossibleNullReferenceException
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 scroll)
        {
            var destination = new Rectangle();

            spriteBatch.Begin();

            for (var l = 0; l < _segmentsDimension0Length; l++)
            {
                var scale = _scales[l];

                scale *= 0.5f;

                for (var i = 0; i < _segmentsDimension1Length; i++)
                {
                    var segment = _segments[l, i];
                    if (segment == null) continue;

                    var definition = _definitions[segment.Index];
                    destination.X = (int)(segment.Location.X - scroll.X * scale);
                    destination.Y = (int)(segment.Location.Y - scroll.Y * scale);
                    destination.Width = (int)(definition.Source.Width * scale);
                    destination.Height = (int)(definition.Source.Height * scale);

                    spriteBatch.Draw(Art.Maps[definition.Index], destination, definition.Source, _colors[l]);
                }
            }

            spriteBatch.End();
        }

        public int GetHoveredSegment(int layer, Vector2 scroll, Vector2 position)
        {
            var scale = _scales[layer];

            for (var i = _segmentsDimension1Length - 1; i >= 0; i--)
            {
                var segment = Segments[layer, i];
                if (segment == null) continue;

                var definition = Definitions[segment.Index];
                var destination = new Rectangle(
                    (int)(segment.Location.X - scroll.X * scale),
                    (int)(segment.Location.Y - scroll.Y * scale),
                    (int)(definition.Source.Width * scale),
                    (int)(definition.Source.Height * scale)
                );

                if (destination.Contains(position))
                    return i;
            }

            return -1;
        }
    }
}
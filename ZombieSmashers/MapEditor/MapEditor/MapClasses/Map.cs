using System;
using System.IO;
using MapEditor.Helpers;
using MapEditor.Ioc.Api.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.MapClasses
{
    public class Map : IReadonlyMapData, IMapComponent
    {
        private MapSegment[,] _segments;
        private int[,] _grid;
        private readonly float[] _scales = { 0.75f, 1, 1.25f };
        private readonly Color[] _colors = { Color.Gray, Color.White, Color.DarkGray };

        public Map()
        {
            Definitions = new SegmentDefinition[512];
            Segments = new MapSegment[3, 64];
            Grid = new int[20, 20];
            Ledges = new Ledge[16];

            for (var i = 0; i < Ledges.Length; i++)
                Ledges[i] = new Ledge();

            ReadDefinitions();
        }

        public SegmentDefinition[] Definitions { get; set; }
        public MapSegment[,] Segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                MaxSegmentsDimension0Index = _segments.GetLength(0) - 1;
                MaxSegmentsDimension1Index = _segments.GetLength(1) - 1;
            }
        }
        public int[,] Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
                MaxGridDimension0Index = _grid.GetLength(0) - 1;
                MaxGridDimension1Index = _grid.GetLength(1) - 1;
            }
        }
        public Ledge[] Ledges { get; set; }
        public int MaxGridDimension0Index { get; private set; }
        public int MaxGridDimension1Index { get; private set; }
        public int MaxSegmentsDimension0Index { get; private set; }
        public int MaxSegmentsDimension1Index { get; private set; }

        public int AddSegment(int layer, int index)
        {
            for (var i = 0; i <= MaxSegmentsDimension1Index; i++)
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
                    Definitions[curDef] = new SegmentDefinition(name, currentTex, tRect, flags);
                }
                // ReSharper restore PossibleNullReferenceException
            }

            reader.Close();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 scroll)
        {
            var destination = new Rectangle();

            spriteBatch.Begin();

            for (var l = 0; l <= MaxSegmentsDimension0Index; l++)
            {
                var scale = _scales[l];

                scale *= 0.5f;

                for (var i = 0; i <= MaxSegmentsDimension1Index; i++)
                {
                    var segment = _segments[l, i];
                    if (segment == null) continue;

                    var definition = Definitions[segment.Index];
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

            for (var i = MaxSegmentsDimension1Index; i >= 0; i--)
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
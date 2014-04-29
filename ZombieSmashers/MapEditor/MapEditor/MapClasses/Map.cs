using System;
using System.IO;
using MapEditor.Helpers;
using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses
{
    public class Map
    {
        private readonly SegmentDefinition[] _segments;

        public Map()
        {
            _segments = new SegmentDefinition[512];
            ReadSegments();
        }

        public SegmentDefinition[] Segments
        {
            get { return _segments; }
        }

        private void ReadSegments()
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
                if (line.StartsWith("#"))
                {
                    if (line.StartsWith("#src"))
                    {
                        split = line.Split(' ');
                        if (split.Length > 1)
                            currentTex = Convert.ToInt32(split[1]) - 1;
                    }
                }
                else
                {
                    curDef++;
                    var name = line;
                    split = reader.ReadLine().Split(' ');

                    if (split.Length > 3)
                    {
                        tRect.X = Convert.ToInt32(split[0]);
                        tRect.Y = Convert.ToInt32(split[1]);
                        tRect.Width = Convert.ToInt32(split[2]) - tRect.X;
                        tRect.Height = Convert.ToInt32(split[3]) - tRect.Y;
                    }
                    else 
                        Console.WriteLine("read fail: " + name);
                    
                    var flags = Convert.ToInt32(reader.ReadLine());
                    _segments[curDef] = new SegmentDefinition(name, currentTex, tRect, flags);
                }
            }
        }
    }
}
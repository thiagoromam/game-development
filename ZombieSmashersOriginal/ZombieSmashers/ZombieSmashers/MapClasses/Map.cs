using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombieSmashers.CharClasses;
using ZombieSmashers.Particles;

// ReSharper disable ForCanBeConvertedToForeach

namespace ZombieSmashers.MapClasses
{
    public class Map
    {
        private const int LayerBack = 0;
        private const int LayerMap = 1;

        public MapScript MapScript;
        public MapFlags GlobalFlags;
        public bool Fog;
        public Bucket Bucket;
        protected float PFrame;
        protected float Frame;

        public float TransInFrame = 0f;
        public float TransOutFrame = 0f;
        public string[] TransitionDestination = { "", "", "" };
        public TransitionDirection TransDir;
        private const int XSize = 20;
        private const int YSize = 20;

        public Map(string path)
        {
            GlobalFlags = new MapFlags(64);
            SegmentDefinitions = new SegmentDefinition[512];
            Segments = new MapSegment[3, 64];
            Grid = new int[XSize, YSize];
            Path = path;

            Ledges = new Ledge[16];

            for (var i = 0; i < 16; i++)
                Ledges[i] = new Ledge();

            ReadSegmentDefinitions();

            Read();
        }

        public Ledge[] Ledges { get; private set; }
        public int[,] Grid { get; private set; }
        public MapSegment[,] Segments { get; private set; }
        public SegmentDefinition[] SegmentDefinitions { get; private set; }
        public string Path { get; set; }

        public void Draw(SpriteBatch sprite, Texture2D[] mapsTex, Texture2D[] mapsBackTex, int startLayer, int endLayer)
        {
            var dRect = new Rectangle();

            sprite.Begin();

            if (startLayer == LayerBack)
            {
                var lim = new Vector2(GetXLim(), GetYLim());
                var targ = Game1.ScreenSize / 2 - (Game1.Scroll / lim - new Vector2(0.5f)) * 100;

                sprite.Draw(mapsBackTex[0], targ, new Rectangle(0, 0, 1280, 720), Color.White, 0, new Vector2(640, 360),
                    1, SpriteEffects.None, 1);
            }

            for (var l = startLayer; l < endLayer; l++)
            {
                var scale = 1.0f;

                var color = Color.White;

                if (l == 0)
                {
                    color = Color.Gray;
                    scale = 0.75f;
                }
                else if (l == 2)
                {
                    color = Color.DarkGray;
                    scale = 1.25f;
                }

                for (var i = 0; i < 64; i++)
                {
                    if (Segments[l, i] != null)
                    {
                        var sRect = SegmentDefinitions[Segments[l, i].Index].SourceRect;

                        dRect.X = (int)(Segments[l, i].Location.X * 2 - Game1.Scroll.X * scale);
                        dRect.Y = (int)(Segments[l, i].Location.Y * 2 - Game1.Scroll.Y * scale);
                        dRect.Width = (int)(sRect.Width * scale);
                        dRect.Height = (int)(sRect.Height * scale);

                        sprite.Draw(mapsTex[SegmentDefinitions[Segments[l, i].Index].SourceIndex], dRect, sRect, color);
                    }
                }
            }

            sprite.End();
        }

        public void Read()
        {
            var file = new BinaryReader(File.Open(@"data/maps/" + Path + ".zmx", FileMode.Open));

            for (var i = 0; i < Ledges.Length; i++)
            {
                Ledges[i] = new Ledge { TotalNodes = file.ReadInt32() };
                for (var n = 0; n < Ledges[i].TotalNodes; n++)
                {
                    Ledges[i].Nodes[n] = new Vector2(file.ReadSingle() * 2, file.ReadSingle() * 2);
                }
                Ledges[i].Flags = file.ReadInt32();
            }

            for (var l = 0; l < 3; l++)
            {
                for (var i = 0; i < 64; i++)
                {
                    var t = file.ReadInt32();

                    if (t == -1)
                        Segments[l, i] = null;
                    else
                    {
                        Segments[l, i] = new MapSegment
                        {
                            Index = t,
                            Location = new Vector2(file.ReadSingle(), file.ReadSingle())
                        };
                    }
                }
            }

            for (var x = 0; x < XSize; x++)
            {
                for (var y = 0; y < YSize; y++)
                {
                    Grid[x, y] = file.ReadInt32();
                }
            }

            MapScript = new MapScript(this);

            for (var i = 0; i < MapScript.Lines.Length; i++)
            {
                var s = file.ReadString();
                if (s.Length > 0)
                    MapScript.Lines[i] = new MapScriptLine(s);
                else
                    MapScript.Lines[i] = null;
            }

            file.Close();

            Fog = false;
            if (MapScript.GotoTag("init"))
                MapScript.IsReading = true;
        }

        private void ReadSegmentDefinitions()
        {
            var s = new StreamReader(@"Content/maps.zdx");

            var currentTex = 0;
            var curDef = -1;

            var tRect = new Rectangle();

            s.ReadLine();

            while (!s.EndOfStream)
            {
                // ReSharper disable PossibleNullReferenceException
                var t = s.ReadLine();

                string[] split;
                if (t.StartsWith("#"))
                {
                    if (t.StartsWith("#src"))
                    {
                        split = t.Split(' ');
                        if (split.Length > 1)
                        {
                            var n = Convert.ToInt32(split[1]);
                            currentTex = n - 1;
                        }
                    }
                }
                else
                {
                    curDef++;

                    var name = t;

                    t = s.ReadLine();
                    split = t.Split(' ');

                    if (split.Length > 3)
                    {
                        tRect.X = Convert.ToInt32(split[0]);
                        tRect.Y = Convert.ToInt32(split[1]);
                        tRect.Width = Convert.ToInt32(split[2]) - tRect.X;
                        tRect.Height = Convert.ToInt32(split[3]) - tRect.Y;
                    }
                    else
                        Console.WriteLine("read fail: " + name);

                    var tex = currentTex;

                    t = s.ReadLine();
                    var flags = Convert.ToInt32(t);

                    SegmentDefinitions[curDef] = new SegmentDefinition(name, tex, tRect, flags);
                }
                // ReSharper restore PossibleNullReferenceException
            }
        }

        public int AddSeg(int layer, int index)
        {
            for (var i = 0; i < 64; i++)
            {
                if (Segments[layer, i] == null)
                {
                    Segments[layer, i] = new MapSegment { Index = index };

                    return i;
                }
            }

            return -1;
        }

        public int GetHoveredSegment(int x, int y, int l, Vector2 scroll)
        {
            var scale = 1.0f;
            if (l == 0)
                scale = 0.75f;
            else if (l == 2)
                scale = 1.25f;

            scale *= 0.5f;

            for (var i = 63; i >= 0; i--)
            {
                if (Segments[l, i] != null)
                {
                    var sRect = SegmentDefinitions[Segments[l, i].Index].SourceRect;
                    var dRect = new Rectangle(
                        (int)(Segments[l, i].Location.X - scroll.X * scale),
                        (int)(Segments[l, i].Location.Y - scroll.Y * scale),
                        (int)(sRect.Width * scale),
                        (int)(sRect.Height * scale)
                        );

                    if (dRect.Contains(x, y))
                        return i;
                }
            }

            return -1;
        }

        public int GetLedgeSec(int l, float x)
        {
            var ledge = Ledges[l];

            for (var i = 0; i < ledge.TotalNodes - 1; i++)
            {
                if (x >= ledge.Nodes[i].X && x <= ledge.Nodes[i + 1].X)
                    return i;
            }

            return -1;
        }

        public float GetLedgeYLoc(int l, int i, float x)
        {
            var ledge = Ledges[l];
            var node1 = ledge.Nodes[i];
            var node2 = ledge.Nodes[i + 1];

            return (node2.Y - node1.Y) * ((x - node1.X) / (node2.X - node1.X)) + node1.Y;
        }

        public bool CheckCol(Vector2 loc)
        {
            if (loc.X < 0 || loc.Y < 0)
                return true;

            var x = (int)loc.X / 64;
            var y = (int)loc.Y / 64;

            if (x >= 0 && y >= 0 && x < XSize && y < YSize)
            {
                if (Grid[x, y] == 0)
                    return false;
            }

            return true;
        }
        public bool CheckParticleCol(Vector2 loc)
        {
            if (CheckCol(loc))
                return true;

            for (int i = 0; i < 16; i++)
            {
                if (Ledges[i].TotalNodes > 1)
                {
                    if (Ledges[i].Flags == (int)LedgeFlags.Solid)
                    {
                        int s = GetLedgeSec(i, loc.X);
                        if (s > -1) if (GetLedgeYLoc(i, s, loc.X) < loc.Y)
                                return true;
                    }
                }
            }

            return false;
        }

        public void Update(ParticleManager pMan, Character[] c)
        {
            CheckTransitions(c);
            if (TransOutFrame > 0f)
            {
                TransOutFrame -= Game1.FrameTime * 3f;
                if (TransOutFrame <= 0f)
                {
                    Path = TransitionDestination[(int) TransDir];
                    Read();
                    TransInFrame = 1.1f;
                    
                    for (var i = 1; i < c.Length; i++)
                        c[i] = null;
                    
                    pMan.Reset();
                }
            }
            if (TransInFrame > 0f)
            {
                TransInFrame -= Game1.FrameTime * 3f;
            }

            if (MapScript.IsReading)
                MapScript.DoScript(c);

            if (Bucket != null)
            {
                if (!Bucket.IsEmpty)
                    Bucket.Update(c);
            }

            Frame += Game1.FrameTime;

            if (Fog)
            {
                if ((int)(PFrame * 10f) != (int)(Frame * 10f))
                    pMan.AddParticle(new Fog(Rand.GetRandomVector2(0f, 1280f, 600f, 1000f)));
            }

            for (var i = 0; i < 64; i++)
            {
                if (Segments[LayerMap, i] != null)
                {
                    if (SegmentDefinitions[Segments[LayerMap, i].Index].Flags == (int)SegmentFlags.Torch)
                    {
                        pMan.AddParticle(
                            new Smoke(
                                Segments[LayerMap, i].Location * 2f + new Vector2(20f, 13f),
                                Rand.GetRandomVector2(-50.0f, 50.0f, -300.0f, -200.0f),
                                1.0f, 0.8f, 0.6f, 1.0f,
                                Rand.GetRandomFloat(0.25f, 0.5f), Rand.GetRandomInt(0, 4)
                            ),
                            true
                        );
                        pMan.AddParticle(
                            new Fire(
                                Segments[LayerMap, i].Location * 2f + new Vector2(20f, 37f),
                                Rand.GetRandomVector2(-30.0f, 30.0f, -250.0f, -200.0f),
                                Rand.GetRandomFloat(0.25f, 0.75f),
                                Rand.GetRandomInt(0, 4)
                            ),
                            true
                        );
                    }
                }
            }
        }

        public float GetXLim()
        {
            return 1280 - Game1.ScreenSize.X;
        }

        public float GetYLim()
        {
            return 1280 - Game1.ScreenSize.Y;
        }

        public float GetTransVal()
        {
            if (TransInFrame > 0f)
                return TransInFrame;

            if (TransOutFrame > 0f)
                return 1 - TransOutFrame;

            return 0f;
        }

        public void CheckTransitions(Character[] c)
        {
            if (TransOutFrame <= 0f && TransInFrame <= 0f)
            {
                if (c[0].DyingFrame > 0f)
                    return;

                if (c[0].Location.X > XSize * 64f - 32f && c[0].Trajectory.X > 0f)
                {
                    if (TransitionDestination[(int)TransitionDirection.Right] != "")
                    {
                        TransOutFrame = 1f;
                        TransDir = TransitionDirection.Right;
                    }
                }
                if (c[0].Location.X < 64f + 16f && c[0].Trajectory.X < 0f)
                {
                    if (TransitionDestination[(int)TransitionDirection.Left] != "")
                    {
                        TransOutFrame = 1f;
                        TransDir = TransitionDirection.Left;
                    }
                }
            }
        }

    }
}
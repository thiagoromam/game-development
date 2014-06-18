using ZombieSmashers.CharClasses;

namespace ZombieSmashers.MapClasses
{
    public class MapScript
    {
        private readonly Map _map;
        private int _curLine;
        private float _waiting;
        public bool IsReading;
        public MapFlags Flags;
        public MapScriptLine[] Lines;

        public MapScript(Map map)
        {
            this._map = map;
            Flags = new MapFlags(32);
            Lines = new MapScriptLine[128];
        }

        public void DoScript(Character[] c)
        {
            if (_waiting > 0f)
            {
                _waiting -= Game1.FrameTime;
            }
            else
            {
                var done = false;
                while (!done)
                {
                    _curLine++;
                    if (Lines[_curLine] != null)
                    {
                        switch (Lines[_curLine].Command)
                        {
                            case MapCommands.Fog:
                                _map.Fog = true;
                                break;
                            case MapCommands.Monster:
                                for (var i = 0; i < c.Length; i++)
                                {
                                    if (c[i] == null)
                                    {
                                        c[i] = new Character(Lines[_curLine].VParam,
                                            Game1.CharDefs[GetMonsterFromString(Lines[_curLine].SParam[1])], i,
                                            Character.TeamBadGuys) { Map = _map };

                                        if (Lines[_curLine].SParam.Length > 4)
                                            c[i].Name = Lines[_curLine].SParam[4];
                                        break;
                                    }
                                }
                                break;
                            case MapCommands.MakeBucket:
                                _map.Bucket = new Bucket(Lines[_curLine].IParam, _map);
                                break;
                            case MapCommands.AddBucket:
                                _map.Bucket.AddItem(Lines[_curLine].VParam, GetMonsterFromString(Lines[_curLine].SParam[1]));
                                break;
                            case MapCommands.IfNotBucketGoto:
                                if (_map.Bucket.IsEmpty) GotoTag(Lines[_curLine].SParam[1]);
                                break;
                            case MapCommands.Wait:
                                _waiting = Lines[_curLine].IParam / 100f;
                                done = true;
                                break;
                            case MapCommands.IfTrueGoto:
                                if (Flags.GetFlag(Lines[_curLine].SParam[1])) GotoTag(Lines[_curLine].SParam[2]);
                                break;
                            case MapCommands.IfFalseGoto:
                                if (!Flags.GetFlag(Lines[_curLine].SParam[1])) GotoTag(Lines[_curLine].SParam[2]);
                                break;
                            case MapCommands.SetGlobalFlag:
                                _map.GlobalFlags.SetFlag(Lines[_curLine].SParam[1]);
                                break;
                            case MapCommands.IfGlobalTrueGoto:
                                if (_map.GlobalFlags.GetFlag(Lines[_curLine].SParam[1]))
                                    GotoTag(Lines[_curLine].SParam[2]);
                                break;
                            case MapCommands.IfGlobalFalseGoto:
                                if (!_map.GlobalFlags.GetFlag(Lines[_curLine].SParam[1]))
                                    GotoTag(Lines[_curLine].SParam[2]);
                                break;
                            case MapCommands.Stop:
                                IsReading = false;
                                done = true;
                                break;
                            case MapCommands.Tag:
                                break;
                        }
                    }
                }
            }
        }

        public bool GotoTag(string tag)
        {
            for (var i = 0; i < Lines.Length; i++)
            {
                if (Lines[i] != null)
                {
                    if (Lines[i].Command == MapCommands.Tag)
                    {
                        if (Lines[i].SParam[1] == tag)
                        {
                            _curLine = i;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static int GetMonsterFromString(string m)
        {
            switch (m)
            {
                case "zombie":
                    return (int)CharacterType.Zombie;
            }

            return (int)CharacterType.Zombie;
        }
    }
}
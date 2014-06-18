using System;
using Microsoft.Xna.Framework;

namespace ZombieSmashers.MapClasses
{
    public class MapScriptLine
    {
        public MapCommands Command;
        // ReSharper disable once InconsistentNaming
        public int IParam;
        public Vector2 VParam;
        public string[] SParam;

        public MapScriptLine(string line)
        {
            if (line.Length < 1) return;
            SParam = line.Split(' ');
            switch (SParam[0])
            {
                case "fog":
                    Command = MapCommands.Fog;
                    break;
                case "monster":
                    Command = MapCommands.Monster;
                    VParam = new Vector2(Convert.ToSingle(SParam[2]), Convert.ToSingle(SParam[3]));
                    break;
                case "makebucket":
                    Command = MapCommands.MakeBucket;
                    IParam = Convert.ToInt32(SParam[1]);
                    break;
                case "addbucket":
                    Command = MapCommands.AddBucket;
                    VParam = new Vector2(Convert.ToSingle(SParam[2]), Convert.ToSingle(SParam[3]));
                    break;
                case "ifnotbucketgoto":
                    Command = MapCommands.IfNotBucketGoto;
                    break;
                case "wait":
                    Command = MapCommands.Wait;
                    IParam = Convert.ToInt32(SParam[1]);
                    break;
                case "setflag":
                    Command = MapCommands.SetFlag;
                    break;
                case "iftruegoto":
                    Command = MapCommands.IfTrueGoto;
                    break;
                case "iffalsegoto":
                    Command = MapCommands.IfFalseGoto;
                    break;
                case "setglobalflag":
                    Command = MapCommands.SetGlobalFlag;
                    break;
                case "ifglobaltruegoto":
                    Command = MapCommands.IfGlobalTrueGoto;
                    break;
                case "ifglobalfalsegoto":
                    Command = MapCommands.IfGlobalFalseGoto;
                    break;
                case "stop":
                    Command = MapCommands.Stop;
                    break;
                case "tag":
                    Command = MapCommands.Tag;
                    break;
            }
        }
    }
}
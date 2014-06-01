namespace CharacterEditor.Character
{
    internal class KeyFrame
    {
        public int FrameRef;
        public int Duration;

        private readonly string[] _scripts;

        public KeyFrame()
        {
            FrameRef = -1;
            Duration = 0;

            _scripts = new string[4];
            for (var i = 0; i < _scripts.Length; i++)
                _scripts[i] = string.Empty;
        }

        public string[] Scripts
        {
            get { return _scripts; }
        }
    }
}
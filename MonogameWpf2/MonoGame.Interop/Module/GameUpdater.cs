using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGame.Interop.Module
{
    internal class GameUpdater
    {
        private readonly Action<GameTime> _updateMethod;
        private readonly Action _requestDraw;
        private readonly TimeSpan _targetElapsedTime;
        private readonly TimeSpan _maxElapsedTime;
        private readonly GameTime _gameTime;
        private Stopwatch _gameTimer;
        private TimeSpan _accumulatedElapsedTime;
        private long _previousTicks;
        private int _updateFrameLag;
        private bool _runLoop;

        public GameUpdater(Action<GameTime> updateMethod, Action requestDraw)
        {
            _updateMethod = updateMethod;
            _requestDraw = requestDraw;
            _targetElapsedTime = TimeSpan.FromSeconds(1f / 60f);
            _maxElapsedTime = TimeSpan.FromMilliseconds(500);
            _gameTime = new GameTime();
        }

        public bool Drawing { get; set; }

        public void Start()
        {
            _runLoop = true;
            _gameTimer = Stopwatch.StartNew();

            Task.Factory.StartNew(RunLoop);
        }
        public void Finish()
        {
            _runLoop = false;
        }

        private void RunLoop()
        {
            while (_runLoop)
                Tick();
        }
        private void Tick()
        {
        RetryTick:

            var currentTicks = _gameTimer.Elapsed.Ticks;
            _accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - _previousTicks);
            _previousTicks = currentTicks;

            if (_accumulatedElapsedTime < _targetElapsedTime)
            {
                var sleepTime = (int)(_targetElapsedTime - _accumulatedElapsedTime).TotalMilliseconds;

                Thread.Sleep(sleepTime);
                goto RetryTick;
            }

            if (_accumulatedElapsedTime > _maxElapsedTime)
                _accumulatedElapsedTime = _maxElapsedTime;

            _gameTime.ElapsedGameTime = _targetElapsedTime;
            var stepCount = 0;

            // Perform as many full fixed length time steps as we can.
            while (_accumulatedElapsedTime >= _targetElapsedTime)
            {
                _gameTime.TotalGameTime += _targetElapsedTime;
                _accumulatedElapsedTime -= _targetElapsedTime;
                ++stepCount;

                _updateMethod(_gameTime);
            }

            _updateFrameLag += Math.Max(0, stepCount - 1);

            if (_gameTime.IsRunningSlowly)
            {
                if (_updateFrameLag == 0)
                    _gameTime.IsRunningSlowly = false;
            }
            else if (_updateFrameLag >= 5)
            {
                _gameTime.IsRunningSlowly = true;
            }

            if (stepCount == 1 && _updateFrameLag > 0)
                _updateFrameLag--;

            _gameTime.ElapsedGameTime = TimeSpan.FromTicks(_targetElapsedTime.Ticks * stepCount);

            while (Drawing) { }
            Drawing = true;
            _requestDraw();
        }
    }
}
namespace NeonVectorShooter.Helpers
{
    public class CircularArray<T> where T : new()
    {
        private readonly T[] _items;
        private int _start;

        public CircularArray(int capacity)
        {
            _items = new T[capacity];

            for (var i = 0; i < capacity; ++i)
                _items[i] = new T();
        }

        public int Start
        {
            get { return _start; }
            set { _start = value % _items.Length; }
        }
        public int Count { get; set; }
        public int Capacity
        {
            get { return _items.Length; }
        }
        public T this[int index]
        {
            get { return _items[(_start + index) % _items.Length]; }
            set { _items[(_start + index) % _items.Length] = value; }
        }
    }
}
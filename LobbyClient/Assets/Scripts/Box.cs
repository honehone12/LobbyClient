namespace Lobby
{
    public class Box<T>
    {
        public T value;

        public Box()
        {
            this.value = default;
        }

        public Box(T value)
        {
            this.value = value;
        }
    }
}

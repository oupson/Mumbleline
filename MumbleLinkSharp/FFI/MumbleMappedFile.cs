namespace Mumbleline.MumbleLink.FFI
{
    public interface IMumbleMappedFile<T> where T : ILinkedMem
    {
        T Read();
        void Write(ref T mem);
        void Tick();
    }
}

namespace Mumbleline.MumbleLink.FFI
{
    public interface MumbleMappedFile<T> where T : LinkedMem
    {
        T Read();
        void Write(T mem);
        void Tick();
    }
}

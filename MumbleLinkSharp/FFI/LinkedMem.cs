using MumbleLinkSharp.Data;

namespace MumbleLinkSharp.FFI
{
    public interface ILinkedMem
    {
        void Tick();
        void UpdateFrom(LinkInformations infos);
        LinkInformations ToInfos();
    }
}
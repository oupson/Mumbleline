using Mumbleline.MumbleLink.Data;

namespace Mumbleline.MumbleLink.FFI
{
    public interface ILinkedMem {
        void Tick();
        void UpdateFrom(LinkInformations infos);
        LinkInformations ToInfos();
    }
}
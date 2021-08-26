using Mumbleline.MumbleLink.Data;

namespace Mumbleline.MumbleLink.FFI
{
    public interface LinkedMem {
        void Tick();
        LinkedMem UpdateFrom(LinkInformations infos);
        LinkInformations ToInfos();
    }
}
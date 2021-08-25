using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumbleline.MumbleLink.FFI
{
    interface MumbleMappedFile
    {
        LinkedMem Read();
        void Write(LinkedMem mem);
        void Tick();
    }
}

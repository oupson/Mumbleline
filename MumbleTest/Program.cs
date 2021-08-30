using MumbleLinkSharp;
using MumbleLinkSharp.Data;
using System;

namespace MumbleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mumble = MumbleLink.GetNewInstance();

            mumble.WriteInfos(new LinkInformations
            {
                UiVersion = 1,
                UiTick = 1,
                Name = "TestMumbleSharpFoo",
                Context = "Connected",
                Description = "foo"
            });

            Console.ReadLine();

            mumble.Dispose();
        }
    }
}

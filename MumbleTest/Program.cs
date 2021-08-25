using Mumbleline.MumbleLink;
using System;

namespace MumbleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mumble = new MumbleLink();

            mumble.Write(new Mumbleline.MumbleLink.Data.LinkInformations
            {
                UiVersion = 1,
                UiTick = 1,
                Name = "TestMumbleSharp",
                Context = "Connected",
                Description = "foo"
            });


            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCommands.Tests
{
    internal class Tokens
    {
        internal static string Token { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf1.txt").ElementAt(0);
        internal static string Token2 { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf2.txt").ElementAt(0);
        internal static string Token3 { get; set; } = System.IO.File.ReadAllLines(@"E:\Dropbox\FluentCommands\botinf3.txt").ElementAt(0);
    }
}

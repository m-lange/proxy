using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PsProxy
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct INTERNET_PER_CONN_OPTION_LIST
    {
        public int Size;

        // The connection to be set. NULL means LAN.
        public System.IntPtr Connection;

        public int OptionCount;
        public int OptionError;

        // List of INTERNET_PER_CONN_OPTIONs.
        public System.IntPtr pOptions;
    }
}

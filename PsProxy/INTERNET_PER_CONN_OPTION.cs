using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PsProxy
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INTERNET_PER_CONN_OPTION
    {
        public const int INTERNET_PER_CONN_FLAGS = 1;
        public const int INTERNET_PER_CONN_PROXY_SERVER = 2;
        public const int INTERNET_PER_CONN_PROXY_BYPASS = 3;
        public const int INTERNET_PER_CONN_AUTOCONFIG_URL = 4;
        public const int INTERNET_PER_CONN_AUTODISCOVERY_FLAGS = 5;
        public const int INTERNET_PER_CONN_AUTOCONFIG_SECONDARY_URL = 6;
        public const int INTERNET_PER_CONN_AUTOCONFIG_RELOAD_DELAY_MINS = 7;
        public const int INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_TIME = 8;
        public const int INTERNET_PER_CONN_AUTOCONFIG_LAST_DETECT_URL = 9;
        public const int INTERNET_PER_CONN_FLAGS_UI = 10;

        public const int PROXY_TYPE_DIRECT = 0x00000001;           // direct to net
        public const int PROXY_TYPE_PROXY = 0x00000002;            // via named proxy
        public const int PROXY_TYPE_AUTO_PROXY_URL = 0x00000004;   // autoproxy URL
        public const int PROXY_TYPE_AUTO_DETECT = 0x00000008;      // use autoproxy detection

        public int dwOption;
        public __INTERNET_PER_CONN_OPTION Value;
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct __INTERNET_PER_CONN_OPTION
    {
        [FieldOffset(0)]
        public int dwValue;
        [FieldOffset(0)]
        public System.IntPtr pszValue;
        [FieldOffset(0)]
        public System.Runtime.InteropServices.ComTypes.FILETIME ftValue;
    }
}

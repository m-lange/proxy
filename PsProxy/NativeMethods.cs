using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PsProxy
{
    internal static class NativeMethods
    {
        /// <summary>
        /// Initialize an application's use of the WinINet functions.
        /// See 
        /// </summary>
        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr InternetOpen(
            string lpszAgent,
            int dwAccessType,
            string lpszProxyName,
            string lpszProxyBypass,
            int dwFlags);

        /// <summary>
        /// Close a single Internet handle.
        /// </summary>
        [DllImport("wininet.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InternetCloseHandle(IntPtr hInternet);

        /// <summary>
        /// Sets an Internet option.
        /// </summary>
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern bool InternetSetOption(
            IntPtr hInternet,
            INTERNET_OPTION dwOption,
            IntPtr lpBuffer,
            int lpdwBufferLength);

        /// <summary>
        /// Queries an Internet option on the specified handle. The Handle will be always 0.
        /// </summary>
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal extern static bool InternetQueryOption(
            IntPtr hInternet,
            INTERNET_OPTION dwOption,
            ref INTERNET_PER_CONN_OPTION_LIST OptionList,
            ref int lpdwBufferLength);
    }
}

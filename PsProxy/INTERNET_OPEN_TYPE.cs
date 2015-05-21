using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsProxy
{
    public enum INTERNET_OPEN_TYPE
    {
        /// <summary>
        /// Retrieves the proxy or direct configuration from the registry.
        /// </summary>
        INTERNET_OPEN_TYPE_PRECONFIG = 0,

        /// <summary>
        /// Resolves all host names locally.
        /// </summary>
        INTERNET_OPEN_TYPE_DIRECT = 1,

        /// <summary>
        /// Passes requests to the proxy unless a proxy bypass list is supplied and the name to be resolved bypasses the proxy.
        /// </summary>
        INTERNET_OPEN_TYPE_PROXY = 3,

        /// <summary>
        /// Retrieves the proxy or direct configuration from the registry and prevents
        /// the use of a startup Microsoft JScript or Internet Setup (INS) file.
        /// </summary>
        INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsProxy
{
    public enum INTERNET_OPTION
    {
        /// <summary>
        /// Sets or retrieves an INTERNET_PER_CONN_OPTION_LIST structure that specifies  
        /// a list of options for a particular connection.
        /// </summary>
        INTERNET_OPTION_PER_CONNECTION_OPTION = 75,

        /// <summary>
        /// Notify the system that the registry settings have been changed so that
        /// it verifies the settings on the next call to InternetConnect.
        /// </summary>
        INTERNET_OPTION_SETTINGS_CHANGED = 39,

        /// <summary>
        /// Causes the proxy data to be reread from the registry for a handle.
        /// </summary>
        INTERNET_OPTION_REFRESH = 37
    }
}

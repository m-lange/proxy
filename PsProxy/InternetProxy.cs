using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsProxy
{
    public enum Type
    {
        DIRECT,
        PROXY,
        AUTO_PROXY_URL,
        AUTO_DETECT,
        UNKNOWN
    }

    public class InternetProxy
    {
        public Type Type { get; set; }
        public string AutoConfigURL { get; set; }
        public string ProxyServer { get; set; }
        public string ProxyOverride { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is InternetProxy)
            {
                InternetProxy proxy = obj as InternetProxy;

                return this.Type.Equals(proxy.Type)
                    && this.AutoConfigURL.Equals(proxy.AutoConfigURL, System.StringComparison.OrdinalIgnoreCase)
                    && this.ProxyServer.EndsWith(proxy.ProxyServer, System.StringComparison.OrdinalIgnoreCase)
                    && this.ProxyOverride.Equals(proxy.ProxyOverride, System.StringComparison.Ordinal);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode()
                + this.AutoConfigURL.GetHashCode()
                + this.ProxyServer.GetHashCode()
                + this.ProxyOverride.GetHashCode();
        }
    }
}

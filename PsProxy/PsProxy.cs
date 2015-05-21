using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.ComponentModel;

namespace PsProxy
{
    [RunInstaller(true)]
    public class ProxyPSSnapIn : CustomPSSnapIn
    {
        public ProxyPSSnapIn() : base() { }

        public override string Name
        {
            get { return "ProxySnapin1.0"; }
        }

        public override string Vendor
        {
            get { return "Martin Lange"; }
        }

        public override string Description
        {
            get { return "This Windows PowerShell snap-in contains cmdlets used to configure the global HTTP proxy."; }
        }


        private Collection<CmdletConfigurationEntry> _cmdlets;
        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get
            {
                if (_cmdlets == null)
                {
                    _cmdlets = new Collection<CmdletConfigurationEntry>();
                    _cmdlets.Add(new CmdletConfigurationEntry("Get-Proxy", typeof(GetProxy), @".\PsProxy.dll-Help.xml"));
                    _cmdlets.Add(new CmdletConfigurationEntry("Set-Proxy", typeof(SetProxy), @".\PsProxy.dll-Help.xml"));
                }

                return _cmdlets;
            }
        }


        private Collection<ProviderConfigurationEntry> _providers;
        public override Collection<ProviderConfigurationEntry> Providers
        {
            get
            {
                if (_providers == null)
                {
                    _providers = new Collection<ProviderConfigurationEntry>();
                }

                return _providers;
            }
        }


        private Collection<TypeConfigurationEntry> _types;
        public override Collection<TypeConfigurationEntry> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new Collection<TypeConfigurationEntry>();
                }

                return _types;
            }
        }


        private Collection<FormatConfigurationEntry> _formats;
        public override Collection<FormatConfigurationEntry> Formats
        {
            get
            {
                if (_formats == null)
                {
                    _formats = new Collection<FormatConfigurationEntry>();
                    _formats.Add(new FormatConfigurationEntry(@".\PsProxy.Format.ps1xml"));
                }

                return _formats;
            }
        }
    }


    [Cmdlet(VerbsCommon.Get, "Proxy")]
    public class GetProxy : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            INTERNET_PER_CONN_OPTION[] options = new INTERNET_PER_CONN_OPTION[5];

            options[0] = new INTERNET_PER_CONN_OPTION();
            options[0].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_FLAGS;
            options[1] = new INTERNET_PER_CONN_OPTION();
            options[1].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_FLAGS_UI;
            options[2] = new INTERNET_PER_CONN_OPTION();
            options[2].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_AUTOCONFIG_URL;
            options[3] = new INTERNET_PER_CONN_OPTION();
            options[3].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_PROXY_SERVER;
            options[4] = new INTERNET_PER_CONN_OPTION();
            options[4].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_PROXY_BYPASS;

            System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(options[0])
                + Marshal.SizeOf(options[1])
                + Marshal.SizeOf(options[2])
                + Marshal.SizeOf(options[3])
                + Marshal.SizeOf(options[4]));

            System.IntPtr current = (System.IntPtr)buffer;

            for (int i = 0; i < options.Length; i++)
            {
                Marshal.StructureToPtr(options[i], current, false);
                current = (System.IntPtr)((int)current + Marshal.SizeOf(options[i]));
            }

            INTERNET_PER_CONN_OPTION_LIST request = new INTERNET_PER_CONN_OPTION_LIST();
            request.pOptions = buffer;
            request.Size = Marshal.SizeOf(request);
            request.Connection = IntPtr.Zero;
            request.OptionCount = options.Length;
            request.OptionError = 0;
            int size = Marshal.SizeOf(request);

            bool result = NativeMethods.InternetQueryOption(
                IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                ref request,
                ref size);

            if (!result)
            {
                throw new ApplicationException("Get System Internet Option Failed! ");
            }

            InternetProxy proxy = new InternetProxy
            {
                Type = PsProxy.Type.UNKNOWN,
                AutoConfigURL = string.Empty,
                ProxyServer = string.Empty,
                ProxyOverride = string.Empty
            };

            current = request.pOptions;
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = (INTERNET_PER_CONN_OPTION)Marshal.PtrToStructure(current, typeof(INTERNET_PER_CONN_OPTION));
                current = (System.IntPtr)((int)current + Marshal.SizeOf(options[i]));
            }

            int flags = options[0].Value.dwValue | options[1].Value.dwValue;

            if ((flags & INTERNET_PER_CONN_OPTION.PROXY_TYPE_DIRECT) == INTERNET_PER_CONN_OPTION.PROXY_TYPE_DIRECT)
                proxy.Type = Type.DIRECT;

            if ((flags & INTERNET_PER_CONN_OPTION.PROXY_TYPE_PROXY) == INTERNET_PER_CONN_OPTION.PROXY_TYPE_PROXY)
                proxy.Type = Type.PROXY;

            if ((flags & INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_PROXY_URL) == INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_PROXY_URL)
                proxy.Type = Type.AUTO_PROXY_URL;

            if ((flags & INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_DETECT) == INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_DETECT)
                proxy.Type = Type.AUTO_DETECT;

            proxy.AutoConfigURL = Marshal.PtrToStringAnsi(options[2].Value.pszValue);
            proxy.ProxyServer = Marshal.PtrToStringAnsi(options[3].Value.pszValue);
            proxy.ProxyOverride = Marshal.PtrToStringAnsi(options[4].Value.pszValue);

            WriteObject(proxy);

            Marshal.FreeCoTaskMem(buffer);
        }
    }


    [Cmdlet(VerbsCommon.Set, "Proxy", DefaultParameterSetName = "PROXY")]
    public class SetProxy : PSCmdlet
    {
        [Parameter(ParameterSetName = "PROXY",
            Position = 0,
            Mandatory = true)]
        public string ProxyServer { get; set; }

        [Parameter(ParameterSetName = "PROXY",
            Position = 1,
            Mandatory = false)]
        public string[] ProxyOverride { get; set; }

        [Parameter(ParameterSetName = "AUTO_PROXY_URL",
            Position = 0,
            Mandatory = true)]
        public string AutoConfigURL { get; set; }

        [Parameter(ParameterSetName = "DIRECT",
            Position = 0,
            Mandatory = true)]
        public SwitchParameter Direct { get; set; }

        [Parameter(ParameterSetName = "AUTO_DETECT",
            Position = 0,
            Mandatory = true)]
        public SwitchParameter AutoDetect { get; set; }

        [Parameter(ParameterSetName = "OBJECT",
            Position = 0,
            Mandatory = true)]
        public InternetProxy InputObject { get; set; }


        protected override void ProcessRecord()
        {
            INTERNET_PER_CONN_OPTION[] options = new INTERNET_PER_CONN_OPTION[4];

            options[0] = new INTERNET_PER_CONN_OPTION();
            options[0].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_FLAGS;
            options[1] = new INTERNET_PER_CONN_OPTION();
            options[1].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_AUTOCONFIG_URL;
            options[2] = new INTERNET_PER_CONN_OPTION();
            options[2].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_PROXY_SERVER;
            options[3] = new INTERNET_PER_CONN_OPTION();
            options[3].dwOption = INTERNET_PER_CONN_OPTION.INTERNET_PER_CONN_PROXY_BYPASS;

            if (Direct)
            {
                options[0].Value.dwValue = INTERNET_PER_CONN_OPTION.PROXY_TYPE_DIRECT;
                WriteVerbose("Direct connection");
            }
            else if (AutoDetect)
            {
                options[0].Value.dwValue = INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_DETECT;
                WriteVerbose("Automaticall detect settings");
            }
            else if (ProxyServer != null)
            {
                options[0].Value.dwValue = INTERNET_PER_CONN_OPTION.PROXY_TYPE_PROXY;
                WriteVerbose("Use a proxy server :  " + ProxyServer);
            }
            else if (AutoConfigURL != null)
            {
                options[0].Value.dwValue = INTERNET_PER_CONN_OPTION.PROXY_TYPE_AUTO_PROXY_URL;
                WriteVerbose("Use automatic configuration script :  " + AutoConfigURL);
            }

            options[1].Value.pszValue = Marshal.StringToHGlobalAnsi(AutoConfigURL == null ? "" : AutoConfigURL);
            options[2].Value.pszValue = Marshal.StringToHGlobalAnsi(ProxyServer == null ? "" : ProxyServer);

            if (ProxyOverride != null && ProxyOverride.Length > 0)
            {
                string value = "";
                foreach (String str in ProxyOverride)
                {
                    value += (value.Length > 0 ? ";" : "") + str;
                }
                value = value.Replace(",", ";");
                WriteVerbose("Bypass proxy for :  " + value);
                options[3].Value.pszValue = Marshal.StringToHGlobalAnsi(ProxyOverride == null ? "" : value);
            }

            System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(options[0])
                + Marshal.SizeOf(options[1])
                + Marshal.SizeOf(options[2])
                + Marshal.SizeOf(options[3]));

            System.IntPtr current = (System.IntPtr)buffer;

            for (int i = 0; i < options.Length; i++)
            {
                Marshal.StructureToPtr(options[i], current, false);
                current = (System.IntPtr)((int)current + Marshal.SizeOf(options[i]));
            }


            INTERNET_PER_CONN_OPTION_LIST request = new INTERNET_PER_CONN_OPTION_LIST();
            request.pOptions = buffer;
            request.Size = Marshal.SizeOf(request);
            request.Connection = IntPtr.Zero;
            request.OptionCount = options.Length;
            request.OptionError = 0;
            int size = Marshal.SizeOf(request);

            IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);
            Marshal.StructureToPtr(request, intptrStruct, true);

            bool result = NativeMethods.InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                intptrStruct,
                size);

            Marshal.FreeCoTaskMem(buffer);
            Marshal.FreeCoTaskMem(intptrStruct);

            if (!result)
            {
                throw new ApplicationException(" Set Internet Option Failed!");
            }

            NativeMethods.InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                IntPtr.Zero,
                0);

            NativeMethods.InternetSetOption(
                IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                IntPtr.Zero,
                0);
        }
    }
}

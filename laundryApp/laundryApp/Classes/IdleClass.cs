using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Runtime.InteropServices;
using System.Threading;

namespace laundryApp.Classes
{
    class IdleClass
    {

        //****************************************************


        [DllImport("user32.dll", SetLastError = false)]
        private static extern bool GetLastInputInfo(ref Lastinputinfo plii);
        public static readonly DateTime SystemStartup = DateTime.Now.AddMilliseconds(-Environment.TickCount);

        [StructLayout(LayoutKind.Sequential)]
        private struct Lastinputinfo
        {
            public uint cbSize;
            public readonly int dwTime;
        }

        public static DateTime LastInput => SystemStartup.AddMilliseconds(LastInputTicks);

        public static TimeSpan IdleTime => DateTime.Now.Subtract(LastInput);

       public static int LastInputTicks
        {
            get
            {
                var lii = new Lastinputinfo { cbSize = (uint)Marshal.SizeOf(typeof(Lastinputinfo)) };
                GetLastInputInfo(ref lii);
                return lii.dwTime;
            }
        }
       
    }
}

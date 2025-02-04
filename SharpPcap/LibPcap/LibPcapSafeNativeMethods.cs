/*
This file is part of SharpPcap.

SharpPcap is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharpPcap is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with SharpPcap.  If not, see <http://www.gnu.org/licenses/>.
*/
/* 
 * Copyright 2005 Tamir Gal <tamir@tamirgal.com>
 * Copyright 2008-2009 Phillip Lemon <lucidcomms@gmail.com>
 * Copyright 2008-2011 Chris Morgan <chmorgan@gmail.com>
 */

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using static SharpPcap.LibPcap.PcapUnmanagedStructures;

namespace SharpPcap.LibPcap
{
    /// <summary>
    /// Per http://msdn.microsoft.com/en-us/ms182161.aspx 
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static partial class LibPcapSafeNativeMethods
    {

        internal static int pcap_setbuff(PcapHandle /* pcap_t */ adapter, int bufferSizeInBytes)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? _pcap_setbuff(adapter, bufferSizeInBytes)
                : (int)PcapError.PlatformNotSupported;
        }
        internal static int pcap_setmintocopy(PcapHandle /* pcap_t */ adapter, int sizeInBytes)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? _pcap_setmintocopy(adapter, sizeInBytes)
                : (int)PcapError.PlatformNotSupported;
        }

        /// <summary>
        /// pcap_set_rfmon() sets whether monitor mode should be set on a capture handle when the handle is activated.
        /// If rfmon is non-zero, monitor mode will be set, otherwise it will not be set.  
        /// </summary>
        /// <param name="p">A <see cref="IntPtr"/></param>
        /// <param name="rfmon">A <see cref="int"/></param>
        /// <returns>Returns 0 on success or PCAP_ERROR_ACTIVATED if called on a capture handle that has been activated.</returns>
        internal static int pcap_set_rfmon(PcapHandle /* pcap_t* */ p, int rfmon)
        {
            try
            {
                return _pcap_set_rfmon(p, rfmon);
            }
            catch (EntryPointNotFoundException)
            {
                return (int)PcapError.RfmonNotSupported;
            }
        }

        #region Timestamp related functions

        private static readonly Version Libpcap_1_5 = new Version(1, 5, 0);
        /// <summary>
        /// Available since libpcap 1.5
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        internal static int pcap_set_tstamp_precision(PcapHandle /* pcap_t* p */ adapter, int precision)
        {
            if (Pcap.LibpcapVersion < Libpcap_1_5)
            {
                return (int)PcapError.TimestampPrecisionNotSupported;
            }
            return _pcap_set_tstamp_precision(adapter, precision);
        }

        /// <summary>
        /// Available since libpcap 1.5
        /// </summary>
        /// <param name="adapter"></param>
        internal static int pcap_get_tstamp_precision(PcapHandle /* pcap_t* p */ adapter)
        {
            if (Pcap.LibpcapVersion < Libpcap_1_5)
            {
                return (int)TimestampResolution.Microsecond;
            }
            return _pcap_get_tstamp_precision(adapter);
        }

        #endregion
    }
}

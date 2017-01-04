using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Print.Printing {

    [Flags]
    public enum HardwareConfiguration : ushort {
        Menu = 0x0001,
        ContactEncoder = 0x0040,
        ContactlessEncoder = 0x0080,
        IsoMagEncoder = 0x0100,
        JisMagEncoder = 0x0200,
        HighCoercivity = 0x0400,
        Encoder = 0x0800,
        HeatRoller = 0x1000,
        Reserved1 = 0x2000,
        Reserved2 = 0x4000,
        Flipper = 0x8000
    }

    public enum Sides { Front = 1, Back = 2 };
    public enum Options : byte { Contact = 0, Contactless = 1, Magnetic = 3 }
    public enum OptionResult : uint { Success = 1, Failure = 2 }

    public class CP500 {

        private const int EscapeMagnetic = 10000;
        private const int EscapeOption = 10002;
        private const int EscapeTag = 10004;

        public enum PrinterStatus : byte { Offline = 0, Ready = 1, Busy = 2, Error = 4, Printing = 8 }
        public enum CardStatus : byte { Waiting = 0, Feeding = 1, Flipping = 2, Encoding = 4, Printing = 8 }
        public enum InterfaceStatus : byte { Ready = 16, Busy = 32 }
        public enum PrintingStatus : byte { Idle = 0, Yellow = 1, Magenta = 2, Cyan = 4, Black = 8, Overlay = 16, Hologram = 32 }

        public enum PR5600RESULT {

            /// PR56ERR_NO_ERROR -> 0
            PR56ERR_NO_ERROR = 0,

            PR56ERR_NOT_REGISTERED,

            PR56ERR_ALREADY_REGISTERED,

            PR56ERR_INVALID_ARGUMENTS,

            PR56ERR_DRIVER_FAILURE,

            PR56ERR_PORT_FAILURE,

            PR56ERR_TOO_MANY_REGISTRATION,

            PR56ERR_ALREADY_USED,

            PR56ERR_INVALID_ID,

            PR56ERR_INVALID_HANDLE,

            PR56ERR_TIMEOUT,

            PR56ERR_PRINTER_NOT_OPEN,

            PR56ERR_PRINTER_NOT_EXIST,

            PR56ERR_INSUFFICIENT_BUFFER,

            PR56ERR_BUSY,

            PR56ERR_IN_PROGRESS,

            PR56ERR_NO_DATA,

            PR56ERR_COMM,

            PR56ERR_MEMORY,

            PR56ERR_OFFLINE,

            PR56ERR_NAC_RETURNED,

            PR56ERR_READWRITE_INCOMPLETE,

            PR56ERR_TAG_NOT_SIGNED,

            PR56ERR_ICENC_NOT_CONNECT,

            PR56ERR_ICENC_NOT_READY,

            PR56ERR_NOT_SUPPORTED_MODEL,

            PR56ERR_SERVER_NOTFOUND,

            PR56ERR_NOT_READY_SERVER,

            PR56ERR_NOT_READY_SMON,

            PR56ERR_NOT_RESERVED,

            PR56ERR_NOT_SUPPORTED,
        }



        // START

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Anonymous_8db05e1a_9e7e_4548_b40b_a75556c9c374 {

            /// BYTE->unsigned char
            public byte btStatus;

            /// BYTE->unsigned char
            public byte btMemA;

            /// BYTE->unsigned char
            public byte btMemB;

            /// BYTE->unsigned char
            public byte btReserved;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Anonymous_02138db1_2365_411c_9bd2_5794f9bc208d {

            /// DWORD->unsigned int
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public uint dwStatus;

            /// Anonymous_8db05e1a_9e7e_4548_b40b_a75556c9c374
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public Anonymous_8db05e1a_9e7e_4548_b40b_a75556c9c374 byte1;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Anonymous_e6ebfffa_9cce_44bc_815b_48f84e0bfa03 {

            /// BYTE->unsigned char
            public byte btCard;

            /// BYTE->unsigned char
            public byte btPrinter;

            /// BYTE->unsigned char
            public byte btEncoder;

            /// BYTE->unsigned char
            public byte btReserved;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Anonymous_99c7f247_922e_420d_894d_6302a947fbc9 {

            /// DWORD->unsigned int
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public uint dwStatus2;

            /// Anonymous_e6ebfffa_9cce_44bc_815b_48f84e0bfa03
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public Anonymous_e6ebfffa_9cce_44bc_815b_48f84e0bfa03 byte2;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct PR5600_PRINTER_STATUS {

            /// Anonymous_02138db1_2365_411c_9bd2_5794f9bc208d
            public Anonymous_02138db1_2365_411c_9bd2_5794f9bc208d Union1;

            /// Anonymous_99c7f247_922e_420d_894d_6302a947fbc9
            public Anonymous_99c7f247_922e_420d_894d_6302a947fbc9 Union2;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct Anonymous_a186a96e_7c37_4522_8d8a_9100975b556c {

            /// BYTE->unsigned char
            public byte btSenseKey;

            /// BYTE->unsigned char
            public byte btAdditionalSenseCode;

            /// BYTE->unsigned char
            public byte btAdditionalSenseCodeQualifier;

            /// BYTE->unsigned char
            public byte btFieldReplaceableUnitCode;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct PR5600_PRINTER_ERROR {

            /// DWORD->unsigned int
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public uint dwError;

            /// Anonymous_a186a96e_7c37_4522_8d8a_9100975b556c
            [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
            public Anonymous_a186a96e_7c37_4522_8d8a_9100975b556c Struct1;
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;
        }




        // END







        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct PR5600_STATUS_INFO {

            /// BYTE->unsigned char
            private byte btHardwareStatus;

            /// BYTE->unsigned char
            public CardStatus btCardPosition;

            /// BYTE->unsigned char
            public PrintingStatus btPrinterStatus;

            /// BYTE->unsigned char
            public byte btEncoderStatus;

            /// BYTE[2]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] btMemoryStatus;

            /// BYTE[2]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] btDataStatus;

            /// BYTE->unsigned char
            public byte btOptionUnitStatus;

            /// BYTE[3]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            private byte[] Count;

            /// BYTE[3]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] btColorControlValue;

            /// CHAR[4]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string acMagEncoderROMVersion;

            /// CHAR[4]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string acHeatRollerROMVersion;

            /// BYTE[8]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] btICTagData;

            /// BYTE->unsigned char
            public byte btReserved;


            public uint PrintCount {
                get { return (uint)Count[2] + ((uint)Count[1] << 8) + ((uint)Count[0] << 16); }
            }

            public PrinterStatus PrinterStatus {
                get { return (PrinterStatus)(btHardwareStatus & 0x0F); }
            }

            public InterfaceStatus InterfaceStatus {
                get { return (InterfaceStatus)(btHardwareStatus & 0xF0); }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PR5600_PRINTER_SENSE {
            public byte btErrorCode;
            public byte btSegmentNumber;
            public byte btSenseKey;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] btInfomation;
            public byte btAdditionalSenseLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] btCommandSpecificInformation;
            public byte btAdditionalSenseCode;
            public byte btAdditionalSenseCodeQualifier;
            public byte btFieldReplaceableUnitCode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] btSenseKeySpecific;
            public byte btAdditionalSenseBytes;
            public byte btErrorClassCode;
            public byte btAdditionalErrorCode;
            public byte btErrorUnitCode;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct PrinterInfo {
            public uint InternalId;
            public uint PrinterId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 264)]
            public string Name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string ServerName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string ShareName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 264)]
            public string DriverName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
            public byte[] ProductId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I1)]
            public byte[] Serial;
            public int bIcEncContact;
            public int bIcEncNoncontact;
            public bool IsOnline;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PR5600_SYSTEM_INFO {
            public HardwareConfiguration Configuration;

            /// BYTE->unsigned char
            public byte btRibbonSetting;

            /// BYTE->unsigned char
            public byte btRibbonPanel;

            /// BYTE->unsigned char
            public byte btReserved1;

            /// BYTE->unsigned char
            public byte btMemoryMode;

            /// BYTE->unsigned char
            public byte btPrinterSetting;

            /// BYTE->unsigned char
            public byte btReserved2;

            /// WORD->unsigned short
            public ushort wdMaxHorizontalSizeImage;

            /// WORD->unsigned short
            public ushort wdMaxVirticalSizeImage;

            /// WORD->unsigned short
            public ushort wdMaxHorizontalSizeText;

            /// WORD->unsigned short
            public ushort wdMaxVirticalSizeText;

            /// WORD->unsigned short
            public ushort wdMaxHorizontalSizeOverlay;

            /// WORD->unsigned short
            public ushort wdMaxVirticalSizeOverlay;

            /// WORD->unsigned short
            public ushort wdStartHorizontalPositionImage;

            /// WORD->unsigned short
            public ushort wdStartVirticalPositionImage;

            /// WORD->unsigned short
            public ushort wdStartHorizontalPositionText;

            /// WORD->unsigned short
            public ushort wdStartVirticalPositionText;

            /// WORD->unsigned short
            public ushort wdStartHorizontalPositionOverlay;

            /// WORD->unsigned short
            public ushort wdStartVirticalPositionOverlay;

            /// WORD->unsigned short
            public ushort wdResolution;

            /// CHAR[4]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string acBaseRevision;

            /// CHAR[4]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string acMainRevision;

            /// CHAR[8]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string acSerialNumber;

            /// CHAR->char
            public byte cPrinterID;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PR5600APPNOTIFY {
            public uint Notify;
            public System.IntPtr WindowHandle;
            public uint Message;

            public PR5600APPNOTIFY(uint notify, IntPtr handle, uint message) {
                this.Notify = notify;
                this.WindowHandle = handle;
                this.Message = message;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OptionVar {
            public byte OptionType;
            public byte Motion;
            public byte MgTrack;
            public byte Reserved;
            public IntPtr hWnd;
            public ushort wMsg;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct EncodeVar {
            public ushort Track;
            public ushort Size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] Data;

            public EncodeVar(ushort track, byte[] data) {
                byte[] buffer = new byte[200];

                Array.Copy(data, buffer, data.Length);
                this.Track = track;
                this.Size = (ushort)data.Length;
                this.Data = buffer;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ApplicationTag {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] Tag;
            public IntPtr hWnd;
            public ushort wMsg;


            public ApplicationTag(byte[] tag) {
                Tag = tag;
                hWnd = IntPtr.Zero;
                wMsg = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct PRNINFO4ICREADY {
            public uint InternalId;
            public uint PrinterId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 264)]
            public string Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
            public byte[] Tag;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PR5600_LOG {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 264)]
            public string User;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 264)]
            public string Document;
            public MiniGuid Tag;
            public uint PrinterId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string SerialNumber;
            public SYSTEMTIME Started;
            public SYSTEMTIME Completed;
            public SYSTEMTIME Received;
            public SYSTEMTIME Sent;
            public PR5600_PRINTER_STATUS Status;
            public PR5600_PRINTER_ERROR Error;
            public uint DataType;
            public uint ErrorPage;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string ErrorMessage;
            public uint ErrorCode;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string Correction;
        }

        //------------------------------ METHODS ------------------------------

        /// <summary>
        /// Enumerate a list of printers
        /// </summary>
        /// <returns>List of printers</returns>
        public static List<PrinterInfo> GetPrinters() {
            uint size = 0;
            uint count = 0;
            PR5600RESULT result = pr56XXEnumPrinters(IntPtr.Zero, 0, ref size, ref count);
            List<PrinterInfo> printers = new List<PrinterInfo>();

            if ((size > 0) && (size % Marshal.SizeOf(typeof(PrinterInfo)) == 0)) {
                byte[] buffer = new byte[size];
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                IntPtr pointer = handle.AddrOfPinnedObject();
                pr56XXEnumPrinters(pointer, size, ref size, ref count);

                for (int i = 0; i < count; i++) {
                    IntPtr p1 = new IntPtr(pointer.ToInt64() + (i * Marshal.SizeOf(typeof(PrinterInfo))));
                    PrinterInfo p = (PrinterInfo)Marshal.PtrToStructure(p1, typeof(PrinterInfo));
                    printers.Add(p);
                }

                handle.Free();
            }

            return printers;
        }

        /// <summary>
        /// Open a connection to a printer up to a maximum of 32. All connections must be closed via ClosePrinter.
        /// </summary>
        /// <param name="printerName">Name of the printer</param>
        /// <returns>Unique handle to the printer</returns>
        public static IntPtr OpenPrinter(string printerName) {
            return pr56XXOpenPrinter(0, 0, printerName, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Close the connection to the printer
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        public static void ClosePrinter(IntPtr printerHandle) {
            uint result = pr56XXClosePrinter(printerHandle);
        }

        /// <summary>
        /// Retrieve the printer log
        /// </summary>
        /// <param name="printerName">Name of the printer</param>
        /// <returns></returns>
        public static PR5600_LOG[] GetLog(string printerName) {
            PR5600_LOG[] log = new PR5600_LOG[500];
            uint count = 0;
            var result = pr56XXGetLogs(0, 0, printerName, (uint)log.Length, ref count, log);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Error retrieving log");

            Array.Resize(ref log, (int)count);
            return log;
        }

        /// <summary>
        /// Get the status of the printer
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <returns></returns>
        public static PR5600_STATUS_INFO GetStatus(IntPtr printerHandle) {
            PR5600_STATUS_INFO status = new PR5600_STATUS_INFO();
            PR5600_PRINTER_SENSE sense = new PR5600_PRINTER_SENSE();
            PR5600RESULT result = pr56XXGetPrinterStatusInformation(printerHandle, ref status, ref sense);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Unable to retrieve status");

            return status;
        }

        /// <summary>
        /// Get information about the printer
        /// </summary>
        /// <param name="printerName">Name of the printer</param>
        /// <returns></returns>
        public static PR5600_SYSTEM_INFO GetSystemInfo(string printerName) {
            PR5600_SYSTEM_INFO info = new PR5600_SYSTEM_INFO();
            PR5600RESULT result = pr56XXGetSysInfoOnly(printerName, ref info);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Unable to retrieve system info");

            return info;
        }

        /// <summary>
        /// Registers an event handler for option processing
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <param name="notification"></param>
        public static void RegisterMessage(IntPtr printerHandle, PR5600APPNOTIFY notification) {
            PR5600RESULT result = pr56XXRegisterMessage(printerHandle, 1, ref notification);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Unable to register message");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="enable"></param>
        public static void SetMonochrome(IntPtr hdc, bool enable) {
            IntPtr pointer = IntPtr.Zero;

            try {
                short option = enable ? (short)1 : (short)0;
                pointer = Marshal.AllocHGlobal(sizeof(ushort));
                Marshal.WriteInt16(pointer, option);
                ExtEscape(hdc, 10006, sizeof(ushort), pointer, 0, IntPtr.Zero);
            } finally {
                if (pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printerHandle"></param>
        /// <returns></returns>
        public static byte[] GetMagneticData(IntPtr printerHandle) {
            byte[] buffer = new byte[384];
            uint received = 0;
            PR5600RESULT result = pr56XXGetPrinterData(printerHandle, ref buffer[0], (uint)buffer.Length, ref received);
            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Retrieving magnetic data failed");
            return buffer;
        }

        /// <summary>
        /// Set the status of the printer after an optional device has been processed
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <param name="result">Result of processing</param>
        public static void SetOptionResult(IntPtr printerHandle, OptionResult result) {
            PR5600RESULT r = pr56XXPrint_SetICEncoder(printerHandle, (uint)result, true);
            if (r != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Encoding action failed");
        }

        /// <summary>
        /// Send data to the onboard contactless encoder
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <param name="buffer">Data to send to the encoder</param>
        public static void SendICEncoder(IntPtr printerHandle, byte[] buffer) {
            PR5600RESULT result = pr56XXSendICEncoder(printerHandle, ref buffer[0], (uint)buffer.Length);
            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Send to encoder action failed");
        }

        /// <summary>
        /// Receive data from the onboard contactless encoder
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <param name="buffer">Buffer to hold data</param>
        /// <param name="offset">Offset in buffer</param>
        /// <param name="count">Length of buffer</param>
        /// <returns>Number of bytes received</returns>
        public static int ReceiveICEncoder(IntPtr printerHandle, ref byte[] buffer, int offset, uint count) {
            uint received = 0;
            PR5600RESULT result = pr56XXRecvICEncoder(printerHandle, ref buffer[offset], count, ref received);
            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Send to encoder action failed");
            return (int)received;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hdevmode"></param>
        /// <param name="side"></param>
        /// <param name="enabled"></param>
        public static void SetBlackControl(IntPtr hdevmode, Sides side, bool enabled) {
            HandleRef handleRef = new HandleRef(null, hdevmode);
            IntPtr devmode = GlobalLock(handleRef);
            CP500._SetBlackControl(devmode, (int)side, enabled);
            GlobalUnlock(handleRef);
        }

        /// <summary>
        /// Receive data from the onboard contactless encoder
        /// </summary>
        /// <param name="printerHandle">Unique handle to the printer</param>
        /// <param name="buffer">Buffer to hold data</param>
        /// <returns>Number of bytes received</returns>
        public static int ReceiveICEncoder(IntPtr printerHandle, ref byte[] buffer) {
            uint received = 0;
            PR5600RESULT result = pr56XXRecvICEncoder(printerHandle, ref buffer[0], (uint)buffer.Length, ref received);
            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Send to encoder action failed");
            return (int)received;
        }

        /// <summary>
        /// Set the image file used for ultraviolet printing
        /// </summary>
        /// <param name="hdevmode">Handle to the printer devmode structure</param>
        /// <param name="side">Side which to apply the image</param>
        /// <param name="fileName">Image file to use for printing</param>
        /// <param name="x">Horizontal offset of image measured in dots</param>
        /// <param name="y">Vertical offset of image measured in dots</param>
        /// <param name="isMonochrome">Whether or not to process the image as a monochrome image</param>
        public static void SetUltraviolet(IntPtr hdevmode, Sides side, string fileName, short x, short y, bool isMonochrome) {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Ultraviolet image file not found");

            HandleRef handleRef = new HandleRef(null, hdevmode);
            IntPtr devmode = GlobalLock(handleRef);
            SetUV(devmode, (int)side, fileName, x, y, isMonochrome);
            GlobalUnlock(handleRef);
        }

        /// <summary>
        /// Set a primerless area
        /// </summary>
        /// <param name="hdevmode">Handle to the printer devmode structure</param>
        /// <param name="side"></param>
        /// <param name="zone"></param>
        /// <param name="x">Horizontal offset of area measured in dots</param>
        /// <param name="y">Vertical offset of area measured in dots</param>
        /// <param name="width">Width of area measured in dots</param>
        /// <param name="height">Height of area measured in dots</param>
        public static void SetPrimerlessZone(IntPtr hdevmode, Sides side, int zone, short x, short y, short width, short height) {
            if (zone < 1 || zone > 5)
                throw new ArgumentOutOfRangeException("zone");

            HandleRef handleRef = new HandleRef(null, hdevmode);
            IntPtr devmode = GlobalLock(handleRef);
            SetPrimerless(devmode, (int)side, zone, x, y, width, height);
            GlobalUnlock(handleRef);
        }

        public static void Release(IntPtr printerHandle) {
            byte[] buffer = { 0x1b, 0x3d, 0x31, 0x2e };
            PR5600RESULT result = pr56XXPrint(printerHandle, ref buffer[0], 4);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Error in print routine");
        }

        public static void Reject(IntPtr printerHandle) {
            byte[] buffer = { 0x1b, 0x3d, 0x31, 0x2e };
            PR5600RESULT result = pr56XXPrint(printerHandle, ref buffer[0], 4);

            if (result != PR5600RESULT.PR56ERR_NO_ERROR)
                throw new Exception("Error in print routine");
        }

        // write a magnetic track
        public static void WriteTrack(IntPtr hdc, EncodeVar track) {
            IntPtr pointer = IntPtr.Zero;

            try {
                pointer = Marshal.AllocHGlobal(Marshal.SizeOf(track));
                Marshal.StructureToPtr(track, pointer, false);
                int result = ExtEscape(hdc, EscapeMagnetic, Marshal.SizeOf(track), pointer, 0, IntPtr.Zero);

                if (result <= 0)
                    throw new Exception("Unable to write track");

            } finally {
                if (pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// Assigns a unique identifier to the print job for tracking purposes
        /// </summary>
        /// <param name="hdc">Handle to the device context</param>
        /// <param name="guid">Unique ID assigned to the print job</param>
        public static void Tag(IntPtr hdc, MiniGuid guid) {
            IntPtr pointer = IntPtr.Zero;

            try {
                ApplicationTag tag = new ApplicationTag(guid.ToByteArray());
                pointer = Marshal.AllocHGlobal(Marshal.SizeOf(tag));
                Marshal.StructureToPtr(tag, pointer, false);
                int result = ExtEscape(hdc, EscapeTag, Marshal.SizeOf(tag), pointer, 0, IntPtr.Zero);

                if (result <= 0)
                    throw new Exception("Unable to assign application tag");
            } finally {
                if (pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pointer);
            }
        }


        public static void RegisterOption(IntPtr hdc, IntPtr hWnd, Options options, byte motion, byte track) {
            IntPtr pointer = IntPtr.Zero;

            OptionVar option = new OptionVar() {
                OptionType = (byte)options,
                Motion = motion,                 // this was 5, 5 means to hold the card after encode; 0 is the other option
                MgTrack = track,
                Reserved = 0,
                wMsg = 32771,
                hWnd = hWnd
            };

            try {
                pointer = Marshal.AllocHGlobal(Marshal.SizeOf(option));
                Marshal.StructureToPtr(option, pointer, false);
                int result = ExtEscape(hdc, EscapeOption, Marshal.SizeOf(option), pointer, 0, IntPtr.Zero);

                if (result <= 0)
                    throw new Exception("Unable to register option");
            } finally {
                if (pointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(pointer);
            }
        }

        /// <summary>
        /// Register an optional device in the print sequence
        /// </summary>
        /// <param name="hdc">Handle to the device context</param>
        /// <param name="hWnd">Window handle to receive callback when the card arrives at the device</param>
        /// <param name="options">Device to include in the print sequence</param>
        /// <param name="motion">Type of processing</param>
        public static void RegisterOption(IntPtr hdc, IntPtr hWnd, Options options, byte motion) {
            RegisterOption(hdc, hWnd, options, motion, 0);
        }

        //------------------------------ PRIVATE ------------------------------

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXOpenPrinter")]
        private static extern IntPtr pr56XXOpenPrinter(uint dwInternalID, uint dwPrinterID, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPWStr)] string wszPrinterName, System.IntPtr hWnd, uint Msg);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXClosePrinter")]
        private static extern uint pr56XXClosePrinter(IntPtr hPrinter);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXEnumPrinters")]
        private static extern PR5600RESULT pr56XXEnumPrinters(IntPtr pInfo, uint dwSize, ref uint pdwNeeded, ref uint pdwReturned);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXEnumEnabledPrinters")]
        private static extern PR5600RESULT pr56XXEnumEnabledPrinters(IntPtr pInfo, uint dwSize, ref uint pdwNeeded, ref uint pdwReturned);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXGetLogs")]
        private static extern PR5600RESULT pr56XXGetLogs(uint dwInternalID, uint dwPrinterID, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string wszPrinterName, uint dwLogs, ref uint pdwReturned, [In, Out] PR5600_LOG[] pLog);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXGetPrinterStatusInformation")]
        private static extern PR5600RESULT pr56XXGetPrinterStatusInformation(IntPtr hPrinter, ref PR5600_STATUS_INFO pInfo, ref PR5600_PRINTER_SENSE pSense);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXGetSysInfoOnly")]
        private static extern PR5600RESULT pr56XXGetSysInfoOnly([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string wszPrinterName, ref PR5600_SYSTEM_INFO pInfo);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXRegisterMessage")]
        private static extern PR5600RESULT pr56XXRegisterMessage(IntPtr hPrinter, uint dwcount, ref PR5600APPNOTIFY aNotifies);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXGetPrinterData")]
        private static extern PR5600RESULT pr56XXGetPrinterData(IntPtr hPrinter, ref byte pBuffer, uint dwSize, ref uint pdwReceived);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXPrint_SetICEncoder")]
        private static extern PR5600RESULT pr56XXPrint_SetICEncoder(IntPtr hPrinter, uint dwAction, [MarshalAs(UnmanagedType.Bool)] bool bContact);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXSendICEncoder")]
        private static extern PR5600RESULT pr56XXSendICEncoder(IntPtr hPrinter, ref byte pBuffer, uint dwSize);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXRecvICEncoder")]
        private static extern PR5600RESULT pr56XXRecvICEncoder(IntPtr hPrinter, ref byte pBuffer, uint dwSize, ref uint pdwReceived);

        [DllImport("CPRDAPI.DLL", EntryPoint = "pr56XXPrint")]
        public static extern PR5600RESULT pr56XXPrint(IntPtr hPrinter, ref byte pBuffer, uint dwSize);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GlobalFree(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr GlobalLock(HandleRef handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern bool GlobalUnlock(HandleRef handle);

        [DllImport("CP500Extensions.dll", EntryPoint = "SetUV", CallingConvention = CallingConvention.Cdecl)]
        private static extern System.IntPtr SetUV(IntPtr devmode, int side, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string fileName, short x, short y, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool isMonochrome);

        [DllImport("CP500Extensions.dll", EntryPoint = "SetPrimerless", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetPrimerless(IntPtr devmode, int side, int zone, short x, short y, short width, short height);

        [DllImport("gdi32.dll")]
        private static extern int ExtEscape(IntPtr hdc, int nEscape, int cbInput, IntPtr lpszInData, int cbOutput, IntPtr lpszOutData);

        [DllImport("CP500Extensions.dll", EntryPoint = "SetBlackControl", CallingConvention = CallingConvention.Cdecl)]
        private static extern void _SetBlackControl(IntPtr devmode, int side, bool enabled);

    }
}

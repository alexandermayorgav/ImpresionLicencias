using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Print.Printing {

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MiniGuid {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.I1)]
        private byte[] tag;

        public static MiniGuid NewGuid() {
            Random generator = new Random();
            MiniGuid guid = new MiniGuid();
            guid.tag = new byte[8];
            generator.NextBytes(guid.tag);
            return guid;
        }

        public override bool Equals(object obj) {
            if (obj is MiniGuid) {
                MiniGuid that = (MiniGuid)obj;

                if (this.tag == null && that.tag == null)
                    return true;
                if (this.tag == null || that.tag == null)
                    return false;
                if (this.tag.Length != that.tag.Length)
                    return false;

                for (int i = 0; i < this.tag.Length; i++) {
                    if (this.tag[i] != that.tag[i])
                        return false;
                }

                return true;
            } else
                return false;
        }

        public static bool operator ==(MiniGuid a, MiniGuid b) {
            if (a.tag == null && b.tag == null)
                return true;
            if (a.tag == null || b.tag == null)
                return false;
            if (a.tag.Length != b.tag.Length)
                return false;

            for (int i = 0; i < a.tag.Length; i++) {
                if (a.tag[i] != b.tag[i])
                    return false;
            }

            return true;
        }

        public static bool operator !=(MiniGuid a, MiniGuid b) {
            return !(a == b);
        }

        public override int GetHashCode() {
            return BitConverter.ToInt32(tag, 0);
        }

        public byte[] ToByteArray() {
            byte[] buffer = new byte[8];
            Array.Copy(tag, buffer, 8);
            return buffer;
        }

        public static MiniGuid Empty {
            get { return new MiniGuid(); }
        }

    }
}

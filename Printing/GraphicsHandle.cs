using System;
using System.Drawing;

namespace Print.Printing {
    public class GraphicsHandle : IDisposable {

        private Graphics graphics;
        private IntPtr handle = IntPtr.Zero;
        private bool isDisposed = false;

        //---------------------------- CONSTRUCTOR ----------------------------

        private GraphicsHandle(Graphics graphics) {
            this.graphics = graphics;
            this.handle = graphics.GetHdc();
        }

        ~GraphicsHandle() {
            Dispose();
        }

        //------------------------------ METHODS ------------------------------

        public static GraphicsHandle Open(Graphics graphics) {
            return new GraphicsHandle(graphics);
        }

        public void Close() {
            if (handle != IntPtr.Zero) {
                graphics.ReleaseHdc(handle);
                handle = IntPtr.Zero;
            }
        }

        public void Dispose() {
            if (!isDisposed) {
                Close();
                isDisposed = true;
            }
        }

        //----------------------------- PROPERTIES ----------------------------

        public IntPtr Handle {
            get { return handle; }
        }

        public bool IsAvailable {
            get { return handle != IntPtr.Zero; }
        }

    }
}

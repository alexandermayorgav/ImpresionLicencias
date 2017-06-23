using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Print.Printing;

namespace Print {
    public class PrintRunner {

        private PrintDocument document = new PrintDocument();
        private CardModel model;
        private enum Surface { Front, Back };
        private Surface sides = Surface.Front;
        private Artifacts artifacts;

        // maximum size of a print
        private Size panel = new Size(2048, 1300);                          
        
        //---------------------------- CONSTRUCTOR ----------------------------

        public PrintRunner(CardModel model) {
            this.model = model;
            this.artifacts = model.Artifacts;
            document.PrintPage += new PrintPageEventHandler(OnPrintPage);
            document.PrintController = new StandardPrintController();
            document.PrinterSettings = new PrinterSettings() {
                PrinterName = model.PrinterName,
                Duplex = (IsPrintingBack) ? Duplex.Horizontal : Duplex.Simplex
            };
        }

        //------------------------------ METHODS ------------------------------

        // by default printing is sent to the print queue 
        public void Run() {
            Initialize();
            document.Print();
        }
        
        // apply ultraviolet settings
        private void Initialize() {
            IntPtr devmode = document.PrinterSettings.GetHdevmode();

            // disable black processing based on primitive (text, image etc) and handle ourselves using escape()
            CP500.SetBlackControl(devmode, Sides.Front, false);
            CP500.SetBlackControl(devmode, Sides.Back, false);

            ConfigureUV(devmode, model.Ultraviolet1, Sides.Front);
            ConfigureUV(devmode, model.Ultraviolet2, Sides.Back);

            document.PrinterSettings.SetHdevmode(devmode);
        }
        
        // print a page (eg side)
        void OnPrintPage(object sender, PrintPageEventArgs e) {
            if (sides == Surface.Front) {
                PrintSide1(e.Graphics);
                e.HasMorePages = IsPrintingBack;
                sides = Surface.Back;
            } else {
                PrintSide2(e.Graphics);
                e.HasMorePages = false;
                sides = Surface.Front;
            }
        }

        // print the front (side1)
        private void PrintSide1(Graphics g) {
            if (model.Color1 != null)
                Rasterize(g, model.Color1, false);
            if (model.Text1 != null)
                Rasterize(g, model.Text1, true);

            //Rasterize(g);
        }

        // print the back (side2)
        private void PrintSide2(Graphics g) {
            if (model.Color2 != null)
                Rasterize(g, model.Color2, false);
            if (model.Text2 != null)
                Rasterize(g, model.Text2, true);
        }

        // draws image onto printing surface
        // for optimal printing use images with a resolution of 600 dpi
        private void Rasterize(Graphics g, Image image, bool monochrome) {
            using(GraphicsHandle gh = GraphicsHandle.Open(g))
                CP500.SetMonochrome(gh.Handle, monochrome);

            GraphicsState state = g.Save();
            g.PageUnit = GraphicsUnit.Pixel;
            // center the graphic inside the panels
            float dx = (panel.Width - image.Width) / 2;         
            float dy = (panel.Height - image.Height) / 2;
            g.TranslateTransform(dx, dy);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            g.Restore(state);
        }

        //private void Rasterize(Graphics g)
        //{
        //    ////using (GraphicsHandle gh = GraphicsHandle.Open(g))
        //    ////    CP500.SetMonochrome(gh.Handle, monochrome);

        //    //GraphicsState state = g.Save();
        //    //g.PageUnit = GraphicsUnit.Pixel;
        //    //// center the graphic inside the panels
        //    //Image image = Image.FromFile("C:\\FaceImage.jpg");
        //    //float dx = (panel.Width - image.Width) / 2;
        //    //float dy = (panel.Height - image.Height) / 2;
        //    //g.TranslateTransform(dx, dy);
        //    //g.DrawImage(image, 0, 0, image.Width, image.Height);
        //    //g.Restore(state);
        //}

        // configure ultraviolet (UV)
        private void ConfigureUV(IntPtr devmode, Image image, Sides side) {
            if (image != null) {
                // create a temporary file which can only be removed after the actual print has finished
                // remember the print is sent to a print queue by default
                string filePath = artifacts.Create("bmp");

                // center the graphic inside the panels
                int dx = (panel.Width - image.Width) / 2;
                int dy = (panel.Height - image.Height) / 2;
                image.Save(filePath, ImageFormat.Bmp);
                CP500.SetUltraviolet(devmode, side, filePath, (short)dx, (short)dy, false);
            }
        }

        //---------------------------- PROPERTIES -----------------------------

        public bool IsPrintingFront {
            get { return (model.Color1 != null) || (model.Text1 != null) || (model.Ultraviolet1 != null); }
        }

        public bool IsPrintingBack {
            get { return (model.Color2 != null) || (model.Text2 != null) || (model.Ultraviolet2 != null); }
        }

        public bool IsDoubleSided {
            get { return IsPrintingFront && IsPrintingBack; }
        }

    }
}

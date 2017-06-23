using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Print.Printing;

namespace Print {
    public class CardModel {

        private Image color1;
        private Image color2;
        private Image text1;
        private Image text2;
        private Image ultraviolet1;
        private Image ultraviolet2;
        private string printerName;
        private Artifacts artifacts;


        //---------------------------- CONSTRUCTION ---------------------------

        public CardModel(Artifacts artifacts) {
            this.artifacts = artifacts;
        }

        //----------------------------- PROPERTIES ----------------------------

        public string PrinterName {
            get { return printerName; }
            set { printerName = value; }
        }

        public Image Color1 {
            get { return color1; }
            set { color1 = value; }
        }

        // black text layer must be a monochrome image (black)
        public Image Text1 {
            get { return text1; }
            set { text1 = value; }
        }

        public Image Ultraviolet1 {
            get { return ultraviolet1; }
            set { ultraviolet1 = value; }
        }

        public Image Color2 {
            get { return color2; }
            set { color2 = value; }
        }

        // black text layer must be a monochrome image (black)
        public Image Text2 {
            get { return text2; }
            set { text2 = value; }
        }

        public Image Ultraviolet2 {
            get { return ultraviolet2; }
            set { ultraviolet2 = value; }
        }

        // storage mechanism for temporary files
        public Artifacts Artifacts {
            get { return artifacts; }
        }

    }
}

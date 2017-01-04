using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Print.Printing {
    public class Artifacts {

        private List<string> files = new List<string>();

        //---------------------------- CONSTRUCTION ---------------------------

        public Artifacts() {

        }
        
        ~Artifacts() {
            Clear();
        }

        //------------------------------ METHODS ------------------------------
        
        public string Create(string extension) {
            string filePath = Application.StartupPath;
            string fileName = "";

            do {
                fileName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
                filePath = Path.Combine(filePath, fileName);
            } while (files.Contains(filePath));

            files.Add(filePath);
            return filePath;
        }

        public void Destroy(string filePath) {
            if (files.Contains(filePath)) {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                files.Remove(filePath);
            }
        }

        public void Clear() {
            foreach (string filePath in files)
                if (File.Exists(filePath))
                    File.Delete(filePath);

            files.Clear();
        }

    }
}

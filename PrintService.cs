using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Print {
    public class PrintService {

        // this is single threaded but could be changed
        public static void Print(CardModel cardModel) {
            PrintRunner runner = new PrintRunner(cardModel);
            runner.Run();
        }

    }
}

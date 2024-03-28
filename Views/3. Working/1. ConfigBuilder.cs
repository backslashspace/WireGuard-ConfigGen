using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigGen.Views
{
    public partial class Working
    {
        private static void BuildConfig(ref UInt16 i, out ConfigPair configPair)
        {

            configPair.Client = "s";
            configPair.Server = "s";

            Task.Delay(1000).Wait();
        }
    }
}
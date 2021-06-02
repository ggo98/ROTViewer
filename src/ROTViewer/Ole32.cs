using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ROTViewer
{
    class Ole32
    {
        [DllImport("Ole32.Dll")]
        public static extern int CreateBindCtx(int reserved, out IBindCtx bindCtx);
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class IDisposableExtension
    {
        public static void DisposeIfNotNull(this IDisposable disObj)
        {
            if (disObj != null)
            {
                disObj.Dispose();
            }
        }
    }
}

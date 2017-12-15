using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threax.AspNetCore.BuiltInTools
{
    public class ToolException : Exception
    {
        public ToolException(String message)
            :base(message)
        {

        }
    }
}

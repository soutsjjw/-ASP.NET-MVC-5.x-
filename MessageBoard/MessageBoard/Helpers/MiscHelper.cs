using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessageBoard.Helpers
{
    public class MiscHelper
	{
        public static string ShortGuid
        {
            get
            {
                return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            }
        }
	}
}

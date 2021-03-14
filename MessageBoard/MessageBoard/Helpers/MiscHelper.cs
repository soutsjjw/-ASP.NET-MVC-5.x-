using System;
using System.Text.RegularExpressions;

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

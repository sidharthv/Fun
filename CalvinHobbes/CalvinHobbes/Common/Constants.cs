using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalvinHobbes.Common
{
    public class Constants
    {
        public const string BASE_URL = "http://www.gocomics.com/calvinandhobbes/{0}/{1}/{2}";

        public const string BASE_IMAGE_URL = "http://assets.amuniversal.com/";

        public const string DATA_FILE_NAME = "ch.txt";

        public const string DATETIME_FORMAT = "M/d/yyyy";

        public static DateTime MIN_COMIC_DATE = new DateTime(1985, 11, 17);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Help_elements
{
    internal static class StaticProperties
    {
        public const int IMAGE_MAX_COUNT = 5;

        public const int DESCRIPTION_TEXT_MAX_LENGTH = 3000;
        public const int DESCRIPTION_TEXT_MIN_LENGTH = 10;

        public const int COMMENT_MAX_LENGTH = 1000;
        public const int URL_MAX_LENGTH = 3000;
        public const int TITLE_MAX_LENGTH = 300;

        public const int PRICE_STEP = 5;

        public const int GENRE_MIN_LENGTH = 3;
		public const int GENRE_MAX_LENGTH = 100;

		public const int USER_NAME_MAX_LENGTH = 100;
		public const int USER_NAME_MIN_LENGTH = 5;

		public const int PASSWORD_MAX_LENGTH = 20;
		public const int PASSWORD_MIN_LENGTH = 8;

        public const int EMAIL_MAX_LENGTH = 200;
        public const int EMAIL_MIN_LENGTH = 5;
	}
}

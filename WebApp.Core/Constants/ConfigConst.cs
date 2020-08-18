using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Core.Constants
{
    public class ConfigConst
    {
        public class Length
        {
            public const int ShortMaMaxLength = 32;
            public const int MaMaxLength = 64;

            public const int ShortNameMaxLength = 64;
            public const int NameMaxLength = 128;
            public const int LongNameMaxLength = 256;

            public const int ShortMessageMaxLength = 256;
            public const int MessageMaxLength = 512;

            public const int FilenameMaxLength = 256;
            public const int FilePathnameMaxLength = 256;

            public const int SoDienThoaiMaxLength = 16;
            public const int GhichuMaxLength = MessageMaxLength;
        }
    }
}

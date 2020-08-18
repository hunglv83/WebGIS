using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ciren.COpenXML
{
    public class TextFormat
    {
        public int _ftype; //0: khong style; //1: bold, italic; //2: bold; //3: italic
        public string _str;

        public int Ftype { get { return this._ftype; } set { this._ftype = value; } }
        public string Str { get { return this._str; } set { this._str = value; } }
    }
}

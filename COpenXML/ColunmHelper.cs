using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ciren.COpenXML
{
    public class ColunmHelper
    {
        public string _cValue;  //ten cot cua datatable
        public string _cText;   //ten muon hien thi trong excel
        public int _cType;      //kieu du lieu cua cot 1:Chu; 2: so; 3: ngay thang
        public int _cWidth;     //chiều rộng của cột 
        public int _cAlignment; //Alignment các cột nội dung của datatable 1: Left; 2: Center; 3: Right;

        public string CValue
        {
            get { return this._cValue; }
            set { this._cValue = value; }
        }
        public string CText { get { return this._cText; } set { this._cText = value; } }
        public int CType { get { return this._cType; } set { this._cType = value; } }
        public int CWidth { get { return this._cWidth; } set { this._cWidth = value; } }
        public int CAlignment { get { return this._cAlignment; } set { this._cAlignment = value; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.Core.EF;

namespace WebApp.Core.DAO
{
    public class jsTree
    {
        public jsTree()
        {
            icon = "fa fa-cubes";
            state = new jsTreeState();
        }
        public int id { get; set; }
        public string text { get; set; }
        public jsTreeState state { get; set; }
        public string icon { get; set; }
        public List<jsTree> children { get; set; }
    }

    public class jsTreeString
    {
        public jsTreeString()
        {
            icon = "fa fa-cubes";
            State = new jsTreeState();
        }
        public string Code { get; set; }
        public string text { get; set; }
        public jsTreeState State { get; set; }
        public string icon { get; set; }
        public List<jsTreeString> children { get; set; }
    }

    public class jsTreeState
    {
        public jsTreeState()
        {
            opened = true;
            disabled = false;
            selected = false;
        }
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }

    public class MainMenu
    {
        public string stringMenu { get; set; }
        public List<int> roleMenu { get; set; }
    }

    public class PagePartialBox
    {
        public CSF_Pages_GetPartial_Result box { get; set; }
        public List<CSF_Pages_GetPartial_Result> boxChild { get; set; }
    }

    public class LichHop
    {
        public string thu { get; set; }
        public string ngay { get; set; }
        public List<CMS_Schedules> data { get; set; }
    }

    public class SubSelectBox
    {
        public int id { get; set; }
        public string name { get; set; }
        public int parentid { get; set; }
    }

    public class SubSelectBoxString
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public bool IsDisable { get; set; }
    }

    public class DieuKien
    {
        public string TenDieuKien { get; set; }
        public string NoiDung { get; set; }
        public decimal CaoTrinh { get; set; }
        public int ToanTu { get; set; }
        //1: Lon hon, 2: Lon hon bang, 3: Bang, 4: Be hon, 5: Be hon bang
    }
}

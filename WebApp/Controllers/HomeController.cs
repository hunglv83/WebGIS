using WebApp.Business.Services;
using WebApp.Common;
using WebApp.Core.DAO;
using WebApp.Core.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private DT_WebGISEntities _db = new DT_WebGISEntities();

        public ActionResult Index(string pagekey, string category, string pagename, string pagetype, string ticket, string id)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();

                #region SSO

                if (ticket != null)
                {
                    if (LoginSSO())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                #endregion SSO

                #region TEMPLATE

                var temp = MyContext.CSF_Templates.FirstOrDefault();
                ViewBag.TEMPLATE = temp != null ? temp.style : "CS01";
                TempData["template"] = temp;
                TempData.Keep("template");
                ViewBag.URLIMAGE = System.Configuration.ConfigurationManager.AppSettings["UrlImage"];

                #endregion TEMPLATE

                string username = HttpContext.User.Identity.Name;
                string url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
                if (username != "")
                {
                    Session["IsAuthenticated"] = true;
                    return Redirect(url + "/admin/qt_home");
                }
                else
                {
                    Session["IsAuthenticated"] = null;
                    return Redirect(url + "/cs/Home/Login");
                }
                if (pagekey == null || pagekey == "Home")
                {
                    pagekey = "trang-chu";
                }
                if (id != null)
                {
                    category = pagetype;
                }
                int intGuestGroup = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDGuestGroup"]);
                //get all partialview in page
                List<CSF_Pages_GetPartial_FrontEnd_Result> pagePartial = new List<CSF_Pages_GetPartial_FrontEnd_Result>();
                if (category != null)
                {
                    pagePartial = MyContext.CSF_Pages_GetPartial_FrontEnd(category, 1, username, intGuestGroup).ToList();
                }
                else
                {
                    pagePartial = MyContext.CSF_Pages_GetPartial_FrontEnd(pagekey, 1, username, intGuestGroup).ToList();
                }
                ViewBag.KEY = category != null ? category : pagekey;//Lấy key loại tin bài hoặc tin bài
                //Active menu
                var page1 = MyContext.CSF_Pages.Where(x => x.Key.Contains(pagekey)).FirstOrDefault();
                if (!Convert.ToBoolean(page1.IsActive))
                {
                    string urlHome = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
                    return Redirect(urlHome);
                }
                int key1 = page1 != null ? page1.ID : 0;
                var page2 = MyContext.CSF_Pages.Where(x => x.Key.Contains(category)).FirstOrDefault();
                int key2 = page2 != null ? page2.ID : 0;
                ViewBag.KEY1 = key1;
                ViewBag.KEY2 = key2;
                return View(pagePartial);
                //return RedirectToAction("Index", "QT_Home", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string Token, string Email)
        {
            var user = _db.CSF_Users.SingleOrDefault(m => m.ActiveCode.ToUpper() == Token.ToUpper());
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.Status = 1;
                    user.ActiveDate = DateTime.Now;
                    user.ActiveCode = Guid.NewGuid().ToString();
                    _db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("Confirm", "Home", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Home", new { Email = "" });
            }

            // Login
            FormsAuthentication.SetAuthCookie(user.UserName.Trim(), false);
            string url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
            //Hiển thời để redirect về trang cập nhật thông tin
            return Redirect(url + @"/admin/nan_tochuc/updateinformation");
        }

        #region Login,Logout

        public ActionResult Login()
        {
            string returnURL = ((System.Web.HttpRequestWrapper)HttpContext.Request).QueryString["RedirectUrl"];
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (string.IsNullOrEmpty(returnURL))
                    return RedirectToAction("Index", "Home");
                else
                    return Redirect(returnURL);
            }

            ViewBag.RequestPath = returnURL;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string strUserName = model.UserName.Trim();
                string strPass = Encryptor.MD5Hash(model.Password);
                CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                int intResult = objUserDao.Login(strUserName, strPass);
                switch (intResult)
                {
                    case 0://Tên đăng nhập hoặc mật khẩu không đúng
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng!");
                        break;

                    case 1://Đăng nhập thành công
                        if (model.RememberPass)
                        {
                            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, model.UserName.Trim(), DateTime.Now, DateTime.Now.AddSeconds(20), false, "", FormsAuthentication.FormsCookiePath);
                            //Encrypt the ticket
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            //Create a cookie and add the encrypted ticket to the cookie as data
                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            //Add the cookie to the outgoing cookie collection
                            Response.Cookies.Add(authCookie);
                        }
                        FormsAuthentication.SetAuthCookie(model.UserName.Trim(), false);
                        string url = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];
                        //Xác định redirect vào trang quản trị hay trang chính
                        //if (model.UserName.Equals("host"))
                        //{
                        //}
                        //return Redirect(url + "/admin/qt_home");
                        //Hiển thời để redirect về trang chủ
                        return Redirect(model.ReturnURL ?? url);

                    case -1:
                        ModelState.AddModelError("", "Tài khoản chưa được click hoạt!");
                        break;

                    case -2:
                        ModelState.AddModelError("", "Mật khẩu không đúng!");
                        break;

                    default:
                        break;
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session["IsAuthenticated"] = null;
                return RedirectToAction("Index", "Home");
                //string urlLogoutSSO = System.Configuration.ConfigurationManager.AppSettings["casLogoutURL"];
                //return Redirect(urlLogoutSSO);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        private bool LoginSSO()
        {
            try
            {
                //DotNetCasAttributePrincipal principal = null;
                //string username = HttpContext.User.Identity.Name;
                //if (username == "")
                //{
                //    DotNetCASClientServiceValidate client = new DotNetCASClientServiceValidate();
                //    principal = client.AuthenticatePrincipal(System.Web.HttpContext.Current.Request, System.Web.HttpContext.Current.Response);
                //}
                //if (principal != null && principal.isAutheniticated)
                //{
                //    Session["IsAuthenticated"] = true;
                //    username = principal.userName;
                //    setAutheniticated(username);
                //}
                //else if (username == "")
                //{
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return false;
            }
        }

        private void setAutheniticated(string username)
        {
            try
            {
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                CSF_Users_DAO objUserDao = new CSF_Users_DAO();
                var user = objUserDao.GetByUserName(username);
                if (user == null)
                {
                    CSF_Users obj = new CSF_Users();
                    obj.UserName = username;
                    obj.FullName = username;
                    obj.Status = 1;
                    obj.RegisterDate = DateTime.Now;
                    obj.Email = username + "@monre.gov.vn";
                    obj.Password = Encryptor.MD5Hash("e10adc3949ba59abbe56e057f20f883e");
                    MyContext.CSF_Users.Add(obj);
                    MyContext.SaveChanges();
                    if (obj.ID > 0)
                    {
                        int IDRegistedGroup = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDRegistedGroup"]);
                        CSF_UserRole objUR = new CSF_UserRole();
                        objUR.RoleID = IDRegistedGroup;
                        objUR.UserID = obj.ID;
                        MyContext.CSF_UserRole.Add(objUR);
                        MyContext.SaveChanges();
                    }
                }
                FormsAuthentication.SetAuthCookie(username, false);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }

        #endregion Login,Logout

        public PartialViewResult DangNhap(string title)
        {
            try
            {
                ViewBag.TITLE = title;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return PartialView();
                }
                else
                    return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult _Blank(string title)
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ThongKeTruyCap(string title)
        {
            try
            {
                ViewBag.TITLE = title;
                DataSet tmpDs = new DataSet();
                string source_xml = Utilities.SiteURL() + "/SoLanTruyCap.xml";
                tmpDs.ReadXml(source_xml);
                ViewBag.TongTruyCap = tmpDs.Tables[0].Rows[0]["hits"].ToString();
                ViewBag.SoNgTruyCapHomNay = tmpDs.Tables[0].Rows[0]["today_hit"].ToString();
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        #region PartialView-Home

        public PartialViewResult SliderNews(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.ISFOCUS == 1).Where(s => s.CateKey == "tin-hoat-dong" || s.CateKey == "tin-chuyen-nganh" || s.CateKey == "tin-tuc-su-kien" || s.CateKey == "tin-cong-nghe" || s.CateKey == "su-kien").OrderByDescending(s => s.CREATEDATE).Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult TinTucSuKienNoiBat(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.ISFOCUS == 1).OrderByDescending(s => s.CREATEDATE).Take(5).ToList();

                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult TinTucSuKienNoiBat1(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.ISFOCUS == 1).Where(s => s.CateKey == "tin-hoat-dong" || s.CateKey == "tin-chuyen-nganh" || s.CateKey == "tin-tuc-su-kien" || s.CateKey == "tin-cong-nghe" || s.CateKey == "su-kien" || s.CateKey == "di-tich-lich-su-van-hoa").OrderByDescending(s => s.CREATEDATE).Take(5).ToList();

                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult TinHoatDong(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "tin-hoat-dong").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult TinCongNghe(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "tin-cong-nghe").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ThongTinChiDaoDieuHanh(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "chi-dao-dieu-hanh").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ThongTinTuyenTruyen(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "thong-tin-tuyen-truyen").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ThongTinCL_DH_QH_KH(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "chien-luoc-quy-hoach-ke-hoach").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ThongTinDuAnHangMuc(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "du-an-hang-muc-dau-tu").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult ChuongTrinhDeTaiKhoaHoc(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                var listData = GetCacheTinBai().Where(s => s.CateKey == "chuong-trinh-de-tai-khoa-hoc").Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult HoiDap(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var listData = MyContext.CMS_Questions.Where(s => s.Publish == true).OrderByDescending(s => s.CreateDate).Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult VanBan(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                var listData = GetCacheVanBan().Where(s => s.Publish == true).OrderByDescending(s => s.IssuedDate).Take(5).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult DSLichHop(string title, string category, string date)
        {
            try
            {
                ViewBag.TITLE = title;
                ViewBag.CATE = category;

                List<LichHop> listLH = getDataLH(date);
                return PartialView(listLH);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult LienKetWebsite(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var listData = MyContext.CMS_Links.Where(x => x.Shows == true).OrderBy(x => x.Order).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult BannerQuangCao(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var listData = MyContext.CMS_AdImages.Where(x => x.Publish == true && x.Location == "right").OrderBy(x => x.Orders).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult BannerQuangCao_Center(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                DT_WebGISEntities MyContext = new DT_WebGISEntities();
                var listData = MyContext.CMS_AdImages.Where(x => x.Publish == true && x.Location == "center").OrderBy(x => x.Orders).ToList();
                return PartialView(listData);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        #region DATE

        public List<LichHop> getDataLH(string date)
        {
            if (date == null)
            {
                date = DateTime.Now.ToString("dd/MM/yyyy");
            }
            ViewBag.DATE = date;
            string[] arrDate = date.Split('/');
            int _ngay = Convert.ToInt32(arrDate[0]);
            int _thang = Convert.ToInt32(arrDate[1]);
            int _nam = Convert.ToInt32(arrDate[2]);
            List<DateTime> listWeeks = getListWeeks(Convert.ToDateTime(date));
            int startWeek = 0;
            int endWeek = 0;
            for (int i = 0; i < listWeeks.Count() - 1; i++)
            {
                if (_ngay >= listWeeks[i].Day && _ngay < listWeeks[i + 1].Day)
                {
                    startWeek = listWeeks[i].Day;
                    endWeek = listWeeks[i + 1].Day - 1;
                    break;
                }
                else
                {
                    startWeek = listWeeks[i + 1].Day;
                }
            }
            if (endWeek == 0)
            {
                string d = "01/" + (_thang + 1) + "/" + _nam;
                endWeek = Convert.ToDateTime(d).AddDays(-1).Day;
            }
            DT_WebGISEntities MyContext = new DT_WebGISEntities();
            var listData = MyContext.CMS_Schedules.Where(x => x.StartDate.Value.Day >= startWeek
                && x.StartDate.Value.Day <= endWeek).ToList();

            #region GanDuLieuLichHop

            //List<CMS_Leader> lLeader = MyContext.CMS_Leader.ToList();
            //CMS_Schedules schedule;
            //for (int i = startWeek; i <= endWeek; i++)
            //{
            //    var data = listData.Where(x => x.StartDate.Value.Day == i).ToList();
            //    DateTime dtime = new DateTime(_nam, _thang, i, 0, 0, 0);
            //    int dow = Convert.ToInt32(dtime.DayOfWeek);//bỏ thứ 7, cn: cn=0
            //    if (data == null || data.Count() < 1 && dow != 6 && dow != 0)
            //    {
            //        foreach (var item in lLeader)
            //        {
            //            schedule = new CMS_Schedules();
            //            schedule.Title = "Làm việc tại cơ quan";
            //            schedule.Leaders = item.Position + " " + item.Name;
            //            schedule.StartDate = dtime.AddHours(8);
            //            MyContext.CMS_Schedules.Add(schedule);
            //            schedule = new CMS_Schedules();
            //            schedule.Title = "Làm việc tại cơ quan";
            //            schedule.Leaders = item.Position + " " + item.Name;
            //            schedule.StartDate = dtime.AddHours(13);
            //            MyContext.CMS_Schedules.Add(schedule);
            //        }
            //    }
            //}
            //MyContext.SaveChanges();
            listData = MyContext.CMS_Schedules.Where(x => x.StartDate.Value.Day >= startWeek
                && x.StartDate.Value.Day <= endWeek).ToList();

            #endregion GanDuLieuLichHop

            List<LichHop> listLH = new List<LichHop>();
            LichHop lh;
            for (int i = startWeek; i <= endWeek; i++)
            {
                string da = i + "/" + _thang + "/" + _nam;
                int dow = Convert.ToInt32(Convert.ToDateTime(da).DayOfWeek);
                if (dow != 6 && dow != 0)
                {
                    lh = new LichHop();
                    lh.data = listData.Where(x => x.StartDate.Value.Day == i).ToList();
                    lh.thu = getThu(Convert.ToDateTime(da).DayOfWeek.ToString());
                    lh.ngay = da;
                    listLH.Add(lh);
                }
            }
            return listLH;
        }

        public string getThu(string ngay)
        {
            switch (ngay)
            {
                case "Monday":
                    return "Thứ hai";

                case "Tuesday":
                    return "Thứ ba";

                case "Wednesday":
                    return "Thứ tư";

                case "Thursday":
                    return "Thứ năm";

                case "Friday":
                    return "Thứ sáu";

                case "Saturday":
                    return "Thứ bảy";

                default:
                    return "Chủ nhật";
            }
        }

        public List<DateTime> getListWeeks(DateTime date)
        {
            try
            {
                var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
                // then filter the only the start of weeks
                var weekends = (from d in dates
                                where d.DayOfWeek == DayOfWeek.Monday
                                select d).ToList();
                if (weekends[0].Day != 1)
                {
                    var firstday = from d in dates
                                   where d.Day == 1
                                   select d;
                    weekends.InsertRange(0, firstday);
                }
                return weekends;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion DATE

        public PartialViewResult LienHe(string title, string category)
        {
            try
            {
                ViewBag.abc = title;
                ViewBag.CATE = category;
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult LienHe(string title, string category, CMS_Contact obj, FormCollection fc)
        {
            try
            {
                ViewBag.abc = title;
                ViewBag.CATE = category;
                if (ModelState.IsValid)
                {
                    CMS_Contact_DAO objDAO = new CMS_Contact_DAO();
                    obj.Title = fc["xTitle"].ToString();
                    int ReturnID = objDAO.Insert(obj);
                    if (ReturnID > 0)
                    {
                        SetAlert("Gửi thông tin liên hệ thành công", AlertType.Success);
                    }
                }
                CMS_Contact n = new CMS_Contact();
                return PartialView(null);
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public PartialViewResult SitePages(string title)
        {
            try
            {
                ViewBag.TITLE = title;
                return PartialView();
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return PartialView();
            }
        }

        public JsonResult sodotrang()
        {
            try
            {
                DT_WebGISEntities ett = new DT_WebGISEntities();
                bool ISADMIN = false;
                bool ISBLANK = false;

                #region jsTreePage

                List<jsTree> jsTreeList = new List<jsTree>();
                jsTree objTree = new jsTree();
                var dataAll = ett.CSF_Pages.Where(x => x.IsAdmin == ISADMIN && x.IsBlank == ISBLANK && x.IsActive == true).OrderBy(x => x.Order).ToList();
                var data = dataAll.Where(x => x.ParentID == 0);
                foreach (var item in data)
                {
                    objTree = new jsTree();
                    objTree.id = item.ID;
                    objTree.text = item.Name;

                    objTree.children = getChild(item.ID, dataAll);
                    jsTreeList.Add(objTree);
                }

                #endregion jsTreePage

                var jsonResults = new { jsTreeList, state = true };
                return Json(jsonResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<jsTree> getChild(int ParentID, List<CSF_Pages> dataAll)
        {
            try
            {
                List<jsTree> jsTreeList = new List<jsTree>();
                jsTree objTree;
                var data = dataAll.Where(x => x.ParentID == ParentID);
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        objTree = new jsTree();
                        objTree.id = item.ID;
                        objTree.text = item.Name;
                        objTree.children = getChild(item.ID, dataAll);
                        jsTreeList.Add(objTree);
                    }
                }
                return jsTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion PartialView-Home

        #region CACHEDATA

        private double SoNgayCache = 365;

        public void Cache_TinBai()
        {
            try
            {
                CMS_News_DAO objNewDao = new CMS_News_DAO();
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> lstNews = objNewDao.LayTinBaiCongKhaiTheoCateKey("");
                objCache.Remove("TinBai");
                objCache.Add("TinBai", lstNews, DateTime.Now.AddDays(SoNgayCache));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }

        public List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> GetCacheTinBai()
        {
            try
            {
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result> listObjs = new List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>();
                if (objCache["TinBai"] != null)
                {
                    listObjs = (List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>)objCache["TinBai"];
                }
                else
                {
                    Cache_TinBai();
                    listObjs = (List<CMS_News_LayTinBaiCongKhaiTheoCateKey_Result>)objCache["TinBai"];
                }
                return listObjs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return null;
            }
        }

        public void Cache_VanBan()
        {
            try
            {
                CMS_Documents_DAO objDAO = new CMS_Documents_DAO();
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_Documents_LayTatCa_Result> lstNews = objDAO.Search("", 0, 0, 0).Where(s => s.Publish == true).ToList();
                objCache.Remove("VanBan");
                objCache.Add("VanBan", lstNews, DateTime.Now.AddDays(SoNgayCache));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }

        public List<CMS_Documents_LayTatCa_Result> GetCacheVanBan()
        {
            try
            {
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_Documents_LayTatCa_Result> listObjs = new List<CMS_Documents_LayTatCa_Result>();
                if (objCache["VanBan"] != null)
                {
                    listObjs = (List<CMS_Documents_LayTatCa_Result>)objCache["VanBan"];
                }
                else
                {
                    Cache_VanBan();
                    listObjs = (List<CMS_Documents_LayTatCa_Result>)objCache["VanBan"];
                }
                return listObjs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return null;
            }
        }

        public void Cache_ThongBao()
        {
            try
            {
                CMS_Notifications_DAO objDAO = new CMS_Notifications_DAO();
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_Notifications_LayTatCa_Result> data = objDAO.GetAll().ToList();
                objCache.Remove("ThongBao");
                objCache.Add("ThongBao", data, DateTime.Now.AddDays(SoNgayCache));
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
            }
        }

        public List<CMS_Notifications_LayTatCa_Result> GetCacheThongBao()
        {
            try
            {
                MemoryCache objCache = MemoryCache.Default;
                List<CMS_Notifications_LayTatCa_Result> listObjs = new List<CMS_Notifications_LayTatCa_Result>();
                if (objCache["ThongBao"] != null)
                {
                    listObjs = (List<CMS_Notifications_LayTatCa_Result>)objCache["ThongBao"];
                }
                else
                {
                    Cache_ThongBao();
                    listObjs = (List<CMS_Notifications_LayTatCa_Result>)objCache["ThongBao"];
                }
                return listObjs;
            }
            catch (Exception ex)
            {
                Logs.WriteLog(ex);
                return null;
            }
        }

        #endregion CACHEDATA
    }
}
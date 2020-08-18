using WebApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Xml;

namespace WebApp.Controllers
{
    public class RSSTinTucController : Controller
    {
        //
        // GET: /RSSTinTuc/
        HomeController objHomeController = new HomeController();
        public PartialViewResult DanhSachRSS(string title, string category)
        {
            try
            {
                ViewBag.TITLE = title;
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView();
            }
        }
        public ActionResult RSSTheoChuyenMuc(string cm, string iNew)
        {
            try
            {
                string catekey = cm;
                int iNews = iNew != null ? Convert.ToInt32(iNew) : 0;
                SyndicationFeed feed = null;
                string siteTitle, description, siteUrl;
                siteTitle = "Danh sách tin tức";
                if (cm=="tin-khoa-hoc-cong-nghe-cua-bo")
                {
                    siteTitle = "Tin khoa học công nghệ của Bộ";
                }
                if (cm == "tin-khoa-hoc-cong-nghe-trong-nuoc")
                {
                    siteTitle = "Tin Khoa học Công nghệ trong nước";
                }
                if (cm == "tin-khoa-hoc-cong-nghe-quoc-te")
                {
                    siteTitle = "Tin Khoa học Công nghệ quốc tế";
                }
                if (cm == "su-kien-khoa-hoc-cong-nghe")
                {
                    siteTitle = "Sự kiện Khoa học Công nghệ";
                }
                siteUrl = Utilities.UrlContent("su-kien-khoa-hoc-cong-nghe", "", "tt", "");
                description = "";
               // CMS_News_DAO objDAO = new CMS_News_DAO();
                var c = objHomeController.GetCacheTinBai().Where(s => s.CateKey == catekey).Take(5).ToList();
                var d = objHomeController.GetCacheTinBai().Where(s => s.CateKey == catekey).Where(x => x.ID == iNews).ToList();
                List<SyndicationItem> items = new List<SyndicationItem>();
                string imageUrl = "";
                foreach (var i in c)
                {
                    string abc = Utilities.UrlContent(cm, GetPlainText(i.TITLE, 200), "tt", i.ID.ToString());
                    //string img = "<a title=" + i.TITLE + " href=" + abc + "> <img src=" + abc + " alt='ảnh tin bài'> </a>";
                     imageUrl = Utilities.SiteURL_Resources() + i.PICTURE;
                    SyndicationItem item = new SyndicationItem
                    {
                        Title = new TextSyndicationContent(i.TITLE),
                        Content = new TextSyndicationContent(GetPlainText(i.CONTENTS, 200)), //here content may be Html content so we should use plain text
                        PublishDate = Convert.ToDateTime(i.CREATEDATE),
                    };
   
                    item.Links.Add(new SyndicationLink(new Uri(abc), "alternate", "Link Title", "text/html", 1000));
                    item.AttributeExtensions.Add(new XmlQualifiedName("myAttribute", ""), "someValue");
                  
                    items.Add(item);
                }
                feed = new SyndicationFeed(siteTitle, description, new Uri(siteUrl));
                feed.Items = items;
                return new RSSResult { feedData = feed };
            }
            catch (Exception ex)
            {
                return PartialView();
            }
        }
        private string GetPlainText(string htmlContent, int length = 0)
        {
            string HTML_TAG_PATTERN = "<.*?>";
            string plainText = Regex.Replace(htmlContent, HTML_TAG_PATTERN, string.Empty);
            return length > 0 && plainText.Length > length ? plainText.Substring(0, length) : plainText;
        }

    }
    public class RSSResult : ActionResult
    {
        public SyndicationFeed feedData { get; set; }
        public string contentType = "rss";

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";
            //check request is for Atom or RSS
            if (context.HttpContext.Request.QueryString["type"] != null && context.HttpContext.Request.QueryString["type"].ToString().ToLower() == "atom")
            {
                //Atom Feed
                context.HttpContext.Response.ContentType = "application/atom+xml";
                var rssFormatter = new Atom10FeedFormatter(feedData);
                using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output, new XmlWriterSettings { Indent = true }))
                {
                    rssFormatter.WriteTo(writer);
                }
            }
            else
            {
                //RSS Feed
                context.HttpContext.Response.ContentType = "application/rss+xml";
                var rssFormatter = new Rss20FeedFormatter(feedData);
                using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output, new XmlWriterSettings { Indent = true }))
                {
                    rssFormatter.WriteTo(writer);
                }
            }

        }
    }
}

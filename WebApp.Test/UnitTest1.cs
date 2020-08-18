using System;
using WebApp.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebApp.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] strTypes =
            {
                "hs_khac",
                "hs_chutruong_hdks",
                "hs_dongcua",
                "hs_khaithac",
                "hs_quyhoach",
                "hs_thamdo_ks",
                "hs_thanhtra",
                "hs_tinhtien_cqktks"
            };

            foreach (var VARIABLE in strTypes)
            {
                var xxx = Encryptor.MD5Hash(VARIABLE);
                Console.WriteLine(xxx);
            }
        }}
}

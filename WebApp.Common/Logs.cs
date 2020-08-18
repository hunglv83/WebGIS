using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace WebApp.Common
{
    public static class Logs
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Logs));
        public static void WriteLog(Exception ex)
        {
            logger.Error(ex);
        }

        public static void LogError(string msg)
        {
            logger.Error(msg);
        }

        public static void LogInfo(string msg)
        {
            logger.Info(msg);
        }
    }

}

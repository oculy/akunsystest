using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace webAkunsys.Common
{
    public class SiteSettings
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PondsBigBangEntities"].ConnectionString;
            }
        }

        public static string AdminPageSize
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminPageSize"];
            }
        }


        public static string MergeImageRoute
        {
            get
            {
                return ConfigurationManager.AppSettings["MergeImageRoute"].ToString();
            }
        }

        public static string ImageOverlay
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageOverlay"].ToString();
            }
        }

        public static string UserUploadRoute
        {
            get
            {
                return ConfigurationManager.AppSettings["UserUploadRoute"].ToString();
            }
        }

        public static string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["Domain"];
            }
        }
        public static string GAAccount
        {
            get
            {
                return ConfigurationManager.AppSettings["GAAccount"];
            }
        }

        public static string HashTag
        {
            get
            {
                return ConfigurationManager.AppSettings["HashTag"];
            }
        }

        public static DateTime StartDate
        {
            get
            {
                DateTime result = DateTime.MinValue;
                DateTime.TryParseExact(ConfigurationManager.AppSettings["StartDate"], "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
                return result;
            }
        }
        public static string TwitterKey
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterKey"];
            }
        }

        public static string TwitterSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["TwitterSecret"];
            }
        }


        public static string FacebookAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppId"];
            }
        }

        public static string FacebookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookAppSecret"];
            }
        }
        public static string FacebookScope
        {
            get
            {
                return ConfigurationManager.AppSettings["FacebookScope"];
            }
        }
        public static string SpesificRootScheduller
        {
            get
            {
                return ConfigurationManager.AppSettings["SpesificRootScheduller"];
            }
        }
        public static string FrontImageOverlay
        {
            get
            {
                return ConfigurationManager.AppSettings["FrontImageOverlay"];
            }
        }
        public static string PinImageOverlay
        {
            get
            {
                return ConfigurationManager.AppSettings["PinImageOverlay"];
            }
        }

        #region FB Auto Post
        public static string FbPostName
        {
            get
            {
                return ConfigurationManager.AppSettings["FbPostName"];
            }
        }
        public static string FbPostCaption
        {
            get
            {
                return ConfigurationManager.AppSettings["FbPostCaption"];
            }
        }
        public static string FbPostMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["FbPostMessage"];
            }
        }

        public static string ImageUrlFBShare
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageUrlFBShare"];
            }
        }




        #endregion


        public static string FBCanvasPageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FBCanvasPageUrl"];
            }
        }

        public static string FBAppPage
        {
            get
            {
                return ConfigurationManager.AppSettings["FBAppPage"];
            }
        }

        public static string FBCanvasPageName
        {
            get
            {
                return ConfigurationManager.AppSettings["FBCanvasPageName"];
            }
        }

        public static string SecureDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["SecureDomain"];
            }
        }

        public static string TargetURL
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetURL"];
            }
        }

        public static string DomainPageFB
        {
            get
            {
                return ConfigurationManager.AppSettings["DomainPageFB"];
            }
        }

        public static string SecureDomainPageFB
        {
            get
            {
                return ConfigurationManager.AppSettings["SecureDomainPageFB"];
            }
        }


        public static string ProxyServerIP
        {
            get
            {
                return ConfigurationManager.AppSettings["ProxyServerIP"];
            }
        }
        public static string ProxyUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["ProxyUserName"];
            }
        }
        public static string ProxyPass
        {
            get
            {
                return ConfigurationManager.AppSettings["ProxyPass"];
            }
        }

        public static bool IsLiveServer
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["IsLiveSite"]);
            }
        }

        public static string SiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteUrl"];
            }
        }

        public static string BlockedTwitterName
        {
            get
            {
                return ConfigurationManager.AppSettings["BlockedTwitterName"];
            }
        }

        public static int MaxFeedData
        {
            get
            {
                int x = 0;
                int.TryParse(ConfigurationManager.AppSettings["MaxFeedData"], out x);

                return x;
            }
        }

        public static int AddMaxFeedData
        {
            get
            {
                int result = 0;
                int.TryParse(ConfigurationManager.AppSettings["AddMaxFeedData"].ToString(), out result);
                return result;
            }
        }
    }
}

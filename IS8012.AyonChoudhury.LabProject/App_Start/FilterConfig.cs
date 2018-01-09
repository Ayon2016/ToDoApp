using System.Web;
using System.Web.Mvc;

namespace IS8012.AyonChoudhury.LabProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

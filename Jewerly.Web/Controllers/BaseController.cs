using System.Web.Mvc;
using Jewerly.Domain;

namespace Jewerly.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DataManager DataManager;

        public BaseController(DataManager dataManager)
        {
            DataManager = dataManager;
        }
    }
}
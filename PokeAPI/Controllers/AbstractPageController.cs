using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace PokeAPI.Controllers
{
    public abstract class AbstractPageController : Controller
    {
        private readonly IFileProvider fileProvider;

        public AbstractPageController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        protected PhysicalFileResult GetPage(string name)
        {
            var filePath = fileProvider.GetFileInfo($"/html/{name}.html").PhysicalPath;
            var contentType = "text/html";
            return PhysicalFile(filePath, contentType);
        }
    }
}

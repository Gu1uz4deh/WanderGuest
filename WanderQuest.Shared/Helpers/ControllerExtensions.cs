using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WanderQuest.Shared.Helpers
{
    public static class ControllerExtensions
    {
        public static async Task<string> RenderAsync(HttpContext httpContext, ViewContext viewContext, string componentName, object arguments = null)
        {
            if (viewContext == null)
                throw new ArgumentNullException(nameof(viewContext), "ViewContext cannot be null");

            var serviceProvider = httpContext.RequestServices;
            var helper = (IViewComponentHelper)serviceProvider.GetRequiredService(typeof(IViewComponentHelper));
            ((IViewContextAware)helper).Contextualize(viewContext);

            using var writer = new StringWriter();
            var result = await helper.InvokeAsync(componentName, arguments);
            result.WriteTo(writer, HtmlEncoder.Default);

            return writer.ToString();
        }
    }
}

#pragma checksum "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/Error/ErrorHandler.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d64d2e14dd0de388dfcfbeb6e66785996756bc42"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_ErrorHandler), @"mvc.1.0.view", @"/Views/Error/ErrorHandler.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/_ViewImports.cshtml"
using DiplomaSolution.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/_ViewImports.cshtml"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d64d2e14dd0de388dfcfbeb6e66785996756bc42", @"/Views/Error/ErrorHandler.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"194b1df8c39a7b893b82bf2916f75ed4e8521c37", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_ErrorHandler : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<h1>Error occured ( ");
#nullable restore
#line 2 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
               Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - error code )</h1>\r\n<h1>Error data ( ");
#nullable restore
#line 3 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
            Write(ViewBag.OP);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - original path )</h1>\r\n<h1>Error data ( ");
#nullable restore
#line 4 "/Users/llasapg/Desktop/DiplomWork/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
            Write(ViewBag.QS);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - query string )</h1>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<int> Html { get; private set; }
    }
}
#pragma warning restore 1591

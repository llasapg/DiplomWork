#pragma checksum "/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/Views/Error/ErrorHandler.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d64d2e14dd0de388dfcfbeb6e66785996756bc42"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Error_ErrorHandler), @"mvc.1.0.view", @"/Views/Error/ErrorHandler.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Error/ErrorHandler.cshtml", typeof(AspNetCore.Views_Error_ErrorHandler))]
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
#line 1 "/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/Views/_ViewImports.cshtml"
using DiplomaSolution.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d64d2e14dd0de388dfcfbeb6e66785996756bc42", @"/Views/Error/ErrorHandler.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5f210b085394b8a48f1876bb38df3d82d0e9960a", @"/Views/_ViewImports.cshtml")]
    public class Views_Error_ErrorHandler : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(12, 20, true);
            WriteLiteral("<h1>Error occured ( ");
            EndContext();
            BeginContext(33, 5, false);
#line 2 "/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
               Write(Model);

#line default
#line hidden
            EndContext();
            BeginContext(38, 39, true);
            WriteLiteral(" - error code )</h1>\r\n<h1>Error data ( ");
            EndContext();
            BeginContext(78, 10, false);
#line 3 "/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
            Write(ViewBag.OP);

#line default
#line hidden
            EndContext();
            BeginContext(88, 42, true);
            WriteLiteral(" - original path )</h1>\r\n<h1>Error data ( ");
            EndContext();
            BeginContext(131, 10, false);
#line 4 "/Users/llasapg/Projects/DiplomaSolution/DiplomaSolution/Views/Error/ErrorHandler.cshtml"
            Write(ViewBag.QS);

#line default
#line hidden
            EndContext();
            BeginContext(141, 22, true);
            WriteLiteral(" - query string )</h1>");
            EndContext();
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

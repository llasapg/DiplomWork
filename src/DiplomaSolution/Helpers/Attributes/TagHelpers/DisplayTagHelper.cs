using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DiplomaSolution.Helpers.Attributes.TagHelpers
{
    [HtmlTargetElement(Attributes = "Display")]
    public class DisplayTagHelper : TagHelper
    {
        [HtmlAttributeName("Display")] // this two names should map to binder input correct values
        public bool IsVisible { get; set; } = true;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if(IsVisible)
            {
                await Task.CompletedTask;
            }
            else
            {
                output.TagName = null;
                output.SuppressOutput(); // removes all the content of element where this tag helper was upplied 
                output.Attributes.RemoveAll("Display"); // removes this attribute in every case ( we dont need to render this body )
            }
        }
    }
}

using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DiplomaSolution.Helpers.Attributes.TagHelpers
{
    [HtmlTargetElement("info")] //<info> </info>
    public class InfoSelectrorTagHelper : TagHelper // name of the tag helper will be --> <Info-Selectror>
    {
        #region Attributes
        [HtmlAttributeName("add-customOne")] // attribute first
        public bool CustomOne { get; set; }

        [HtmlAttributeName("add-customTwo")] // attribute second
        public bool CustomTwo { get; set; }

        #endregion

        private HtmlEncoder HtmlEncoder { get; set; } // Html encoder to encode html before we render it ( right case to work with )

        public InfoSelectrorTagHelper(HtmlEncoder htmlEncoder)
        {
            HtmlEncoder = htmlEncoder;
        }

        /// <summary>
        /// Main method to perform html generation and other stuff
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {            
            output.TagName = "div"; // setting up the output element

            output.TagMode = TagMode.StartTagAndEndTag; // setting tag mode

            var stringContent = "<h1>Hello there!!!</h1>";

            var result = HtmlEncoder.Encode(stringContent);

            output.Content.SetHtmlContent(result);

            return Task.CompletedTask;
        }
    }
}

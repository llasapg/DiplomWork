using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DiplomaSolution.Helpers.Attributes
{
    public class EmailPatternAttribute : ValidationAttribute
    {
        private List<string> rigthTemplates = new List<string> { "gmail.com", "softheme.com", "yahoo.com" };

        public override bool IsValid(object value)
        {
            var email = value.ToString().Split("@"); // Creates array with 2 objects - one is before this element and etc...

            foreach (var item in rigthTemplates)
            {
                Trace.WriteLine($"{item} - correct remplate");

                if (email[1] == item)
                    return true;
            }

            ErrorMessage = $"Please try to use valid domain template like {rigthTemplates}";
                
            return false;
        }
    }
}

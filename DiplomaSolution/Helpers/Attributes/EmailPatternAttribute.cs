using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Helpers.Attributes
{
    public class EmailPatternAttribute : ValidationAttribute
    {
        public string[] RigthTemplates { get; set; }

        public override bool IsValid(object value)
        {
            var email = value.ToString().Split("@");

            foreach (var item in RigthTemplates)
            {
                if (email[1] == item)
                    return true;
            }

            ErrorMessage = $"Please try to use valid domain templates";
                
            return false;
        }
    }
}

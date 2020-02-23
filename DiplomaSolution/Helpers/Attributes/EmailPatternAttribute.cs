using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Helpers.Attributes
{
    public class EmailPatternAttribute : ValidationAttribute
    {
        public string[] RigthTemplates { get; set; }

        /// <summary>
        /// Overrided method to validate provided email ( if true --> email template is good )
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value) // Value - is our provided data
        {
            var email = value.ToString().Split("@"); // There we get 2 strings ( 1 - ... ,  2 - @gmail.com )

            foreach (var item in RigthTemplates)
            {
                if (email[1] == item)
                    return true;
            }

            ErrorMessage = $"Please try to use valid domain templates"; // Property of ValidationAttribute
                
            return false;
        }
    }
}

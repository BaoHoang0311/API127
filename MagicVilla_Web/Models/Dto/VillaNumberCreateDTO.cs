using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
        public string SpecialDetails2 { get; set; }
        [DateLessThan("End")]
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        [Calc(new string [] { "Value2","Value3" })]
        public int Value1 { get;set;}
        public int Value2 { get; set; }
        public int Value3 { get; set; }

    }
    public class CalcAttribute : ValidationAttribute
    {
        private readonly string[] _comparisonProperty;
        string propertyName;
        public CalcAttribute(string[] comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int value1 = (int)value;
            int value2 = 0;
            int value3 = 0;
            foreach (string field in _comparisonProperty)
            {
                propertyName = field;
                PropertyInfo property = validationContext.ObjectType.GetProperty(field);
                if (property == null)
                    return new ValidationResult(string.Format("Property '{0}' is undefined.", field));

                if(property.Name == "Value2")
                {
                    value2 = (int)property.GetValue(validationContext.ObjectInstance, null);
                }
                if (property.Name == "Value3")
                {
                    value3 = (int)property.GetValue(validationContext.ObjectInstance, null);
                }

            }
            if(value1+value2 > value3)
                return new ValidationResult("value 1 + value 2 > value3");

            return ValidationResult.Success;
        }
    }
    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                //return new ValidationResult(ErrorMessage);
                return new ValidationResult("date begin > end");

            return ValidationResult.Success;
        }
    }

}

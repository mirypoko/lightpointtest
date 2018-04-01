using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class GetFilterViewModel
    {
        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "Count can not be less than 1.")]
        public int? Count { get; set; }

        [RegularExpression("^[1-9]\\d*$", ErrorMessage = "Offset can not be less than 0.")]
        public int? Offset { get; set; }
    }
}

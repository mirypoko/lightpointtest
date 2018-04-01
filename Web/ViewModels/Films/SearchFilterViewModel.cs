using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Films
{
    public class SearchProductFilterViewModel : GetFilterViewModel
    {
        public string SearchString { get; set; }

        public int? ShopId { get; set; }
    }
}

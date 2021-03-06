using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer.LocomotiveService;
using WebApp.Helpers;
using static DataLayer.Models.ModelItem;
using static DataLayer.Models.Products.Locomotive;

namespace WebApp.Pages.Locomotive
{
    public class IndexModel : PageModel
    {
        public List<ListLocomotiveDto> Locomotives { get; set; }
        [BindProperty(SupportsGet = true)]
        public QueryOptions QueryOptions { get; set; }
        public int NumberOfPages { get; set; }
        
        public List<string> AllTags { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<EScale> Scales { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<EEpoch> Epochs { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<ELocoType> LocoTypes { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<EControl> Controls { get; set; }

        readonly ILocomotiveService _locomotiveService;

        public IndexModel(ILocomotiveService locomotiveService)
        {
            _locomotiveService = locomotiveService;
        }

        public void OnGet(int? pg = 1)
        {
            // Prepare query options
            if (Tags?.Count > 0
                || Scales?.Count > 0
                || Epochs?.Count > 0
                || Controls?.Count > 0
                || LocoTypes?.Count > 0)
            {
                QueryOptions.FilterOptions = new FilterOptions
                {
                    Tags = Tags,
                    Scales = Scales,
                    Epochs = Epochs,
                    Controls = Controls,
                    LocoTypes = LocoTypes
                };
            }

            QueryOptions.PageNumber = (ushort)pg;

            // Do query
            var result = _locomotiveService.GetList(QueryOptions);

            // Prepare data for view
            Locomotives = result.Item1.ToList();
            QueryOptions.PageNumber = result.Item2;
            NumberOfPages = result.Item3;
            
            AllTags = _locomotiveService.GetTags().ToList();
        }

        public IActionResult OnGetAddToBasket(int id)
        {
            HttpContext.AddProductToBasket(id);

            return RedirectToPage("/Basket");
        }
    }
}

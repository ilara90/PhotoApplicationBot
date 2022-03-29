using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhotoApplication.Models.View
{
    public class IndexViewModel
    {
        public IEnumerable<Image>? Images { get; set; }
        public SelectList? Tags { get; set; }
        public List<int>? TagIds { get; set; }
    }
}

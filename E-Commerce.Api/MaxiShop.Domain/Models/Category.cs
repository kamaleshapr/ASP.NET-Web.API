using MaxiShop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace MaxiShop.Domain.Models
{
    public class Category : BaseModel
    {

        [Required]
        public string Name { get; set; }
    }
}

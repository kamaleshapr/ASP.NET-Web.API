using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Business.DTO.Product
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }
        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; }

        public string Specification { get; set; }

        public double Price { get; set; }
    }
}

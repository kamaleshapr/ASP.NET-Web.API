using MaxiShop.Business.DTO.Product;
using MaxiShop.Business.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        //[Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult> Get()
        {
            var items = await _productService.GetAllAsync();
            return Ok(items);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("id")]
        //[Route("Detail")]
        public async Task<ActionResult> Get(int id)
        {
            var item = await _productService.GetByIdAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Filter")]
        public async Task<ActionResult> GetByFilter(int? categoryId, int? brandId)
        {
            var items = await _productService.GetByFilterAsync(categoryId, brandId);
            return Ok(items);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CreateAsync(dto);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<ActionResult> Update(UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await _productService.GetByIdAsync(dto.Id);
            if (item == null)
            {
                return NotFound();
            }
            await _productService.UpdateAsync(dto);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _productService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _productService.DeleteAsync(id);
            return Ok();
        }
    }
}

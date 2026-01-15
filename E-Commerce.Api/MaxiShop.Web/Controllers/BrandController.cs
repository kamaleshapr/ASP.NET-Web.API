using MaxiShop.Business.DTO.Brand;
using MaxiShop.Business.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto createBrandDto)
        {
            await _brandService.CreateAsync(createBrandDto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _brandService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("Detail")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _brandService.GetByIdAsync(id);
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBrandDto updateBrandDto)
        {
            await _brandService.UpdateAsync(updateBrandDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteAsync(id);
            return Ok();
        }
    }
}

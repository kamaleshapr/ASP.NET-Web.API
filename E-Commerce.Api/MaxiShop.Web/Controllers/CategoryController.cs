using MaxiShop.Business.Contracts;
using MaxiShop.Business.DTO.Category;
using MaxiShop.Business.Services.Interface;
using MaxiShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaxiShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICatergoryService  _catergoryService;
        public CategoryController(ICatergoryService catergoryService)
        {
            _catergoryService = catergoryService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _catergoryService.GetAllAsync();
            return Ok(items);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        [Route("Detail")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _catergoryService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _catergoryService.CreateAsync(dto);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await _catergoryService.GetByIdAsync(dto.Id);
            if (item == null)
            {
                return NotFound("Specific Item not found in the record to update");
            }
            await _catergoryService.UpdateAsync(dto);
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _catergoryService.GetByIdAsync(id);
            if (item == null || id == 0)
            {
                return NotFound("Specific Item not found in the record to delete");
            }
            await _catergoryService.DeleteAsync(id);
            return Ok();
        }
    }
}

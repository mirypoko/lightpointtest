using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataBaseModels.Goods;
using Domain.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Servises.Interfaces.Products;
using Web.Extensions;
using Web.GuidelinesControllers;
using Web.ViewModels;

namespace Web.Controllers.Products
{
    /// <summary>
    /// Shop modes API.
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public sealed class ShopModesController : Controller, IFullRestApiController<int, ShopMode>
    {
        private readonly IShopModesService _dataService;

        public ShopModesController(IShopModesService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Get count of shop mode.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CountAsync()
        {
            return Ok(await _dataService.CountAsync());
        }

        /// <summary>
        /// Get list of shop mode.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get shop mode. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<ShopMode>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(GetFilterViewModel filter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var entities = await _dataService.GetListAsync(filter.Count, filter.Offset);
            return Ok(entities);
        }

        /// <summary>
        /// Get shop mode by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get shop mode. Error list in response body.</response>
        /// <response code="404">The shop mode with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ShopMode), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest(new List<string> { "Id can not be less than 1." });
            }

            var entity = await _dataService.GetByIdOrDefaultAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        /// <summary>
        /// Chang shop mode.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The shop mode was changed.</response>
        /// <response code="400">Failed to change shop mode. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ShopMode), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody] ShopMode putViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var result = await _dataService.UpdateAsync(putViewModel);

            if (result.Succeeded)
            {
                return Ok(putViewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Create new shop mode.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The shop mode was created.</response>
        /// <response code="400">Failed to create shop mode. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ShopMode), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody] ShopMode postViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var result = await _dataService.CreateAsync(postViewModel);

            if (result.Succeeded)
            {
                return new CreatedResult(Request.GetDisplayUrl() + $"/{postViewModel.Id}", postViewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete shop mode.
        /// </summary>
        /// <param name="id">Id of shop mode to delete.</param>
        /// <response code="200">The shop mode was deleted.</response>
        /// <response code="400">Failed to delete shop mode. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        /// <response code="404">The shop mode with the received id was not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest(new List<string> { "Id can not be less than 1" });
            }

            var entity = await _dataService.GetByIdOrDefaultAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            var result = await _dataService.DeleteAsync(entity);

            return result.ToActionResult();
        }
    }
}

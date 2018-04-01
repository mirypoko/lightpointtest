using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Products;
using Domain.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Servises.Interfaces.Products;
using Web.Extensions;
using Web.GuidelinesControllers;
using Web.ViewModels;
using Web.ViewModels.Films;

namespace Web.Controllers.Products
{
    /// <summary>
    /// Shops API.
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public sealed class ShopsController : Controller, IFullRestApiController<int, Shop>
    {
        private readonly IShopsService _dataService;

        public ShopsController(IShopsService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Get count of shop.
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
        /// Get list of shop.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get shop. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<Shop>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(GetFilterViewModel filter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var entities = await _dataService.GetListAsync(filter.Count, filter.Offset);
            return Ok(entities);
        }

        /// <summary>
        /// Get shop by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get shop. Error list in response body.</response>
        /// <response code="404">The shop with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Shop), 200)]
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
        /// Chang shop.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The shop was changed.</response>
        /// <response code="400">Failed to change shop. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Shop), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody] Shop putViewModel)
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
        /// Create new shop.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The shop was created.</response>
        /// <response code="400">Failed to create shop. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Shop), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody] Shop postViewModel)
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
        /// Delete shop.
        /// </summary>
        /// <param name="id">Id of shop to delete.</param>
        /// <response code="200">The shop was deleted.</response>
        /// <response code="400">Failed to delete shop. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        /// <response code="404">The shop with the received id was not found.</response>
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

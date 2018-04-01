using System.Collections.Generic;
using System.Threading.Tasks;
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
    /// Products API.
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public sealed class ProductsController : Controller, IFullRestApiController<int, Product>
    {
        private readonly IProductsService _dataService;

        public ProductsController(IProductsService productsService)
        {
            _dataService = productsService;
        }

        /// <summary>
        /// Search products.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get products. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> SearchAsync(SearchProductFilterViewModel filter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());
            return Ok(await _dataService.SearchAsync(filter.Count, filter.Offset, filter.SearchString, filter.ShopId));
        }

        /// <summary>
        /// Get count of products with filter.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpGet("countFilter")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CountAsync(string searchString, int? shopId)
        {
            return Ok(await _dataService.CountAsync(searchString, shopId));
        }


        /// <summary>
        /// Get count of products.
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
        /// Get list of products.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get products. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(GetFilterViewModel filter)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var products = await _dataService.GetListAsync(filter.Count, filter.Offset);
            return Ok(products);
        }

        /// <summary>
        /// Get product by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get product. Error list in response body.</response>
        /// <response code="404">The product with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), 200)]
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
        /// Chang product.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The product was changed.</response>
        /// <response code="400">Failed to change product. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody] Product putViewModel)
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
        /// Create new product.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The product was created.</response>
        /// <response code="400">Failed to create product. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody] Product postViewModel)
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
        /// Delete product.
        /// </summary>
        /// <param name="id">Id of product to delete.</param>
        /// <response code="200">The product was deleted.</response>
        /// <response code="400">Failed to delete product. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        /// <response code="404">The product with the received id was not found.</response>
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

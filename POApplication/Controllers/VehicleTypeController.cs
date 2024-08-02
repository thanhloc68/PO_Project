using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POApplication.Data;
using POApplication.Models;
using POApplication.Wrappers;

namespace POApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypeController : Controller
    {
        private readonly POInfos _dbContext;
        public VehicleTypeController(POInfos dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetList(int pageIndex, int pageSize)
        {
            return await GetPagedDataAsync(pageIndex, pageSize, _dbContext.VehicleType);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int pageIndex, int pageSize)
        {
            var query = _dbContext.VehicleType.Where(x => x.ID == id);
            return await GetPagedDataAsync(pageIndex, pageSize, query);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleType([FromBody] VehicleType vehicleType)
        {
            if (vehicleType == null) return BadRequest("Không tìm thấy dữ liệu");
            var query = new VehicleType
            {
                Name = vehicleType.Name,
            };
            await _dbContext.AddAsync(query);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVehicleType(int id, [FromBody] VehicleType vehicleType)
        {
            if (id <= 0 && vehicleType == null) return BadRequest("Dữ liệu không hợp lệ");
            var check = await _dbContext.VehicleType.FindAsync(id);
            if (check == null) return BadRequest();
            check.Name = vehicleType.Name;
            try
            {
                _dbContext.VehicleType.Update(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            try
            {
                var poInfo = await _dbContext.VehicleType.FindAsync(id);
                if (poInfo == null) return BadRequest("Không tìm thấy ID");
                _dbContext.Remove(poInfo);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        private async Task<IActionResult> GetPagedDataAsync(int pageIndex, int pageSize, IQueryable<VehicleType> query)
        {
            try
            {
                var list = await query
                    .OrderBy(x => x.ID)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var count = await query.CountAsync();
                var totalPage = (int)Math.Ceiling(count / (double)pageSize);

                var response = new PagedResponse<VehicleType>(list, pageIndex, totalPage)
                {
                    Message = "Data fetched successfully."
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new Response<string>
                {
                    Succeeded = false,
                    Message = "An error occurred while fetching data.",
                    Errors = new[] { ex.Message }
                };

                return StatusCode(500, errorResponse);
            }
        }

    }
}

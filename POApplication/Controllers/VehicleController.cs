using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POApplication.Data;
using POApplication.Models;
using POApplication.Wrappers;

namespace POApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : Controller
    {
        private readonly POInfos _dbContext;
        public VehicleController(POInfos dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetList(int pageIndex, int pageSize)
        {
            var query = _dbContext.Vehicle.Include(x => x.Transporter).Include(x => x.VehicleType).AsNoTracking().AsQueryable();
            return await GetPagedDataAsync(pageIndex, pageSize, query);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int pageIndex, int pageSize)
        {
            var query = _dbContext.Vehicle.Where(x => x.ID == id);
            return await GetPagedDataAsync(pageIndex, pageSize, query);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] Vehicle vehicle)
        {
            if (vehicle == null) return BadRequest("Không tìm thấy dữ liệu");
            var query = new Vehicle
            {
                LicensePlate = vehicle.LicensePlate,
                RegistrationNumber = vehicle.RegistrationNumber,
                Capacity = vehicle.Capacity,
                Status = vehicle.Status,
                TransporterID = vehicle.TransporterID,
                VehicleTypeID = vehicle.VehicleTypeID
            };
            await _dbContext.AddAsync(query);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            if (id <= 0 && vehicle == null) return BadRequest("Dữ liệu không hợp lệ");
            var check = await _dbContext.Vehicle.FindAsync(id);
            if (check == null) return BadRequest();
            vehicle.LicensePlate = vehicle.LicensePlate;
            vehicle.RegistrationNumber = vehicle.RegistrationNumber;
            vehicle.Capacity = vehicle.Capacity;
            vehicle.Status = vehicle.Status;
            vehicle.TransporterID = vehicle.TransporterID;
            vehicle.VehicleTypeID = vehicle.VehicleTypeID;
            try
            {
                _dbContext.Vehicle.Update(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            try
            {
                var check = await _dbContext.Vehicle.FindAsync(id);
                if (check == null) return BadRequest("Không tìm thấy ID");
                _dbContext.Remove(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        private async Task<IActionResult> GetPagedDataAsync(int pageIndex, int pageSize, IQueryable<Vehicle> query)
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

                var response = new PagedResponse<Vehicle>(list, pageIndex, totalPage)
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

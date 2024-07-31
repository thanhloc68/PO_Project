using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POApplication.Data;
using POApplication.Models;
using POApplication.Wrappers;

namespace POApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoDetailController : Controller
    {
        private readonly POInfos _dbContext;

        public PoDetailController(POInfos dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            return await GetPagedDataAsync(pageIndex, pageSize, _dbContext.PODetail);
        }
        [HttpPost]
        public async Task<IActionResult> AddPODetail([FromBody] PoDetail poDetail)
        {
            try
            {
                if (poDetail == null) return BadRequest("Không thấy dữ liệu");
                var query = new PoDetail()
                {
                    NumberOfTrips = poDetail.NumberOfTrips,
                    QuantityAccordingToFlowMeter = poDetail.QuantityAccordingToFlowMeter,
                    QuantityAccordingToTankMeasurement = poDetail.QuantityAccordingToTankMeasurement,
                    QuantityAccordingToVehicleWeight = poDetail.QuantityAccordingToVehicleWeight,
                    DateShipping = poDetail.DateShipping
                };
                await _dbContext.AddAsync(query);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePoDetail(int id, [FromBody] PoDetail poDetailDto)
        {
            if (id <= 0 && poDetailDto == null) return BadRequest("Dữ liệu không hợp lệ");
            var check = await _dbContext.PODetail.FindAsync(id);
            if (check == null) return BadRequest();
            check.NumberOfTrips = poDetailDto.NumberOfTrips;
            check.QuantityAccordingToFlowMeter = poDetailDto.QuantityAccordingToFlowMeter;
            check.QuantityAccordingToTankMeasurement = poDetailDto.QuantityAccordingToTankMeasurement;
            check.QuantityAccordingToVehicleWeight = poDetailDto.QuantityAccordingToVehicleWeight;
            check.DateShipping = poDetailDto.DateShipping;
            try
            {
                _dbContext.PODetail.Update(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoDetail(int id)
        {
            try
            {
                var poDetail = await _dbContext.PODetail.FindAsync(id);
                if (poDetail == null) return BadRequest("Không tìm thấy ID");
                _dbContext.Remove(poDetail);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        private async Task<IActionResult> GetPagedDataAsync(int pageIndex, int pageSize, IQueryable<PoDetail> query)
        {
            try
            {
                var list = await query
                    .OrderBy(x => x.id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var count = await query.CountAsync();
                var totalPage = (int)Math.Ceiling(count / (double)pageSize);

                var response = new PagedResponse<PoDetail>(list, pageIndex, totalPage)
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

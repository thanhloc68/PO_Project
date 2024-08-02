using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POApplication.Data;
using POApplication.Models;
using POApplication.Wrappers;
namespace POApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransporterController : Controller
    {
        private readonly POInfos _dbContext;
        public TransporterController(POInfos dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            var query = _dbContext.Transporter.AsNoTracking();
            return await GetPagedDataAsync(pageIndex, pageSize, query);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int pageIndex, int pageSize)
        {
            var query = _dbContext.Transporter.Where(x => x.ID == id).AsNoTracking();
            return await GetPagedDataAsync(pageIndex, pageSize, query);
        }
        [HttpPost]
        public async Task<IActionResult> AddTransporter([FromBody] Transporter transporter)
        {
            if (transporter == null) return BadRequest("Không tìm thấy dữ liệu");
            var query = new Transporter
            {
                Name = transporter.Name,
                Phone = transporter.Phone,
                Address = transporter.Address,
                Notes = transporter.Notes,
            };
            await _dbContext.AddAsync(query);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTransporter(int id, [FromBody] Transporter transporter)
        {
            if (id <= 0 && transporter == null) return BadRequest("Dữ liệu không hợp lệ");
            var check = await _dbContext.Transporter.FindAsync(id);
            if (check == null) return BadRequest();
            check.Name = transporter.Name;
            check.Phone = transporter.Phone;
            check.Address = transporter.Address;
            check.Notes = transporter.Notes;
            try
            {
                _dbContext.Transporter.Update(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransporter(int id)
        {
            try
            {
                var poInfo = await _dbContext.Transporter.FindAsync(id);
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
        private async Task<IActionResult> GetPagedDataAsync(int pageIndex, int pageSize, IQueryable<Transporter> query)
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

                var response = new PagedResponse<Transporter>(list, pageIndex, totalPage)
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

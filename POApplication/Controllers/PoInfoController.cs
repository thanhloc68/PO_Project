using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POApplication.Data;
using POApplication.Models;
using POApplication.Wrappers;

namespace POApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoInfoController : Controller
    {
        private readonly POInfos _dbContext;

        public PoInfoController(POInfos dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex, int pageSize)
        {
            return await GetPagedDataAsync(pageIndex, pageSize, _dbContext.POInfomation);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int pageIndex, int pageSize)
        {
            var query = _dbContext.POInfomation.Where(x => x.id == id);
            return await GetPagedDataAsync(pageIndex, pageSize, _dbContext.POInfomation);
        }
        [HttpPost]
        public async Task<IActionResult> AddPoInfo([FromBody] POInfomation pOInfomation)
        {
            if (pOInfomation == null) return BadRequest("Không tìm thấy dữ liệu");
            var query = new POInfomation
            {
                MaterialCode = pOInfomation.MaterialCode,
                CustomerRef = pOInfomation.CustomerRef,
                GrossWeight = pOInfomation.GrossWeight,
                Line = pOInfomation.Line,
                MaterialDescription = pOInfomation.MaterialDescription,
                NetWeight = pOInfomation.NetWeight,
                Quantity = pOInfomation.Quantity,
                SupplierRef = pOInfomation.SupplierRef,
                UOM = pOInfomation.UOM,
                Volumn = pOInfomation.Volumn
            };
            await _dbContext.AddAsync(query);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePoInfo(int id, [FromBody] POInfomation pOInfomation)
        {
            if (id <= 0 && pOInfomation == null) return BadRequest("Dữ liệu không hợp lệ");
            var check = await _dbContext.POInfomation.FindAsync(id);
            if (check == null) return BadRequest();
            check.MaterialCode = pOInfomation.MaterialCode;
            check.CustomerRef = pOInfomation.CustomerRef;
            check.GrossWeight = pOInfomation.GrossWeight;
            check.Line = pOInfomation.Line;
            check.MaterialDescription = pOInfomation.MaterialDescription;
            check.NetWeight = pOInfomation.NetWeight;
            check.Quantity = pOInfomation.Quantity;
            check.SupplierRef = pOInfomation.SupplierRef;
            check.UOM = pOInfomation.UOM;
            check.Volumn = pOInfomation.Volumn;
            try
            {
                _dbContext.POInfomation.Update(check);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoInfo(int id)
        {
            try
            {
                var poInfo = await _dbContext.POInfomation.FindAsync(id);
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
        private async Task<IActionResult> GetPagedDataAsync(int pageIndex, int pageSize, IQueryable<POInfomation> query)
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

                var response = new PagedResponse<POInfomation>(list, pageIndex, totalPage)
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

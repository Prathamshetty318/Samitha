using DubaiChaRaja.Filters;
using DubaiChaRaja.Models;
using DubaiChaRaja.Service;
using Microsoft.AspNetCore.Mvc;

[RequireSession]
[ApiController]
[Route("api/[controller]")]
public class FestivalController : ControllerBase
{
    private readonly IFestivalService _festivalService;

    public FestivalController(IFestivalService festivalService)
    {
        _festivalService = festivalService;
    }

    [HttpPost("add")]
    public IActionResult AddRecord([FromBody] FestivalRecord record)
    {
        var hasaccess = HttpContext.Session.GetInt32("hasaccess");
        Console.WriteLine($"HasAccess from session: {hasaccess}");

        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null || hasaccess != 1)
            return new JsonResult(new { message = "You do not have permission to add records. Please contact the admin." })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };


        _festivalService.InsertFestivalRecords(record.Description, record.Amount, record.Type, record.Year, userId.Value);
        return Ok(new { message = "Record inserted successfully!" });
    }

    [HttpGet("get-by-year/{year}")]
    public async Task<IActionResult> GetByYear(int year)
    {
        var records = await _festivalService.GetByYearAsync(year);
        return Ok(records);
    }


    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var hasaccess = HttpContext.Session.GetInt32("hasaccess");

            if (userId == null || hasaccess != 1)
                return new JsonResult(new { message = "You do not have permission to add records. Please contact the admin." })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

            var deleted = await _festivalService.DeleteFestivalRecordAsync(id);
            if (deleted)
                return Ok();
            else
                return NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Upload Error: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}



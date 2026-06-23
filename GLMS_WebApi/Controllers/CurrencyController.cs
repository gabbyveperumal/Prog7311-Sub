using GLMS_Project.Services.Currency;
using Microsoft.AspNetCore.Mvc;
namespace GLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
        {
            private readonly CurrencyConversionService _currencyService;

            public CurrencyController(CurrencyConversionService currencyService)
            {
                _currencyService = currencyService;
            }

            [HttpGet("convert")]
            public async Task<IActionResult> Convert([FromQuery] decimal amountUsd)
            {
                if (amountUsd <= 0)
                    return BadRequest(new { error = "Amount must be greater than zero." });

                try
                {
                    var zar = await _currencyService.ConvertUsdToZarAsync(amountUsd);
                    return Ok(new { amountUsd, amountZar = zar });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }



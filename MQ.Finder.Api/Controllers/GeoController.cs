using Microsoft.AspNetCore.Mvc;
using MQ.Finder.Api.Helpers;
using MQ.Finder.Api.Params;
using MQ.Finder.Data.Finder;

namespace MQ.Finder.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class GeoController(IDataFinder dataFinder, ILogger<GeoController> logger) : ControllerBase
    {
        private readonly IDataFinder _dataFinder = dataFinder;
        private readonly ILogger<GeoController> _logger = logger;

        [HttpGet()]
        [Route("ip/location")]
        public virtual IActionResult FindLocationByIp([FromQuery] IpRequest ipRequest)
        {
            try
            {
                // do Trim
                var locations = _dataFinder.FindLocationByIp(ipRequest.Ip.Trim());
                return Ok(ViewModelHelper.ConvertLocations(locations));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding location by IP.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet()]
        [Route("city/locations")]
        public virtual IActionResult FindLocationsByCity([FromQuery] CityRequest cityRequest)
        {
            try
            {
                // do Trim as database also contains whitespaces in city names
                var locations = _dataFinder.FindLocationsByCity(cityRequest.City.Trim());
                return Ok(ViewModelHelper.ConvertLocations(locations));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding location by IP.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}

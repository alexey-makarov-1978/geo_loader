using System.ComponentModel.DataAnnotations;

namespace MQ.Finder.Api.Params
{
    public record CityRequest
    {
        [Required(ErrorMessage = "City is required.")]
        [MaxLength(24, ErrorMessage = "City length is 24 symbols max.")]
        public required string City { get; set; }
    }

}

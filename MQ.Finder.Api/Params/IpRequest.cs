using System.ComponentModel.DataAnnotations;

namespace MQ.Finder.Api.Params
{
    public record IpRequest
    {
        [Required(ErrorMessage = "IP address is required.")]
        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$",
            ErrorMessage = "Invalid IPv4 address format.")]
        public required string Ip { get; set; }
    }
}

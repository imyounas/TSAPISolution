using System.ComponentModel.DataAnnotations;
using TSWebAPI.Common.Constants;

namespace TSWebAPI.Common.Options
{
    public class MockAPIClientSettings
    {
        [Required]
        public string BaseUrl { get; set; } = string.Empty;
        public string AuthorizationScheme { get; set; } = string.Empty;
        [Required]
        public string UserAgent { get; set; } = AppConstants.DEFAULT_USER_AGENT;
        [Required]
        public string AcceptHeader { get; set; } = AppConstants.DEFAULT_ACCEPT_HEADER;
        [Required]
        public string ProductEndPoint { get; set; } = AppConstants.DEFAULT_PRODUCT_ENDPOINT;
    }
}

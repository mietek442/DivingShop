using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Common.Services.UrlHelper
{
    public class UrlHelpers : IUrlHelpers
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlHelpers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePictureUrl(Guid? id)
        {
            var host = _httpContextAccessor.HttpContext?.Request?.Host.Value;
            if (id == null||id == Guid.Empty)
            {
                return null;
            }
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            return $"https://{host}/api/files/{id}";
        }
    }
}

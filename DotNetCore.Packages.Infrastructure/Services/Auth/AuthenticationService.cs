using DotNetCore.Packages.Application.Common.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetCore.Packages.Infrastructure.Services.Auth;

 public class AuthenticationService : IAuhtenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthenticationService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public bool HasPermission(string permission)
        {
            var userPermissions = GetUserPermissions();
            return userPermissions.Contains(permission);
        }

        public void EnsurePermissionForHandler<THandler>()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new UnauthorizedAccessException("Access Denied: No HTTP Context available.");
            }

            var formattedPermission = FormatHandlerName<THandler>();

            _logger.LogInformation($"Checking Permission: {formattedPermission}");

            if (!HasPermission(formattedPermission))
            {
                _logger.LogWarning($"403 Forbidden: Missing Permission - {formattedPermission}");
                throw new UnauthorizedAccessException($"403 Forbidden: User does not have permission - {formattedPermission}");
            }
        }

        private List<string> GetUserPermissions()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User.Claims;
            if (userClaims == null)
            {
                return new List<string>();
            }

            return userClaims
                .Where(c => c.Type == "Permission")  // JWT içindeki "Permission" claim'lerini alıyoruz
                .Select(c => c.Value)
                .ToList();
        }

        private static string FormatHandlerName<THandler>()
        {
            return typeof(THandler).Name
                .Replace("Handler", "")
                .Replace("Command", "")
                .Replace("Query", "");
        }
    }
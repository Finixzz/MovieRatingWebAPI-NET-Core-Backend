using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TokenDecryptor
{
    public class GetUserJWTAsync
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserJWTAsync(IHttpContextAccessor _httpContextAccessor)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }
        public async Task<object> executeAsync()
        {
            Task<string> getJWT = new Task<string>(() => _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
            getJWT.Start();
            return await getJWT;
        }
    }
}

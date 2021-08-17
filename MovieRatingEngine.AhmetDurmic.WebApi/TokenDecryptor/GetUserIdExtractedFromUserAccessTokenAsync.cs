using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TokenDecryptor
{
    public class GetUserIdExtractedFromUserAccesTokenAsync : IRequest<object>
    {
    }

    public class GetUserIdExtractedFromUserAccesTokenAsyncHandler : IRequestHandler<GetUserIdExtractedFromUserAccesTokenAsync, object>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GetUserIdExtractedFromUserAccesTokenAsyncHandler(IHttpContextAccessor _httpContextAccessor)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }
        public async Task<object> Handle(GetUserIdExtractedFromUserAccesTokenAsync request, CancellationToken cancellationToken)
        {

            string JWT = (string)await new GetUserJWTAsync(_httpContextAccessor).executeAsync();

            var handler = new JwtSecurityTokenHandler();

            return handler.ReadJwtToken(JWT).Payload["Id"].ToString();
        }
    }
}

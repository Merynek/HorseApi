using HorseApi.Models;
using HorseApi.Models.BindingModels;
using HorsiApi.Models.BindingModels;
using SqlKata.Execution;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HorseApi.Services
{
    public class UserService : BaseService
    {
        private readonly ITokenService _tokenService;

        public UserService(QueryFactory db, ITokenService tokenService) : base(db)
        {
            this._tokenService = tokenService;
        }

        public ResponseModel Login(LoginBindingModel model)
        {
            User user = _db.Query("Users").Where("username", model.username).First<User>();

            if (user != null)
            {
                var usersClaims = new[]
                {
                    new Claim("ID", user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, user.role)
                };

                var generatedToken = _tokenService.GenerateAccessToken(usersClaims, model.permanent);
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(generatedToken);
                var refreshToken = _tokenService.GenerateRefreshToken();

                refreshTokenField(user.ID, refreshToken);
                _response.SetResponseData(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken,
                    expire = getTokenExpireTime(generatedToken)
                });
            }
            else
            {
                _response.SetError(1, "Fail Login");
            }

            return _response;
        }

        public ResponseModel RefreshToken(RefreshTokenBindingModel model)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(model.token);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            User user = _db.Query("Users").Where("username", username).First<User>();
            if (user == null || user.refreshToken != model.refreshToken)
            {
                _response.SetError(2, "Fail refresh token");
                return _response;
            }

            var generatedToken = _tokenService.GenerateAccessToken(principal.Claims, false);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(generatedToken);
            var refreshToken = _tokenService.GenerateRefreshToken();

            refreshTokenField(user.ID, refreshToken);

            _response.SetResponseData(new
            {
                token = jwtToken,
                refreshToken = refreshToken,
                expire = getTokenExpireTime(generatedToken)
            });

            return _response;
        }

        public ResponseModel CheckToken(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;

            User user = _db.Query("Users").Where("username", username).First<User>();
            if (user == null)
            {
                _response.SetError(3, "Fail check token");
                return _response;
            }
            _response.SetResponseData(new
            {
                username = user.username
            });

            return _response;
        }

        public void Registration(RegistrationBindingModel model)
        {
            _db.Query("Users").Insert(
                new
                {
                    username = model.username,
                    password = model.password,
                    role = "user"
                });
        }

        private void refreshTokenField(int userID, string refreshToken)
        {
            _db.Query("Users").Where("ID", userID).Update(new
            {
                refreshToken = refreshToken
            });
        }

        private long getTokenExpireTime(JwtSecurityToken token)
        {
            return (long)(token.ValidTo - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}

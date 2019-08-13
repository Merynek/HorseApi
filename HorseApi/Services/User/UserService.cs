using HorseApi.Enums;
using HorseApi.Models;
using HorseApi.Models.BindingModels;
using HorsiApi.Models.BindingModels;
using SqlKata.Execution;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            User user = _db.Query("Users").Where("email", model.email).First<User>();

            if (user != null)
            {
                if (hashPassword(model.password) == user.password)
                {
                    var usersClaims = new[]
                                    {
                        new Claim("ID", user.ID.ToString()),
                        new Claim(ClaimTypes.Email, user.email),
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.Role, user.role)
                    };

                    var generatedToken = _tokenService.GenerateAccessToken(usersClaims, model.permanent);
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(generatedToken);
                    var refreshToken = _tokenService.GenerateRefreshToken();

                    refreshTokenField(user.ID, refreshToken);
                    _response.SetResponseData(createTokenResponse(jwtToken, refreshToken, generatedToken));
                }
                else
                {
                    _response.SetError(ReponseErrorType.INVALID_PASSWORD);
                }
            }
            else
            {
                _response.SetError(ReponseErrorType.USER_NOT_FOUND);
            }

            return _response;
        }

        public ResponseModel RefreshToken(RefreshTokenBindingModel model)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(model.token);
            var username = principal.Identity.Name;

            User user = _db.Query("Users").Where("username", username).First<User>();
            if (user == null || user.refreshToken != model.refreshToken)
            {
                _response.SetError(ReponseErrorType.INVALID_REFRESH_TOKEN);
                return _response;
            }

            var generatedToken = _tokenService.GenerateAccessToken(principal.Claims, false);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(generatedToken);
            var refreshToken = _tokenService.GenerateRefreshToken();

            refreshTokenField(user.ID, refreshToken);
            _response.SetResponseData(createTokenResponse(jwtToken, refreshToken, generatedToken));

            return _response;
        }

        public ResponseModel CheckToken(string token)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;

            User user = _db.Query("Users").Where("username", username).First<User>();
            if (user == null)
            {
                _response.SetError(ReponseErrorType.USER_NOT_FOUND);
                return _response;
            }
            user.password = null;
            _response.SetResponseData(user);

            return _response;
        }

        public void Registration(RegistrationBindingModel model)
        {
            _db.Query("Users").Insert(
                new
                {
                    username = model.username,
                    password = hashPassword(model.password),
                    email = model.email,
                    role = "user"
                });
        }

        private object createTokenResponse(string jwtToken, string refreshToken, JwtSecurityToken generatedToken)
        {
            return new
            {
                token = jwtToken,
                refreshToken = refreshToken,
                expire = getTokenExpireTime(generatedToken)
            };
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

        private string hashPassword(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}

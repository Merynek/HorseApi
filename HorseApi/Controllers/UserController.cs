using HorseApi.Models.BindingModels;
using HorseApi.Services;
using HorsiApi.Controllers;
using HorsiApi.Models.BindingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HorseApi.Controllers
{
    [Route("api/user")]
    public class UserController : BaseApiController
    {
        private readonly UserService _userService;
        public UserController(ITokenService tokenService, IConfiguration configuration) : base(configuration)
        {
            _userService = new UserService(_db, tokenService);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login([FromBody]LoginBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var response = _userService.Login(model);

            if (response.OK)
            {
                return Ok(response.ResponseData);
            }
            return BadRequest(response.Error);
        }

        [HttpPost]
        [Authorize]
        [Route("refreshToken")]
        public IActionResult RefreshToken([FromBody]RefreshTokenBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            var response = _userService.RefreshToken(model);

            if (response.OK)
            {
                return Ok(response.ResponseData);
            }
            return BadRequest(response.Error);
        }

        [HttpPost]
        [Authorize]
        [Route("checkToken")]
        public IActionResult CheckToken([FromBody]CheckTokenBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }
            var response = _userService.CheckToken(model.token);

            if (response.OK)
            {
                return Ok(response.ResponseData);
            }
            return BadRequest(response.Error);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("registration")]
        public IActionResult Registration([FromBody]RegistrationBindingModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest(ModelState);
            }

            _userService.Registration(model);
            return Ok();
        }
    }
}

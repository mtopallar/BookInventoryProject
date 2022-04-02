using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Entities.Concrete;
using Entities.DTOs;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getalluserdetailswithrolesview")]
        public IActionResult GetAllUserDetailsWithRolesView()
        {
            var result = _userService.GetAllUserDetailsWithRoles();
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        
        
        [HttpGet("getuserdetailswithrolesbyuserid")]
        public IActionResult GetUserDetailsWithRolesByUserId(int userId)
        {
            var result = _userService.GetUserDetailsWithRolesByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        

        [HttpPost("update")]
        public IActionResult Update(UserForUpdateDto userForUpdateDto)
        {
            var result = _userService.Update(userForUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        //front taraftan kullanıcının rollerine göre yönlendirilme yapılmalı.
        [HttpPost("deleteforadmin")]
        public IActionResult DeleteForAdmin(DeleteForAdminWithAttemptingUserIdDto deleteForAdminWithAttemptingUserIdDto)
        {
            var result = _userService.DeleteForAdmin(deleteForAdminWithAttemptingUserIdDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("deleteforuser")]
        public IActionResult DeleteForUser(UserForDeleteDto userForDeleteDto)
        {
            var result = _userService.DeleteForUser(userForDeleteDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

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
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly IUserOperationClaimService _userOperationClaimService;

        public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
        {
            _userOperationClaimService = userOperationClaimService;
        }

        [HttpGet("getauthenticateduserclaims")]
        public IActionResult GetAuthenticatedUserClaimDtos(int userId)
        {
            var result = _userOperationClaimService.GetUserClaimDtosByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            var result = _userOperationClaimService.Add(userOperationClaimWithAttemptingUserIdDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("update")] //angular fe de kullanmadım.
        public IActionResult Update(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            var result = _userOperationClaimService.Update(userOperationClaimWithAttemptingUserIdDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpPost("delete")]
        public IActionResult Delete(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            var result = _userOperationClaimService.Delete(userOperationClaimWithAttemptingUserIdDto);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

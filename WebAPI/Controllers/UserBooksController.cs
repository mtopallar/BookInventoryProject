using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private IUserBookService _userBookService;

        public UserBooksController(IUserBookService userBookService)
        {
            _userBookService = userBookService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll(int userId)
        {
            var result = _userBookService.GetAll(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getauthorsinuserlibrary")]
        public IActionResult GetAuthorsFromUsersLibrary(int userId)
        {
            var result = _userBookService.GetAuthorsFromUsersLibrary(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getgenresinuserlibrary")]
        public IActionResult GetGenresFromUsersLibrary(int userId)
        {
            var result = _userBookService.GetGenresFromUsersLibrary(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getpublishersinuserlibrary")]
        public IActionResult GetPublishersFromUsersLibrary(int userId)
        {
            var result = _userBookService.GetPublishersFromUsersLibrary(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbynoteincluded")]
        public IActionResult GetByNoteIncluded(int userId)
        {
            var result = _userBookService.GetByNoteIncluded(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbypublisherid")]
        public IActionResult GetByPublisherId(int userId,int publisherId)
        {
            var result = _userBookService.GetByPublisherId(userId, publisherId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyauthorid")]
        public IActionResult GetByAuthorId(int userId, int authorId)
        {
            var result = _userBookService.GetByAuthorId(userId, authorId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyauthornativestatue")]
        public IActionResult GetByAuthorNativeSatue(int userId, bool native)
        {
            var result = _userBookService.GetByAuthorNativeStatue(userId, native);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbygenreid")]
        public IActionResult GetByGenreId(int userId, int genreId)
        {
            var result = _userBookService.GetByGenreId(userId, genreId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyreadstatue")]
        public IActionResult GetByReadStatue(int userId, bool readStatue)
        {
            var result = _userBookService.GetByReadStatue(userId, readStatue);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(UserBook userBook)
        {
            var result = _userBookService.Add(userBook);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(UserBook userBook)
        {
            var result = _userBookService.Update(userBook);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(UserBook userBook)
        {
            var result = _userBookService.Delete(userBook);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}

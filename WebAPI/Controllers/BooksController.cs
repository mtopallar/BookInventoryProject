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
    public class BooksController : ControllerBase
    {
        private IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /*
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _bookService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        } */

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _bookService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getallview")]
        public IActionResult GetAllView()
        {
            var result = _bookService.GetAllForAddToLibrary();
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyisbnview")]
        public IActionResult GetByIsbnView(string isbn)
        {
            var result = _bookService.GetByIsbnForAddToLibrary(isbn);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getlistbybooknameview")]
        public IActionResult GetListByBookNameView(string bookName)
        {
            var result = _bookService.GetListByBookNameForAddToLibrary(bookName);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getlistbypublisheridview")]
        public IActionResult GetListByPublisherId(int publisherId)
        {
            var result = _bookService.GetListByPublisherIdForAddToLibrary(publisherId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getlistbyauthoridview")]
        public IActionResult GetListByAuthorIdView(int authorId)
        {
            var result = _bookService.GetListByAuthorIdForAddToLibrary(authorId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getlistbynativestatueview")]
        public IActionResult GetListByNativeStatue(bool native)
        {
            var result = _bookService.GetListByNativeStatueForAddToLibrary(native);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getlistbygenreidview")]
        public IActionResult GetListByGenreIdView(int genreId)
        {
            var result = _bookService.GetListByGenreIdForAddToLibrary(genreId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Book book)
        {
            var result = _bookService.Add(book);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Book book)
        {
            var result = _bookService.Update(book);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}

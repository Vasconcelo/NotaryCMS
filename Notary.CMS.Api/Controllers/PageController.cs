using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notary.CMS.DataAccess.Interfaces;
using Notary.CMS.DataAccess.Models;
using Notary.CMS.DataAccess.Models.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notary.CMS.Api.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class PageController : ControllerBase
    {

        private readonly IPageRepository _pageRepository;
        private readonly IMapper _mapper;

        public PageController(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }

        // GET: api/<PageController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var pages = _pageRepository.GetPages();
                if (pages == null) return NotFound();

                return Ok(pages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<PageController>/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var page = _pageRepository.GetPage(id);
                if (page == null) return NotFound();

                return Ok(page);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<PageController>
        [HttpPost]
        public IActionResult Create(PageDTO model)
        {
            try
            {
                var page = _mapper.Map<Page>(model);
                _pageRepository.CreatePage(page);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<PageController>/5
        [HttpPut("{id:int}")]
        public IActionResult Edit(PageDTO model, int id)
        {
            try
            {
                var page = _mapper.Map<PageSDTO>(_pageRepository.UpdatePage(model, id));
                return Ok(page);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<PageController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _pageRepository.DeletePage(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

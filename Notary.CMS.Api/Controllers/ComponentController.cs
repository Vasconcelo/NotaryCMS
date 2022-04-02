//using AutoMapper;
//using IdentityServer4.AccessTokenValidation;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Notary.Api.DataAccess.Interfaces;
//using Notary.Api.DataAccess.Models;
//using Notary.Api.DataAccess.Models.DTOs;
//using NotaryApi.Models;
//using NotaryApi.Models.Request;


//namespace NotaryApi.Controllers
//{
//    [Route("api/[controller]")]  
//    [ApiController]
//    public class ComponentController : ControllerBase
//    {
//        private readonly IComponentRepository _componentRepository;
//        private readonly IMapper _mapper;


//        public ComponentController(IComponentRepository componentRepository, IMapper mapper)
//        {
//            _componentRepository = componentRepository;
//            _mapper = mapper;
//        }

//        // GET: ApplicationController
//        [HttpGet]
//        public IActionResult Get()
//        {           
//            try
//            {

//                var listComponents = _componentRepository.GetComponents();
//                if (listComponents == null) return NotFound();

//                return Ok(listComponents);
               
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }


//        }

//        [HttpGet("{id:int}")]
//        public IActionResult Get(int id)
//        {            
//            try
//            {
//                var listComponents = _mapper.Map<ComponentSDTO>(_componentRepository.GetComponent(id));
//                return Ok(listComponents);                

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }


//        }

//        [HttpPost("getComponentByIdentifier")]
//        public IActionResult Get([FromBody] ComponentIdDTO dto)
//        {
//            try
//            {
//                var listComponents = _mapper.Map<ComponentSDTO>(_componentRepository.GetComponentByIdentifier(dto));
//                return Ok(listComponents);

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }


//        }


//        [HttpPost("getComponentByAppId")]
//        [AllowAnonymous]
//        public IActionResult Get([FromBody] DynamicModelDTO model)
//        {
//            try
//            {
//                string result = string.Empty;

//                var component = _componentRepository.GetComponentById(model);
//                if (component == null) NotFound();

//                return Ok(component);
               
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }

//        }

//        // POST: ApplicationController/Create
//        [HttpPost]
//        public IActionResult Create(ComponentDTO model)
//        {         
//            try
//            {               
//                var comp = _mapper.Map<Component>(model);
//                var compCreated = _mapper.Map<ComponentSDTO>(_componentRepository.CreateComponent(comp));
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }


//        }

//        [HttpPut("{id:int}")]
//        public IActionResult Edit(ComponentDTO model, int id)
//        {            
//            try
//            {
//                var app = _mapper.Map<ComponentSDTO>(_componentRepository.UpdateComponent(model, id));
//                return Ok(app);  
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }

            
//        }

//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {           
//            try
//            {
//                using (NotaryDBContext db = new NotaryDBContext())
//                {
//                    Component comp = db.Components.Find(id);
//                    if (comp == null)
//                    {
//                        return NotFound();
//                    }
//                    _componentRepository.DeleteComponent(comp);
//                    return Ok();
//                }
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }

            
//        }
//    }
//}

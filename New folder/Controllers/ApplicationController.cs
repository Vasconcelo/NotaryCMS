using Microsoft.AspNetCore.Mvc;

namespace ApiNotaryNNA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;
        private NotaryDBContext db;


        public ApplicationController(IApplicationRepository applicationRepository, IMapper mapper, NotaryDBContext DBContext)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            db = DBContext;
        }

        // GET: ApplicationController
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var applications = _applicationRepository.GetApplications();
                if (applications == null) return NotFound();

                return Ok(applications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var app = _mapper.Map<ApplicationSDTO>(_applicationRepository.GetApplication(id));
                if (app == null) return NotFound();

                return Ok(app);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: ApplicationController/Create
        [HttpPost]
        public IActionResult Create(ApplicationDTO model)
        {
            try
            {
                var app = _mapper.Map<Application>(model);
                var appCreated = _mapper.Map<ApplicationSDTO>(_applicationRepository.CreateApplication(app));
                return Ok(appCreated);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpPut("{id:int}")]
        public IActionResult Edit(ApplicationDTO model, int id)
        {
            try
            {
                var app = _mapper.Map<ApplicationSDTO>(_applicationRepository.UpdateApplication(model, id));
                return Ok(app);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {

                Application app = db.Applications.Find(Id);
                if (app == null)
                {
                    return NotFound();
                }
                _applicationRepository.DeleteApplication(app);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }



    }
}

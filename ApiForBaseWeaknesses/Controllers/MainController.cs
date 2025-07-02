using ApiForBaseWeaknesses.Dto;
using ApiForBaseWeaknesses.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiForBaseWeaknesses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly BaseService _baseservice;

        public MainController(ILogger<MainController> logger, BaseService baseservice)
        {
            _logger = logger;
            _baseservice = baseservice;
        }

        [HttpGet]
        public string Get()
        {
            bool result = _baseservice.FiilBase();
            return "Успешный успех";
        }

        [HttpPost]
        public string Post([FromBody] VulnerabilitiesDto vuln)
        {
            VulnerabilitiesDto resul = vuln;
            return "Успех";
        }
    }
}

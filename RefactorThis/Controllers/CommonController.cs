using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RefactorThis.Controllers
{
    [ApiController]   
    [Route("api/v1/[Controller]")]
    [Produces("application/json", "application/xml")]    
    public class CommonController : ControllerBase
    {
        
    }
}

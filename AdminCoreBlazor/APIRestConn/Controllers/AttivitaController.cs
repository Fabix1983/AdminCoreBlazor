using Microsoft.AspNetCore.Mvc;
using AdminCoreBlazorClass;

namespace APIRestConn.Controllers
{
    public class AttivitaController : ControllerBase
    {
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(IEnumerable<TipologiaAttivita>), StatusCodes.Status200OK)]
        public IActionResult GetAllTipoAttivita()
        { return Ok(); }

        [HttpGet("[action]/{anno}/{mese}")]
        [ProducesResponseType(typeof(IEnumerable<Attivita>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAttivita(int anno, int mese)
        {
            ModelState.AddModelError("anno e mese", "Periodo Senza Attivita");
            return NotFound(new ValidationProblemDetails(ModelState));
        }

        /*
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Attivita>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(Attivita studente)
        {
            ModelState.AddModelError("Nome", "Nome troppo lungo");
            return BadRequest(ModelState);
            //return Ok(); 
        }

        [HttpPut]
        public IActionResult Put(Attivita studente)
        { return Ok(); }
        */

        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteAttivita(int id)
        { 
            return Ok();      
        }
        
    }
}

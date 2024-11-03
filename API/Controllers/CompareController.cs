
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System;
using Shared.Class;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CompareController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public CompareController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: CompareController/Compare/anno/mese
        [HttpGet("{anno}/{mese}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Compare(int anno, int mese)
        {
            string json = "";
            CompareOUT compareOUT = new CompareOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();
                    int i = 0;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    compareOUT.Status = "OK";
                    compareOUT.Compare = new List<Compare>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        Compare compare = new Compare
                        {
                            TipoAttivita = row["TipoAttivita"].ToString(),
                            ColoreHTML = row["ColoreHTML"].ToString(),
                            Descrizione = row["Descrizione"].ToString(),
                            TotAtt = Convert.ToDecimal(row["TotAtt"])
                        };
                        compareOUT.Compare.Add(compare);
                    }

                    if (i > 0)
                    {
                        compareOUT.StatusError = "";
                    }
                    else
                    {
                        compareOUT.StatusError = "Nessun elemento trovato nel periodo indicato.";
                    }
                }
                catch (Exception e)
                {
                    compareOUT.Status = "KO";
                    compareOUT.StatusError = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(compareOUT, Formatting.None);
            return Json(json);
        }

    }
}

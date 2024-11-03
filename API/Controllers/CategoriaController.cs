
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
    public class CategoriaController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public CategoriaController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: CategoriaController/Categoria/rifCategoria
        [HttpGet("{rifCategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Categoria(int rifCategoria = 1)
        {
            string json = "";
            CategoriaOUT categoriaOUT = new CategoriaOUT();

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
                    SqlCommand cmd = new SqlCommand("EXEC spCategoriaDiSpesaPerMese @RifCat", connection);
                    cmd.Parameters.Add("@RifCat", SqlDbType.Int, 4);
                    cmd.Parameters["@RifCat"].Value = rifCategoria;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    categoriaOUT.Status = "OK";
                    categoriaOUT.Categoria = new List<Categoria>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        Categoria categoria = new Categoria
                        {
                            Descrizione = row["Descrizione"].ToString(),
                            TipoAttivita = row["TipoAttivita"].ToString(),
                            Valore = Convert.ToDecimal(row["Valore"]),
                            Media = Convert.ToDecimal(row["Media"]),
                            TotaleMese = Convert.ToDecimal(row["TotaleMese"]),
                            PercentualeSulTotale = Convert.ToDecimal(row["PercentualeSulTotale"]),
                            Tipologia = row["Tipologia"].ToString(),
                            ColoreHTML = row["ColoreHTML"].ToString(),                                                                           
                        };
                        categoriaOUT.Categoria.Add(categoria);
                    }

                    if (i > 0)
                    {
                        categoriaOUT.StatusError = "";
                    }
                    else
                    {
                        categoriaOUT.StatusError = "Nessuna Categoria di Spesa trovata.";
                    }
                }
                catch (Exception e)
                {
                    categoriaOUT.Status = "KO";
                    categoriaOUT.StatusError = e.ToString();
                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(categoriaOUT, Formatting.None);
            return Json(json);
        }

    }
}

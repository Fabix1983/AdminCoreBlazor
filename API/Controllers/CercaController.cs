using Microsoft.AspNetCore.Mvc;
using System.Data;
using API.Models;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CercaController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public CercaController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: CercaController/Cerca/parola
        [HttpGet("{parola}")]
        public JsonResult Cerca(string parola)
        {
            string json = "";
            CercaOUT cercaOUT = new CercaOUT();
            
            if (parola != "" && !System.String.IsNullOrEmpty(parola))
            {
                var connString = _configuration.GetConnectionString("Default");

                string connStringEscape = connString.ToString().Replace("\\\\","\\");

                // dichiaro il chiamante con Certificato Valido
                connStringEscape = connStringEscape + ";TrustServerCertificate=true";

                using (SqlConnection connection = new SqlConnection(connStringEscape))
                {
                    try
                    {
                        connection.Open();
                        int i = 0;
                        DataTable dt = new DataTable();
                        SqlCommand cmdDB = new SqlCommand("EXEC spCercaAttivitaPerParola @Cerca", connection);
                        cmdDB.Parameters.Add("@Cerca", SqlDbType.VarChar, 150);
                        cmdDB.Parameters["@Cerca"].Value = parola.ToString().ToUpper();
                        SqlDataAdapter adp = new SqlDataAdapter(cmdDB);
                        adp.Fill(dt);

                        cercaOUT.Status = "OK";
                        cercaOUT.Cerca = new List<Cerca>();

                        foreach (DataRow row in dt.Rows)
                        {
                            i++;
                            Cerca cerca = new Cerca
                            {
                                DataSpesa = Convert.ToDateTime(row["DataSpesa"]),
                                Dettagli = row["Dettagli"].ToString(),
                                TipoAttivita = row["TipoAttivita"].ToString(),
                                ColoreHTML = row["ColoreHTML"].ToString(),
                                Tipologia = row["Tipologia"].ToString(),
                                Descrizione = row["Descrizione"].ToString(),
                                Costo = Convert.ToDecimal(row["Costo"])
                            };
                            cercaOUT.Cerca.Add(cerca);
                        }     
                        
                        if (i>0)
                        {
                            cercaOUT.StatusError = "";
                        }
                        else
                        {
                            cercaOUT.StatusError = "Nessuna attivita trovata per la parola cercata.";
                        }
                    }
                    catch (Exception e)
                    {
                        cercaOUT.Status = "KO";
                        cercaOUT.StatusError = e.ToString();
                        
                    }
                    connection.Close();
                }
            }
            else
            {
                cercaOUT.Status = "OK";
                cercaOUT.StatusError = "Parola da cercare non indicata.";
            }

            json = JsonConvert.SerializeObject(cercaOUT, Formatting.Indented);
            return Json(json);
        }

    }
}

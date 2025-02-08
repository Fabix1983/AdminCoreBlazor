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

    public class PrevisioniController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public PrevisioniController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: api/Previsioni/Lista/anno/mese
        [HttpGet("[action]/{anno}/{mese}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Lista(int anno, int mese)
        {
            string json = "";
            PrevisioneOUT previsioneOUT = new PrevisioneOUT();

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

                    string sSQL_Select = "SELECT " +
                                        "   Pre.ID, " +
                                        "   Pre.Giorno, " +
                                        "   Pre.Descrizione, " +
                                        "   Pre.Costo, " +
                                        "   Per.Anno, " +
                                        "   Per.Mese " +
                                        "FROM " +
                                        "   tblPrevisioni Pre " +
                                        "   INNER JOIN tblPeriodi Per ON Per.ID = Pre.RifPeriodo " +
                                        "WHERE " +
                                        "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                        "ORDER BY " +
                                        "   Pre.Giorno ";


                    previsioneOUT.Status = "OK";

                    SqlCommand cmd = new SqlCommand(sSQL_Select, connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    previsioneOUT.Previsione = new List<Previsione>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        Previsione previsione = new Previsione
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            Giorno = Convert.ToInt32(row["Giorno"]),
                            Descrizione = row["Descrizione"].ToString(),
                            Costo = Convert.ToDecimal(row["Costo"])                         
                        };
                        previsioneOUT.Previsione.Add(previsione);
                    }

                    if (i <= 0)
                    {
                        previsioneOUT.StatusError = "Nessuna previsione trovata.";
                    }
                }
                catch (Exception e)
                {
                    previsioneOUT.Status = "KO";
                    previsioneOUT.StatusError = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(previsioneOUT, Formatting.None);
            return Json(json);
        }

        // DELETE: api/Previsioni/Delete/id
        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Delete(int id)
        {
            string json = "";
            GenericOUT deleteOUT = new GenericOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM tblPrevisioni WHERE ID = @iRifPre", connection);
                    command.Parameters.Add("iRifPre", SqlDbType.Int, 4);
                    command.Parameters["iRifPre"].Value = (Int32)id;
                    int iRecordDelete = 0;
                    iRecordDelete = command.ExecuteNonQuery();
                    if (iRecordDelete == 1)
                    {
                        deleteOUT.Status = "OK";
                        deleteOUT.StatusError = "";
                    }
                    else
                    {
                        deleteOUT.Status = "KO";
                        deleteOUT.StatusError = "Errore Previsione da eliminare non trovata!";
                    }
                    connection.Close();
                }
                catch (Exception e)
                {
                    deleteOUT.Status = "KO";
                    deleteOUT.StatusError = "Delete Error " + e.ToString();
                }
            }
            json = JsonConvert.SerializeObject(deleteOUT, Formatting.None);
            return Json(json);
        }

        // POST: api/Previsioni/New/previsione
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult New([FromBody] PrevisioneIN previsione)
        {
            string json = "";
            GenericOUT previsioneNewOUT = new GenericOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();
                    var command = new SqlCommand("EXEC spPrevisione_Nuova @Giorno, @RifPeriodo, @Descrizione, @Costo", connection);
                    command.Parameters.Add("@Giorno", SqlDbType.Int, 4);
                    command.Parameters["@Giorno"].Value = previsione.Giorno;
                    command.Parameters.Add("@RifPeriodo", SqlDbType.Int, 4);
                    command.Parameters["@RifPeriodo"].Value = previsione.RifPeriodo;
                    command.Parameters.Add("@Descrizione", SqlDbType.VarChar, 250);
                    command.Parameters["@Descrizione"].Value = previsione.Descrizione;
                    command.Parameters.Add("@Costo", SqlDbType.Money, 8);
                    command.Parameters["@Costo"].Value = previsione.Costo;
                    command.ExecuteNonQuery();
                    previsioneNewOUT.Status = "OK";
                    previsioneNewOUT.StatusError = "";

                }
                catch (Exception e)
                {
                    previsioneNewOUT.Status = "KO";
                    previsioneNewOUT.StatusError = "Previsione New Error " + e.ToString();
                }
            }
            json = JsonConvert.SerializeObject(previsioneNewOUT, Formatting.None);
            return Json(json);
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using Shared.Class;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AttivitaController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public AttivitaController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: api/Attivita/Periodo/anno/mese
        [HttpGet("[action]/{anno}/{mese}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Periodo(int anno, int mese)
        {
            string json = "";
            PeriodoOUT periodoOUT = new PeriodoOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();

                    using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                    {
                        cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                        cmd.Parameters["@Anno"].Value = anno;
                        cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                        cmd.Parameters["@Mese"].Value = mese;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                periodoOUT.ID = (int)reader["ID"];
                                periodoOUT.Descrizione = reader["Descrizione"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    periodoOUT.ID = 0;
                    periodoOUT.Descrizione = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(periodoOUT, Formatting.None);
            return Json(json);
        }

        // GET: api/Attivita/Riepilogo/anno/mese
        [HttpGet("[action]/{anno}/{mese}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Riepilogo(int anno, int mese)
        {
            string json = "";
            DatiSuntoOUT datiSuntoOUT = new DatiSuntoOUT();

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

                    using (var cmdb = new SqlCommand("SELECT " +
                                                    "   'Entrate' AS Tipo, " +
                                                    "   SUM(Att.Costo) AS Tot " +
                                                    "FROM " +
                                                    "   tblAttivita ATT " +
                                                    "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                    "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                    "WHERE " +
                                                    "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                    "   AND Att.Costo > 0 " +
                                                    "GROUP BY " +
                                                    "   ATT.RifPeriodo " +
                                                    "UNION " +
                                                    "SELECT " +
                                                    "   'Uscite' AS Tipo, " +
                                                    "   SUM(Att.Costo) AS Tot " +
                                                    "FROM " +
                                                    "   tblAttivita ATT " +
                                                    "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                    "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                    "WHERE " +
                                                    "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                    "   AND Att.Costo < 0 " +
                                                    "GROUP BY " +
                                                    "   ATT.RifPeriodo " +
                                                    "UNION " +
                                                    "SELECT " +
                                                    "   'Bilancio' AS Tipo, " +
                                                    "   SUM(Att.Costo) AS Tot " +
                                                    "FROM " +
                                                    "   tblAttivita ATT " +
                                                    "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                    "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                    "WHERE " +
                                                    "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                    "GROUP BY " +
                                                    "   ATT.RifPeriodo", connection))
                    {
                        cmdb.Parameters.Add("@Anno", SqlDbType.Int, 4);
                        cmdb.Parameters["@Anno"].Value = anno;
                        cmdb.Parameters.Add("@Mese", SqlDbType.Int, 4);
                        cmdb.Parameters["@Mese"].Value = mese;

                        SqlDataAdapter adp = new SqlDataAdapter(cmdb);
                        adp.Fill(dt);

                        datiSuntoOUT.Status = "OK";
                        datiSuntoOUT.DatiSunto = new List<DatiSunto>();

                        foreach (DataRow row in dt.Rows)
                        {
                            i++;
                            DatiSunto datiSunto = new DatiSunto
                            {
                                Tipo = row["Tipo"].ToString(),
                                Tot = Convert.ToDecimal(row["Tot"])
                            };
                            datiSuntoOUT.DatiSunto.Add(datiSunto);
                        }

                        if (i <= 0)
                        {
                                DatiSunto datiSunto1 = new DatiSunto
                                {
                                    Tipo = "Entrate",
                                    Tot = 0
                                };
                                datiSuntoOUT.DatiSunto.Add(datiSunto1);

                                DatiSunto datiSunto2 = new DatiSunto
                                {
                                    Tipo = "Uscite",
                                    Tot = 0
                                };
                                datiSuntoOUT.DatiSunto.Add(datiSunto2);

                                DatiSunto datiSunto3 = new DatiSunto
                                {
                                    Tipo = "Bilancio",
                                    Tot = 0
                                };
                                datiSuntoOUT.DatiSunto.Add(datiSunto3);
                                datiSuntoOUT.StatusError = "NO RECORD";
                        }
                    }
                }
                catch (Exception e)
                {
                    datiSuntoOUT.Status = "KO";
                    datiSuntoOUT.StatusError = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(datiSuntoOUT, Formatting.None);
            return Json(json);
        }

        // GET: api/Attivita/Attivita/anno/mese/filter/order
        [HttpGet("[action]/{anno}/{mese}/{filter}/{order}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Attivita(int anno, int mese, string? filter = "none", string? order = "none")
        {
            string json = "";
            AttivitaOUT attivitaOUT = new AttivitaOUT();

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
                                        "   ATT.ID, " +
                                        "   ATT.Giorno, " +
                                        "   ISNULL(ATT.Dettagli, '-') AS Dettagli, " +
                                        "   ATT.Costo, " +
                                        "   Per.Anno, " +
                                        "   Per.Mese, " +
                                        "   Per.Descrizione, " +
                                        "   TP.TipoAttivita, " +
                                        "   Tp.ColoreAzione, " +
                                        "   Tp.ColoreHTML, " +
                                        "   Tp.Tipologia " +
                                        "FROM " +
                                        "   tblAttivita ATT " +
                                        "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                        "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                        "WHERE " +
                                        "   Per.Anno = @Anno AND Per.Mese = @Mese ";

                    string FiltroSQL = "";
                    attivitaOUT.Status = "OK";

                    if (filter != null && filter.Trim() != "" && filter.Trim() != "none")
                    {
                        FiltroSQL = " AND TP.TipoAttivita = @TipoAttivita ";
                        attivitaOUT.StatusError = "Filtro Richiesto: " + filter.ToString();
                    }

                    if (order != null && order.Trim() != "" && order.Trim() != "none")
                    {
                        sSQL_Select = sSQL_Select + " ORDER BY ATT.Costo ";
                        attivitaOUT.StatusError = "Ordinamento Richiesto: " + order.ToString();
                    }
                    else
                    {
                        sSQL_Select = sSQL_Select + FiltroSQL + " ORDER BY ATT.Giorno ";
                    }

                    SqlCommand cmd = new SqlCommand(sSQL_Select, connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;

                    if (filter != null && filter != "")
                    {
                        cmd.Parameters.Add("@TipoAttivita", SqlDbType.VarChar, 50);
                        cmd.Parameters["@TipoAttivita"].Value = filter.ToString();
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    attivitaOUT.Attivita = new List<Attivita>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        Attivita attivita = new Attivita
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            Giorno = Convert.ToInt32(row["Giorno"]),
                            Dettagli = row["Dettagli"].ToString(),
                            Costo = Convert.ToDecimal(row["Costo"]),
                            Anno = Convert.ToInt32(row["Anno"]),
                            Mese = Convert.ToInt32(row["Mese"]),
                            Descrizione = row["Descrizione"].ToString(),
                            TipoAttivita = row["TipoAttivita"].ToString(),
                            ColoreAzione = row["ColoreAzione"].ToString(),
                            ColoreHTML = row["ColoreHTML"].ToString(),
                            Tipologia = row["Tipologia"].ToString(),                                                 
                        };
                        attivitaOUT.Attivita.Add(attivita);
                    }

                    if (i <= 0)
                    {
                        attivitaOUT.StatusError = "Nessuna attivita trovata.";
                    }                 
                }
                catch (Exception e)
                {
                    attivitaOUT.Status = "KO";
                    attivitaOUT.StatusError = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(attivitaOUT, Formatting.None);
            return Json(json);
        }

        // GET: api/Attivita/AttivitaAggregate/anno/mese
        [HttpGet("[action]/{anno}/{mese}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult AttivitaAggregate(int anno, int mese)
        {
            string json = "";
            AggregatoAttivitaOUT aggregatoAttivitaOUT = new AggregatoAttivitaOUT();

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

                    aggregatoAttivitaOUT.Status = "OK";
                    aggregatoAttivitaOUT.AggregatoAttivita = new List<AggregatoAttivita>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        AggregatoAttivita aggregatoAttivita = new AggregatoAttivita
                        {
                            TipoAttivita = row["TipoAttivita"].ToString(),
                            ColoreHTML = row["ColoreHTML"].ToString(),
                            Descrizione = row["Descrizione"].ToString(),
                            TotAtt = Convert.ToDecimal(row["TotAtt"])
                        };
                        aggregatoAttivitaOUT.AggregatoAttivita.Add(aggregatoAttivita);
                    }
                }
                catch (Exception e)
                {
                    aggregatoAttivitaOUT.Status = "KO";
                    aggregatoAttivitaOUT.StatusError = e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(aggregatoAttivitaOUT, Formatting.None);
            return Json(json);
        }

        // GET: api/Attivita/TipoAttivitaList
        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult TipoAttivitaList()
        {
            string json = "";
            TipoAttivitaOUT tipoAttivitaOUT = new TipoAttivitaOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();
                    DataTable dt = new DataTable();

                    using (var cmd = new SqlCommand("SELECT ID, Tipologia + ' - '+ TipoAttivita AS Tipo FROM tblTipoAttivita ORDER BY Tipologia", connection))
                    {
                        {
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(dt);

                            tipoAttivitaOUT.Status = "OK";
                            tipoAttivitaOUT.TipoAttivita = new List<TipoAttivita>();

                            foreach (DataRow row in dt.Rows)
                            {
                                TipoAttivita tipoAttivita = new TipoAttivita
                                {
                                    ID = Convert.ToInt32(row["ID"]),
                                    Tipo = row["Tipo"].ToString(),
                                };
                                tipoAttivitaOUT.TipoAttivita.Add(tipoAttivita);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    tipoAttivitaOUT.Status = "KO";
                    tipoAttivitaOUT.StatusError = "Error " + e.ToString();

                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(tipoAttivitaOUT, Formatting.None);
            return Json(json);
        }

        // DELETE: api/Attivita/Delete/id
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
                    var command = new SqlCommand("DELETE FROM tblAttivita WHERE ID = @iRifAtt", connection);
                    command.Parameters.Add("iRifAtt", SqlDbType.Int, 4);
                    command.Parameters["iRifAtt"].Value = (Int32)id;
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
                        deleteOUT.StatusError = "Errore Attivita da cancellare non trovata!";
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

        // POST: api/Attivita/AttivitaNew/attivita
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult AttivitaNew([FromBody]  AttivitaIN attivita)
        {
            string json = "";
            GenericOUT attivitaNewOUT = new GenericOUT();

            var connString = _configuration.GetConnectionString("Default");

            string connStringEscape = connString.ToString().Replace("\\\\", "\\");

            // dichiaro il chiamante con Certificato Valido
            connStringEscape = connStringEscape + ";TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connStringEscape))
            {
                try
                {
                    connection.Open();  
                    var command = new SqlCommand("EXEC spAttivita_Nuova @Giorno, @RifPeriodo, @RiftipoAttivita, @Dettagli, @Costo", connection);
                    command.Parameters.Add("@Giorno", SqlDbType.Int, 4);
                    command.Parameters["@Giorno"].Value = attivita.Giorno;
                    command.Parameters.Add("@RifPeriodo", SqlDbType.Int, 4);
                    command.Parameters["@RifPeriodo"].Value = attivita.RifPeriodo;
                    command.Parameters.Add("@RiftipoAttivita", SqlDbType.Int, 4);
                    command.Parameters["@RiftipoAttivita"].Value = attivita.RifTipoAttivita;
                    command.Parameters.Add("@Dettagli", SqlDbType.VarChar, 250);
                    string sDettagli = "";
                    if (attivita.Dettagli == "" || attivita.Dettagli is null)
                    {
                        sDettagli = "-";
                    }
                    else
                    {
                        sDettagli = attivita.Dettagli;
                    }
                    command.Parameters["@Dettagli"].Value = sDettagli;
                    command.Parameters.Add("@Costo", SqlDbType.Money, 8);
                    command.Parameters["@Costo"].Value = attivita.Costo;
                    command.ExecuteNonQuery();
                    attivitaNewOUT.Status = "OK";
                    attivitaNewOUT.StatusError = "";

                }
                catch (Exception e)
                {
                    attivitaNewOUT.Status = "KO";
                    attivitaNewOUT.StatusError = "Attivita New Error " + e.ToString();
                }
            }
            json = JsonConvert.SerializeObject(attivitaNewOUT, Formatting.None);
            return Json(json);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http;
using Shared.Class;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TrendController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public TrendController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        // GET: TrendController/Trend/id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public JsonResult Trend(int id = 10000000)
        {
            string json = "";
            TrendOUT TrendOUT = new TrendOUT();

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
                    SqlCommand cmd = new SqlCommand("EXEC spTrend @Top", connection);
                    cmd.Parameters.Add("@Top", SqlDbType.Int, 4);
                    cmd.Parameters["@Top"].Value = id;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    TrendOUT.Status = "OK";
                    TrendOUT.StatusError = "";
                    TrendOUT.Trend = new List<Trend>();

                    foreach (DataRow row in dt.Rows)
                    {
                        i++;
                        Trend trend = new Trend
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            Descrizione = row["Descrizione"].ToString(),
                            Anno = Convert.ToInt32(row["Anno"]),
                            Mese = Convert.ToInt32(row["Mese"]),
                            Bilancio = Convert.ToDecimal(row["Bilancio"])
                        };
                        TrendOUT.Trend.Add(trend);
                    }
                }
                catch (Exception e)
                {
                    TrendOUT.Status = "KO";
                    TrendOUT.StatusError = e.ToString();
                }
                connection.Close();
            }

            json = JsonConvert.SerializeObject(TrendOUT, Formatting.None);
            return Json(json);
        }
    }
}

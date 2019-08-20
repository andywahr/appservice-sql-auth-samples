using Dotnetcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Dotnetcore.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Models.Configuration configuration;
        private ILogger<ValuesController> _logger;

        public ValuesController(Models.Configuration config, ILogger<ValuesController> logger)
        {
            configuration = config;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ValueModel>> Get()
        {
            _logger.LogInformation("I am here");
            List<ValueModel> retVal = new List<ValueModel>();

            string connStr = configuration.ConnectionStrings.AuthTest;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.AccessToken = await (new AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/");
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT Id, [Value], USER_NAME() FROM [VALUES]", connection))
                using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
                {
                    while (await dataReader.ReadAsync())
                    {
                        ValueModel val = new ValueModel();
                        val.Id = dataReader.GetInt32(0);
                        val.Value = dataReader.GetString(1);
                        val.CurrentUser = dataReader.GetString(2);

                        retVal.Add(val);
                    }
                }
            }

            return retVal;
        }
    }
}

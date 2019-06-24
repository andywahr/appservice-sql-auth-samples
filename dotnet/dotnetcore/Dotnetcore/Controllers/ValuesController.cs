using Dotnetcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Services.AppAuthentication;
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

        public ValuesController(Models.Configuration config)
        {
            configuration = config;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<ValueModel>> Get()
        {
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

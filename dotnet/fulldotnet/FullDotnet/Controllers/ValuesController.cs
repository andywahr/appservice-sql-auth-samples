using FullDotnet.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FullDotnet.Controllers
{
    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route()]
        // GET api/values
        public  async Task<IEnumerable<ValueModel>> Get()
        {
            List<ValueModel> retVal = new List<ValueModel>();

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["AuthTest"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connStr))
            {
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

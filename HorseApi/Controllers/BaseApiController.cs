using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;

namespace HorsiApi.Controllers
{
    public class BaseApiController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _dbConnection;

        protected readonly QueryFactory _db;

        public BaseApiController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._dbConnection = new SqlConnection(getConnectionString());
            this._db = new QueryFactory(_dbConnection, new SqlServerCompiler());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbConnection.Dispose();
            }
            base.Dispose(disposing);
        }

        private string getConnectionString()
        {
            if (bool.Parse(_configuration["isProduction"]))
            {
                return _configuration.GetConnectionString("ProductionConnection");
            }
            return _configuration.GetConnectionString("LocalConnection");
        }
    }
}

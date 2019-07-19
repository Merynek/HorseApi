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
            _configuration = configuration;
            this._dbConnection = new SqlConnection(_configuration.GetConnectionString("LocalConnection"));
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
    }
}

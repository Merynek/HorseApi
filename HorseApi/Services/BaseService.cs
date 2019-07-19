using HorseApi.Models;
using SqlKata.Execution;

namespace HorseApi.Services
{
    public class BaseService
    {
        protected readonly QueryFactory _db;
        protected ResponseModel response;

        public BaseService(QueryFactory db)
        {
            this._db = db;
            this.response = new ResponseModel();
        }
    }
}

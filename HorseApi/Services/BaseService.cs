using HorseApi.Models;
using SqlKata.Execution;

namespace HorseApi.Services
{
    public class BaseService
    {
        protected readonly QueryFactory _db;
        protected ResponseModel _response;

        public BaseService(QueryFactory db)
        {
            this._db = db;
            this._response = new ResponseModel();
        }
    }
}

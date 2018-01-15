using Radikool6.Entities;

namespace Radikool6.Models
{
    public class BaseModel
    {
        protected Db Db { get; private set; }

        public BaseModel(Db db)
        {
            this.Db = db;
        }
    }
}
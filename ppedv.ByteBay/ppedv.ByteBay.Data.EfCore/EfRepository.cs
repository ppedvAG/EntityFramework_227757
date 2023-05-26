using ppedv.ByteBay.Model;
using ppedv.ByteBay.Model.Contracts;

namespace ppedv.ByteBay.Data.EfCore
{
    public class EfRepository : IRepository
    {
        private readonly string conString;
        ByteBayContext con;
        public EfRepository(string conString)
        {
            this.conString = conString;
            con = new ByteBayContext(conString);
        }

        public void Add<T>(T entity) where T : Entity
        {
            con.Add(entity);
        }

        public void Delete<T>(T entity) where T : Entity
        {
            con.Remove(entity);
        }

        public IEnumerable<T> GetAll<T>() where T : Entity
        {
            return con.Set<T>().ToList();
        }

        public T GetById<T>(int id) where T : Entity
        {
            return con.Set<T>().Find(id);
        }

        public void SaveAll()
        {
            con.SaveChanges();
        }

        public void Update<T>(T entity) where T : Entity
        {
            con.Update(entity);
        }
    }
}

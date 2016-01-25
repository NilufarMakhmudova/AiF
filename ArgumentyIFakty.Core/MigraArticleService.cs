using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentyIFakty.DataAccess;

namespace ArgumentyIFakty.Core
{
    public class MigraArticleService : IArticlesWithMigraService
    {
        //singleton
        private AiFArchiveEntities db;

        public MigraArticleService()
        {
            db = new AiFArchiveEntities();
        }

        public void Delete(int articleId)
        {
            throw new NotImplementedException();
        }

        public List<ArticlesWithWordMigra> GetAll()
        {
            return db.ArticlesWithWordMigras.Where(x=>x.Author == "Лидия Юдина").ToList();
        }

        public ArticlesWithWordMigra GetById(int articleId)
        {
            throw new NotImplementedException();
        }

        public void Insert(ArticlesWithWordMigra article)
        {
            throw new NotImplementedException();
        }

        public void Update(ArticlesWithWordMigra article)
        {
            throw new NotImplementedException();
        }
    }
}

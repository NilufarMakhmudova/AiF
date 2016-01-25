using ArgumentyIFakty.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
    public interface IArticlesWithMigraService
    {
           void Insert(ArticlesWithWordMigra article);
            void Delete(int articleId);
            void Update(ArticlesWithWordMigra article);
            List<ArticlesWithWordMigra> GetAll();
        ArticlesWithWordMigra GetById(int articleId);

        
    }
}

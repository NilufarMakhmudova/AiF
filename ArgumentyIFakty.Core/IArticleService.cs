
using ArgumentyIFakty.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
   public interface IArticleService
    {
       void Insert(Article article);
       void Delete(int articleId);
       void Update(Article article);
       List<Article> GetAll();
       Article GetById(int articleId);

    }
}

using ArgumentyIFakty.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentyIFakty.Core
{
    public class ArticleEFService : IArticleService

    {    //singleton
        private AiFArchiveEntities db;

        public ArticleEFService() {
            db = new AiFArchiveEntities();
        }
               
        public void Insert(Article article)
        {
            if (article.Url == null)
            { throw new NotImplementedException("You cannot insert empty article"); }

            db.Articles.Add(article);
            db.SaveChanges();
            
        }

        public void Delete(int articleId)
        {
            var item = db.Articles.FirstOrDefault(x => x.Id == articleId);
            if (item == null) throw new ArgumentNullException("Not possible to delete empty article");
            db.Articles.Remove(item);
            db.SaveChanges();
        }

        public void Update(Article article)
        {
            var dbItem = db.Articles.FirstOrDefault(x => x.Id == article.Id);
            if (dbItem == null) throw new ArgumentNullException("Not possible to delete empty article");
            dbItem.DatePublished = article.DatePublished;
            dbItem.EditionName = article.EditionName;
            dbItem.Title = article.Title;
            dbItem.Context = article.Context;
            dbItem.Author = article.Author;
            dbItem.Url = article.Url;
            db.SaveChanges();
        }

        public List<Article> GetAll()
        {
            return db.Articles.ToList();
        }

        public Article GetById(int articleId)
        {
            var article = db.Articles.FirstOrDefault(x => x.Id == articleId);
            if (article == null) throw new ArgumentNullException("Such article does not exist");
            return article;
        }
    }
}

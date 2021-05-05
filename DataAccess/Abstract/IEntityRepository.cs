using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    // class: referans tip olsun gönderilen şey. int tarzı değer tipler gönderilmesin
    // IEntity(X) : gönderilen şey X ya da X ' i implemente eden bir şey olsun.
    // new() : newlenebilir olmalı... interfaceler newlenemez o yüzden interface olan IEntity gönderme diyor...
    public interface IEntityRepository<T> where T:class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        //List<T> GetAllByCategory(int categoryId); artık ihtiyaç yok.
    }
}

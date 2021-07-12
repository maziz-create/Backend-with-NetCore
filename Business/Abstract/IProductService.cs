using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IProductService
    {
        //List<Product> GetAll();
        IDataResult<List<Product>> GetAll();
        IDataResult<List<Product>> GetAllByCategoryId(int categoryID);
        IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);
        IDataResult<List<ProductDetailDto>> GetProductDetails();
        IDataResult<Product> GetById(int productId);

        IResult Add(Product product);   // dönebilecek bir data olmadığından ötürü IDataResult ' a göndermedik. yani aslında gereksiz görüyoruz. eklediğimiz adamın datasını neden geri döndürelim ki. eklendiğine dair mesaj döndürsem yeterli...
        IResult Update(Product product);
        IResult AddTransactionalTest(Product product); //transaction yönetimi için.
    }
}

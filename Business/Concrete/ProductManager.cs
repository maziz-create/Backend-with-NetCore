using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        //Claim
        [SecuredOperation("product.add,admin")] //ürün ekleme ve admin claimleri(yetkileri). Bu yetkiyi veritabanından tanımlayıp sonra userOperationClaims tablosunda tanımlarsan eğer user ve claim id ile birlikte, o zaman hallolur. 
        //product.add gibi claimler => method bazında yetkilendirme. 
        [CacheRemoveAspect("IProductService.Get")] //yeni bir ürün eklendiği zaman var olan get cachelerini sileriz ki bu ürün gözüksün artık.
        //NOT!: IProductService.Get ne oluyor ? hani biz cache yaparken isimlendirme yaptık ya, oradaki isim işte. 
        //NOT!: Remove cache'i OnSuccess bir aspecttir. yani alttaki Add ancak başarılı olursa çalışır Remove aspecti. Aksi taktirde eğer yeni bir ürün eklemede hata oluşursa ben neden durduk yere get cachelerini sileyim dimi?
        public IResult Add(Product product)
        {
            //      VALIDATIONUN YAPILACAĞI DİĞER Bİ KATMANDA BAŞKA YAPILACAKLAR
            //LOGLAMA
            //CACHE // CACHEREMOVE
            //PERFORMANS YÖNETİMİ
            //TRANSACTİON
            //YETKİLENDİRME

            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName), CheckIfProductCountOfCategoryCorrect(product.CategoryID), CheckIfCategoryLimitExceeded());

            if (result!=null)   //geçilemeyen bir kural varsa eğer o result'u döndür.
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheAspect]
        [PerformanceAspect(5)]//Bu method 5 saniyeden fazla çalışırsa beni ayıktır.
        public IDataResult<List<Product>> GetAll()
        {
            Thread.Sleep(5000); // NOT: BURASI SAYESİNDE PERFORMANCE ASPECT ÇALIŞACAK VE SİSTEM HATA VERECEK. ONA GÖRE HATA ALDIĞINDA BURAYA GELİRSEN COMMENTLE BU KODU
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryID)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryID == categoryID));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p=>p.ProductId==productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p=> p.UnitPrice>=min && p.UnitPrice<=max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [ValidationAspect(typeof(ProductValidator))]
        //cacheRemoveAspect = cacheleri siler.
        //ürün güncellendi. eski data gelmesin yeni datayı getirsinler diye burası için yani product manager için çalışan tüm getlerin silinmesi lazım. yoksa o ürünü bir yerde kullanacakları zaman cachede varsa gidip eski ürün bilgilerini getirirler.
        [CacheRemoveAspect("IProductService.Get")] //yeni bir ürün eklendiği zaman var olan get cachelerini sileriz ki bu ürün gözüksün artık.
        //NOT!: IProductService.Get ne oluyor ? hani biz cache yaparken isimlendirme yaptık ya, oradaki isim işte. 
        //NOT!: Remove cache'i OnSuccess bir aspecttir. yani alttaki Add ancak başarılı olursa çalışır Remove aspecti. Aksi taktirde eğer yeni bir ürün eklemede hata oluşursa ben neden durduk yere get cachelerini sileyim dimi?
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryID == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();

            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceeded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceeded);
            }
            return new SuccessResult();
        }

        [TransactionScopeAspect]
        //kodun çalışma mantığını tam anlayamadığım yer...
        //Ne işe yarıyor?: göndrdiğimiz 5 adet sorguyu tek pakette gönderiyoruz ve eğer 1 tanesinde hata varsa diğer 4 başarılı sorguyu da 5. sorguyu da iptal ediyor. 4ünü database'e yazıp 1 ini yazmamazlık yapmıyor.
        public IResult AddTransactionalTest(Product product)
        {
            _productDal.Update(product); //?
            _productDal.Add(product); //?
            return new SuccessResult(Messages.Updated); //?
        }
    }
}

using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        //[Authorize()] 
        //Getall methodunu çalıştırabilmen için sisteme giriş yapman ve giriş yaptığın token ile bu işlemi gerçekleştirmen gerekli.
        //parantez içi örnekleri: [Authorize("product.Add")] = product.Add yetkisi olanlar yalnızca GetAll ' ı çalıştırabilir.
        //[Authorize("admin")]      yalnızca admin yetkisi olanlar GetAll ' ı çalıştırabilir. 
        //yetkinin şu an için sadece ismini veriyoruz... eklemesini henüz bilmiyorum. ya da biliyorumdur belki de. evettt öğrendim.
        //product.List yetkisi verdiğimizi varsayarsak eğer bunu sadece veritabanından halledebiliriz. önce product.List diye bi yetki üret sonra kullanıcı Id ile yetki Id ' ı userOperationClaims'e koy. orada bizzat bu bilgilere göre yetki dağıtımı yapıalbilir.
        public IActionResult GetAll()
        {
            Thread.Sleep(1000);
            var result = _productService.GetAll();
            if (result.Success)
            { 
                return Ok(result); //ok 200
            }
            return BadRequest(result); //bad 400
        }

        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryID)
        {
            var result = _productService.GetAllByCategoryId(categoryID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result); 
                //buraya result.data , result.message falan da yazabiliriz ama standart gitmek daha iyi mesela mevzu okeyse datayı gönder, mevzu çıktıysa mesaj gönder yapabiliriz ama bu sefer her yerde bu yöntemle yapmalıyız ki API mizi kullanan kişinin kafası karışmasın.
            }
            return BadRequest(result);
        }

         [HttpPost("add")]  //dışardan biri aşağıdaki metodu nasıl çalıştıracak?
         public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

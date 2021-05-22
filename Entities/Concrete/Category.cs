using Core.Entities;
//using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Category : IEntity
    {
            //çıplak class kalmasın! proje büyüdükçe sorun yaşarsın.
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

    }
}

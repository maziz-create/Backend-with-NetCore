using Business.Abstract;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal; //zayıf bağlılık var burada. ne demek bu?= interface'e bağlı kalmak demek.
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user); //kişinin yetkilerini çek.
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email); //mail adresine göre kullanıcıyı getir.
        }
    }
}

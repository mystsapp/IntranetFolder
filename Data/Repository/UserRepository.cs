using Data.Interfaces;
using Data.Models;
using Data.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> Login(string username, string password);
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(qltaikhoanContext context) : base(context)
        {
        }


        public async Task<int> Login(string username, string password)
        {
            
            //MaHoaSHA1 sha1 = new MaHoaSHA1();
            var result = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Trangthai == false)
                {
                    return -1;
                }
                else
                {//sha1.EncodeSHA1
                    if (result.Password == MaHoaSHA1.EncodeSHA1(password))
                    {
                        //'Data is Null. This method or property cannot be called on Null values.'

                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

    }
}

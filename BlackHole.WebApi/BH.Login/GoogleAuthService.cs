using BlackHole.DAL;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.Login
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoogleAuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Authenticate(GoogleJsonWebSignature.Payload payload)
        {
            return await FindUserOrAdd(payload);
        }

        private async Task<User> FindUserOrAdd(GoogleJsonWebSignature.Payload payload)
        {
            var u = (await _unitOfWork.Users.GetByFilter(u => u.Mail == payload.Email)).FirstOrDefault();
            if (u is null)
            {
                u = new User()
                {
                    Mail = payload.Email,
                    Role = RoleConstants.User,
                };

                await _unitOfWork.Users.CreateAsync(u);
                await _unitOfWork.CompleteAsync();
            }

            return u;
        }
    }
}

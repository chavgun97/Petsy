﻿using Petsy.Models;
using System.Threading.Tasks;

namespace Petsy.Services.Interfaces
{
    public interface IFireBaseAuth
    {
        Task<ResultAuth> LoginWithEP(string Email, string Psw);
        Task<ResultAuth> RegisteredWithEP(string Name, string Email, string Psw);
        Task<ResultAuth> LoginWithFacebook();
        Task<ResultAuth> LoginWithGoogle();





    }
}

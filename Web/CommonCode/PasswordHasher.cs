using LMYFrameWorkMVC.Common.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return Hasher.HashString(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == Hasher.HashString(providedPassword))
                return PasswordVerificationResult.Success;
            else
              return  PasswordVerificationResult.Failed;
        }
    }
}
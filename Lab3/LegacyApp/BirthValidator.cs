using System;

namespace LegacyApp
{
    public class BirthValidator : IUserValidator
    {
        public bool Validate(User user)
        {
            var now = DateTime.Now;
            int age = now.Year - user.DateOfBirth.Year;
            if (now.Month < user.DateOfBirth.Month || (now.Month == user.DateOfBirth.Month && now.Day < user.DateOfBirth.Day)) age--;

            return age >= 21;
        }
    }
}
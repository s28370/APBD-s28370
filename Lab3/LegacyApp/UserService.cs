using System;
using System.Collections.Generic;
using System.Linq;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserCreditCheck _userCreditCheck = new UserCreditCheck();
        private readonly List<IUserValidator> _userValidators = new() {new NameValidator(), new EmailValidator(), new BirthValidator()};
            public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (_userValidators.Any(userValidator => !userValidator.Validate(user)))
            {
                return false;
            }

            if (!_userCreditCheck.CheckCredit(client, user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
    }
}

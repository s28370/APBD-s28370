namespace LegacyApp
{
    public class NameValidator : IUserValidator
    {
        public bool Validate(User user)
        {
            return !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
        }
    }
}
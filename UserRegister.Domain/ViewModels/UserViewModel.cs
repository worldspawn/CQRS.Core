using System;
using CQRS.Core.ViewModel;

namespace UserRegister.Domain.ViewModels
{
    [Serializable]
    public class UserViewModel : IPersistedDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        #region IPersistedDto Members

        public Guid Id { get; set; }

        #endregion
    }
}
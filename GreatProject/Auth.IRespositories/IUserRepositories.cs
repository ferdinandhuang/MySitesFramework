using Framework.Core.Extensions;
using Framework.Core.Repositories;
using MySites.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.IRespositories
{
    public interface IUserRepositories : IRepository<User>
    {
        void AddTest();

        Result<User> Search(string username, string passwords);
    }
}

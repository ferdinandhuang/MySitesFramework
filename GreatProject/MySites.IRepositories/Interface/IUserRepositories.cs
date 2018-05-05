using Framework.Core.Repositories;
using MySites.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySites.IRepositories.Interface
{
    public interface IUserRepositories : IRepository<User>
    {
        void AddTest();
    }
}

using MySites.IServices;
using System;
using MySites.IRepositories.Interface;

namespace MySites.Services
{
    public class Class1 : IClass1
    {
        private IUserRepositories userRepositories;
        public Class1(IUserRepositories _userRepositories)
        {
            userRepositories = _userRepositories;
        }
        public void Test()
        {
            userRepositories.AddTest();
        }
    }
}

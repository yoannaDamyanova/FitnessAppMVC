using FitnessApp.Web.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data.Contracts
{
    public interface IUserService
    {
        public Task<IEnumerable<UserServiceModel>> AllAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data.Contracts
{
    internal interface IBaseService
    {
        bool IsGuidValid(string? id, ref Guid parsedGuid);
    }
}

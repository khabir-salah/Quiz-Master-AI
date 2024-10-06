using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Interfaces.IService
{
    public interface ISeedRole
    {
        Task AssignStandardRole(ApplicationUser user);
        Task AssignClassicRole(ApplicationUser user);
        Task AssignBasicRole(ApplicationUser user);
    }
}

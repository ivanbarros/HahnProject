using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.Jobs.Interfaces;

public interface IFoodRecipeUpsertJob
{
    Task RunUpsertAsync();
}

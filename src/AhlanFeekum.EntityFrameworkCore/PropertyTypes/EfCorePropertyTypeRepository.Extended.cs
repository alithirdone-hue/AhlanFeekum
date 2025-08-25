using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using AhlanFeekum.EntityFrameworkCore;

namespace AhlanFeekum.PropertyTypes
{
    public class EfCorePropertyTypeRepository : EfCorePropertyTypeRepositoryBase, IPropertyTypeRepository
    {
        public EfCorePropertyTypeRepository(IDbContextProvider<AhlanFeekumDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
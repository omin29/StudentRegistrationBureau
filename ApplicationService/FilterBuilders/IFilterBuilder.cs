using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.FilterBuilders
{
    public interface IFilterBuilder<TEntity>
    {
        Expression<Func<TEntity, bool>> BuildFilter();
    }
}

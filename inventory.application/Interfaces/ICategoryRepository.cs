using inventory.core.Entities;
using MyApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category> { }

}

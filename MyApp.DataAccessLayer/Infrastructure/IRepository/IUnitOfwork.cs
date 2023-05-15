using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.IRepository
{
    public interface IUnitOfwork
    {
        ICategoryRepository Category{ get; }
        IProductRepository Product { get; } 
        ICartRepository Cart { get; } 
        IApplicationUser ApplicationUser { get; } 
        IOrderDetailRepository OrderDetail { get; } 
        IOrderHeaderRepository OrderHeader { get; } 
        void Save();
    }
}

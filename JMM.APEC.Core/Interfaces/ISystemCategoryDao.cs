using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Core.Interfaces
{
   public interface ISystemCategoryDao
    {
        List<System_Category> GetCategories(int? GatewayId, int? ObjectId, bool? Active);
        void InsertCategory(System_Category category);
        void UpdateCategory(System_Category category);
        void DeleteCategory(System_Category category);
    }
}

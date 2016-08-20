using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class StatusDao : IStatusDao
    {
        static StatusDao()
        {
            Mapper.CreateMap<tblIdentity_Statuses, Status>();
            Mapper.CreateMap<Status, tblIdentity_Statuses>();
        }

        public List<Status> FindStatuses()
        {
            using (var context = new ApecIdentityEntities())
            {
                var statuses = from s in context.tblIdentity_Statuses select s;

                return statuses.AsQueryable().Select(u =>
                    new Status
                    {
                        Id = u.ID,
                        Code = u.Code,
                        Description = u.Description,
                        Value = u.Value
                    })
                    .ToList();
            }
        }

        public Status FindStatusByCode(string Code)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.tblIdentity_Statuses.SingleOrDefault(m => m.Code == Code);
                return Mapper.Map<tblIdentity_Statuses, Status>(entity);
            }
        }

        public Status FindStatusById(int StatusId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.tblIdentity_Statuses.SingleOrDefault(m => m.ID == StatusId);
                return Mapper.Map<tblIdentity_Statuses, Status>(entity);
            }
        }

        public Status GetStatusByUserId(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = (from u in context.AspNetUsers where u.Id == userId select u.tblIdentity_Statuses).FirstOrDefault();
                return Mapper.Map<tblIdentity_Statuses, Status>(entity);
            }
        }


    }
}

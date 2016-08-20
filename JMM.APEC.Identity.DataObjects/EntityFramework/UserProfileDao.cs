using AutoMapper;
using JMM.APEC.Identity.BusinessObjects;
using JMM.APEC.Identity.DataObjects.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMM.APEC.Identity.DataObjects
{
    public class UserProfileDao : IUserProfileDao
    {
        static UserProfileDao()
        {
            Mapper.CreateMap<tblIdentity_UserProfiles, UserProfile>();
            Mapper.CreateMap<UserProfile, tblIdentity_UserProfiles>();
        }

        public int Insert(UserProfile profile)
        {

            using (var context = new ApecIdentityEntities())
            {
                var entity = Mapper.Map<UserProfile, tblIdentity_UserProfiles>(profile);

                context.tblIdentity_UserProfiles.Add(entity);
                context.SaveChanges();

                // update business object with new id
                profile.ID = entity.ID;
            }

            return profile.ID;
        }

        public UserProfile GetByUserId(int UserId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var userProfile = context.tblIdentity_UserProfiles.FirstOrDefault(c => c.UserId == UserId) as tblIdentity_UserProfiles;

                var rUserProfile = Mapper.Map<tblIdentity_UserProfiles, UserProfile>(userProfile);

                return rUserProfile;
            }
        }
    }
}

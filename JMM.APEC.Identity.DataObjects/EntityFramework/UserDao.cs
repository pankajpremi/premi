using JMM.APEC.Identity.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Data.SqlTypes;
using JMM.APEC.Identity.DataObjects.Helpers;
using JMM.APEC.Core;
using JMM.APEC.DAL;
using JMM.APEC.WebAPI.Logging;
using System.Reflection;

namespace JMM.APEC.Identity.DataObjects.EntityFramework
{
    public class UserDao : IUserDao
    {
        static UserDao()
        {
            Mapper.CreateMap<AspNetUser, User>();
            Mapper.CreateMap<User, AspNetUser>();
        }

        public string GetUserName(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var user = (from u in context.AspNetUsers where u.Id == userId where u.Deleted == false select u).FirstOrDefault() as AspNetUser;
                //var user = context.AspNetUsers.FirstOrDefault(c => c.Id == userId) as AspNetUser;
                return user.UserName;
            }
        }

        public int GetUserId(string userName)
        {
            using (var context = new ApecIdentityEntities())
            {
                //var user = context.AspNetUsers.FirstOrDefault(c => c.UserName == userName) as AspNetUser;
                var user = (from u in context.AspNetUsers where u.UserName == userName where u.Deleted == false select u).FirstOrDefault() as AspNetUser;
                return user.Id;
            }
        }

        public User GetUserById(int userId, int portalId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var user = (from u in context.AspNetUsers where u.Id == userId where u.Deleted == false select u).FirstOrDefault() as AspNetUser;
                //var user = context.AspNetUsers.FirstOrDefault(c => c.Id == userId) as AspNetUser;

                if (user == null)
                {
                    return null;
                }

                var rUser = Mapper.Map<AspNetUser, User>(user);

                //get user profile
                UserProfileDao uprDao = new UserProfileDao();
                var userProfile = uprDao.GetByUserId(userId);
                rUser.Profile = userProfile;

                //get user portals
                UserPortalDao upDao = new UserPortalDao();

                List<UserPortal> userportals = upDao.FindByUserId(userId);
                rUser.UserPortals = userportals;

                //get user gateways
                UserGatewayDao ugDao = new UserGatewayDao();

                List<UserGateway> usergateways = ugDao.FindByUserId(userId, portalId);
                rUser.UserGateways = usergateways;

                //get user status
                StatusDao statusDao = new StatusDao();
                var status = statusDao.FindStatusById(user.StatusId);

                rUser.StatusName = status.Value;
                rUser.StatusCode = status.Code;
                rUser.StatusUpdateDateTime = (DateTime)user.StatusUpdateDateTime;

                return rUser;
            }
        }

        public List<User> GetUserByName(string userName)
        {
            using (var context = new ApecIdentityEntities())
            {
                var users = from u in context.AspNetUsers where u.UserName == userName where u.Deleted == false select u;

                return users.AsQueryable().Select(u =>
                    new User
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Address1 = u.Address1,
                        Address2 = u.Address2,
                        City = u.City,
                        State = u.State,
                        PostalCode = u.PostalCode,
                        Country = u.Country,
                        AccessFailedCount = u.AccessFailedCount,
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Approved = u.Approved,
                        LockoutEnabled = u.LockoutEnabled,
                        LockoutEndDateUtc = u.LockoutEndDateUtc ?? DateTime.MinValue,
                        PasswordHash = u.PasswordHash,
                        PhoneNumber = u.PhoneNumber,
                        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                        SecurityStamp = u.SecurityStamp,
                        StatusId = u.StatusId,
                        StatusCode = u.tblIdentity_Statuses.Code,
                        StatusName = u.tblIdentity_Statuses.Value,
                        StatusUpdateDateTime = u.StatusUpdateDateTime ?? DateTime.MinValue,
                    })
                    .ToList();
            }
        }

        public List<User> GetUserByEmail(string email)
        {
            using (var context = new ApecIdentityEntities())
            {
                var users = from u in context.AspNetUsers where u.Email == email where u.Deleted == false select u;
                return users.AsQueryable().Select(u =>
                    new User
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        Address1 = u.Address1,
                        Address2 = u.Address2,
                        City = u.City,
                        State = u.State,
                        PostalCode = u.PostalCode,
                        Country = u.Country,
                        AccessFailedCount = u.AccessFailedCount,
                        Email = u.Email,
                        EmailConfirmed = u.EmailConfirmed,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Approved = u.Approved,
                        LockoutEnabled = u.LockoutEnabled,
                        LockoutEndDateUtc = u.LockoutEndDateUtc ?? DateTime.MinValue,
                        PasswordHash = u.PasswordHash,
                        PhoneNumber = u.PhoneNumber,
                        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                        SecurityStamp = u.SecurityStamp,
                        StatusId = u.StatusId,
                        StatusCode = u.tblIdentity_Statuses.Code,
                        StatusName = u.tblIdentity_Statuses.Value,
                        StatusUpdateDateTime = u.StatusUpdateDateTime ?? DateTime.MinValue,
                    })
                    .ToList();
            }
        }

        public List<User> GetUsers(ApplicationSystemUser user, string Gateways, string Statuses, string SearchText, int? PageNum, int? PageSize, string SortField, string SortDirection)
        {
            var intGatewayList = DbHelpers.MakeParamIntList(Gateways);
            var intStatusList = DbHelpers.MakeParamIntList(Statuses);
            var intUserGatewayList = (from g in user.Gateways select g.PortalGatewayId).ToList();

            List<int> intFinalGatewayList = null;

            if (intGatewayList == null)
            {
                intFinalGatewayList = intUserGatewayList;
            }
            else
            {
                intFinalGatewayList = (from a in intUserGatewayList join b in intGatewayList on a equals b select a).ToList();
            }

            using (var context = new ApecIdentityEntities())
            {

                //get users from database
                var users = from u in context.AspNetUsers join p in context.tblIdentity_UserPortals
                            on u.Id equals p.UserId where p.PortalId == user.Portal.PortalId
                            join up in context.tblIdentity_UserProfiles on u.Id equals up.UserId
                            where u.Deleted == false
                            select new User()
                            {
                                Id = u.Id,
                                UserName = u.UserName,
                                GatewayCount = (from g in context.tblIdentity_UserGateways where u.Id == g.UserId select g).Count(),
                                Address1 = u.Address1,
                                Address2 = u.Address2,
                                City = u.City,
                                State = u.State,
                                PostalCode = u.PostalCode,
                                Country = u.Country,
                                AccessFailedCount = u.AccessFailedCount,
                                Email = u.Email,
                                EmailConfirmed = u.EmailConfirmed,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Approved = u.Approved,
                                LockoutEnabled = u.LockoutEnabled,
                                LockoutEndDateUtc = u.LockoutEndDateUtc ?? DateTime.MinValue,
                                PasswordHash = u.PasswordHash,
                                PhoneNumber = u.PhoneNumber,
                                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                                SecurityStamp = u.SecurityStamp,
                                StatusId = u.StatusId,
                                StatusCode = u.tblIdentity_Statuses.Code,
                                StatusName = u.tblIdentity_Statuses.Value,
                                StatusUpdateDateTime = u.StatusUpdateDateTime ?? DateTime.MinValue,
                                Profile = new UserProfile
                                {
                                    ID = u.tblIdentity_UserProfiles.FirstOrDefault().ID,
                                    CompanyName = u.tblIdentity_UserProfiles.FirstOrDefault().CompanyName,
                                    TimeZoneCode = u.tblIdentity_UserProfiles.FirstOrDefault().TimeZoneCode
                                }
                            };

                //get users for specific gateways
                if (!user.IsAdmin && !user.IsSuperAdmin)
                {
                    if (intFinalGatewayList != null)
                    {
                        users = from u in users
                                join ug in context.tblIdentity_UserGateways on u.Id equals ug.UserId
                                join gg in context.tblIdentity_Gateways on ug.GatewayId equals gg.ID
                                join uug in intFinalGatewayList on gg.PortalGatewayID equals uug
                                select u;

                        var grouped = users.GroupBy(item => item.Id);

                        users = grouped.Select(grp => grp.OrderBy(item => item.Id).FirstOrDefault());
                    }
                }
                else
                {
                    if (intGatewayList != null)
                    {
                        users = from u in users
                                join ug in context.tblIdentity_UserGateways on u.Id equals ug.UserId
                                join gg in context.tblIdentity_Gateways on ug.GatewayId equals gg.ID
                                join uug in intFinalGatewayList on gg.PortalGatewayID equals uug
                                select u;

                        var grouped = users.GroupBy(item => item.Id);

                        users = grouped.Select(grp => grp.OrderBy(item => item.Id).FirstOrDefault());
                    }
                }

                //get users for specific statuses
                if(intStatusList != null)
                {
                    users = from u in users
                            join s in intStatusList on u.StatusId equals s
                            select u;
                }

                //get users for specific keywords
                if(SearchText != null)
                {
                    users = from u in users
                            where (u.Email.Contains(SearchText) ||
                                   u.FirstName.Contains(SearchText) ||
                                   u.LastName.Contains(SearchText) ||
                                   u.Profile.CompanyName.Contains(SearchText) ||
                                   u.StatusName.Contains(SearchText))
                            select u;
                }

                //sort
                if(SortField != null)
                {
                    users = users.Sort(SortField, SortDirection) as IQueryable<User>;
                }

                if (PageNum != null && PageNum > 0 && PageSize != null && PageSize > 0)
                {
                    users = users.Page((int)PageNum, (int)PageSize);
                }

                return users.ToList();
            }

        }

        public string GetPasswordHash(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var user = context.AspNetUsers.FirstOrDefault(c => c.Id == userId) as AspNetUser;
                
                var passHash = user.PasswordHash;
                if (string.IsNullOrEmpty(passHash))
                {
                    return null;
                }

                return passHash;
            }
        }

        public int SetPasswordHash(int userId, string passwordHash)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetUsers.SingleOrDefault(m => m.Id == userId);
                entity.PasswordHash = passwordHash;

                //context.Members.Attach(entity); 
                context.SaveChanges();
            }

            return 1;
        }

        public string GetSecurityStamp(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var user = context.AspNetUsers.FirstOrDefault(c => c.Id == userId) as AspNetUser;

                var secStamp = user.SecurityStamp;
                if (string.IsNullOrEmpty(secStamp))
                {
                    return null;
                }

                return secStamp;
            }
        }

        public int Insert(User user)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = Mapper.Map<User, AspNetUser>(user);

                context.AspNetUsers.Add(entity);
                context.SaveChanges();

                // update business object with new id
                user.Id = entity.Id;
            }

            return user.Id;
        }

        public int Delete(int userId)
        {
            using (var context = new ApecIdentityEntities())
            {
                var entity = context.AspNetUsers.SingleOrDefault(m => m.Id == userId);

                entity.Deleted = true;

                context.SaveChanges();

                UpdateStatus(userId, "DELTED", DateTime.UtcNow);
            }

            return 1;
        }

        public int UpdateStatus(int userId, string statusCode, DateTime statusUpdateDateTime)
        {
            using (var context = new ApecIdentityEntities())
            {
                var newStatus = context.tblIdentity_Statuses.SingleOrDefault(m => m.Code == statusCode);
                var user = context.AspNetUsers.SingleOrDefault(u => u.Id == userId);

                if (newStatus != null)
                {
                    if (user.StatusId != newStatus.ID)
                    {
                        user.StatusId = newStatus.ID;
                        user.StatusUpdateDateTime = statusUpdateDateTime;

                        context.SaveChanges();
                    }
                }

                return user.Id;
            }

        }

        public int Update(User user)
        {
            using (var context = new ApecIdentityEntities())
            {
                try { 
                var entity = context.AspNetUsers.SingleOrDefault(m => m.Id == user.Id);

                    if (entity != null)
                    {

                        var minServerDataTime = (DateTime)SqlDateTime.MinValue;

                        entity.UserName = user.UserName;
                        entity.FirstName = user.FirstName;
                        entity.LastName = user.LastName;
                        entity.PasswordHash = user.PasswordHash;
                        entity.SecurityStamp = user.SecurityStamp;
                        entity.Id = user.Id;
                        entity.Email = user.Email;
                        entity.EmailConfirmed = user.EmailConfirmed;
                        entity.PhoneNumber = user.PhoneNumber;
                        entity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                        entity.AccessFailedCount = user.AccessFailedCount;
                        entity.LockoutEnabled = user.LockoutEnabled;
                        entity.LockoutEndDateUtc = (DateTime)((user.LockoutEndDateUtc < minServerDataTime) ? SqlDateTime.MinValue : user.LockoutEndDateUtc);

                        if (entity.LockoutEnabled && entity.LockoutEndDateUtc <= DateTime.UtcNow)
                        {
                            entity.LockoutEndDateUtc = (DateTime)SqlDateTime.MaxValue;
                        }

                        entity.TwoFactorEnabled = user.TwoFactorEnabled;
                        entity.Approved = user.Approved;
                        entity.StatusId = (int)user.StatusId;
                        entity.StatusUpdateDateTime = user.StatusUpdateDateTime;

                        //context.Members.Attach(entity); 
                        context.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    return 0;
                }

                return user.Id;

            }
        }

        public int UpdateWithProfile(User user)
        {
            using (var context = new ApecIdentityEntities())
            {
                try
                {
                    var userEntity = context.AspNetUsers.SingleOrDefault(m => m.Id == user.Id);

                    if (userEntity != null)
                    {

                        userEntity.FirstName = user.FirstName;
                        userEntity.LastName = user.LastName;
                        userEntity.Address1 = user.Address1;
                        userEntity.Address2 = user.Address2;
                        userEntity.City = user.City;
                        userEntity.State = user.State;
                        userEntity.Country = user.Country;
                        userEntity.PostalCode = user.PostalCode;
                        userEntity.PhoneNumber = user.PhoneNumber;

                        var profileEntity = context.tblIdentity_UserProfiles.SingleOrDefault(p => p.UserId == user.Id);

                        if (profileEntity != null)
                        {
                            profileEntity.TimeZoneCode = user.Profile.TimeZoneCode;
                            profileEntity.CompanyName = user.Profile.CompanyName;
                        }

                        //context.Members.Attach(entity); 
                        context.SaveChanges();

                    }

                }
                catch (Exception ex)
                {
                    return 0;
                }

                return user.Id;
            }
        }

    }
}

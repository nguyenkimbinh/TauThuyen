using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Models;
using WebMatrix.WebData;

namespace Pii.Areas.Accounts.Models
{
    public partial class UserRepository
    {
        private PiiContext DbContext;
        private HttpContextBase HttpContext;
        public UserRepository(PiiContext _dbContext, HttpContextBase _httpContextBase)
        {
            this.DbContext = _dbContext;
            this.HttpContext = _httpContextBase;
        }

        internal UserProfile GetUser(int UserId)
        {
            return DbContext.UserProfiles.Find(UserId);
        }
    }
}
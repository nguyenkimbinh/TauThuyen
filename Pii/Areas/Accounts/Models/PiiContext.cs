using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pii.Models
{
    public partial class PiiContext : DbContext
    {
        public DbSet<Pii.Areas.Accounts.Models.UserProfile> UserProfiles { get; set; }
    }
}
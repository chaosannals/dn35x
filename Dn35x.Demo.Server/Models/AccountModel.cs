using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dn35x.Base;

namespace Dn35x.Demo.Server.Models
{
    public class AccountModel
    {
        public Definition<int?> Id { get; set; }
        public Definition<string> Account { get; set; }
        public Definition<string> Password { get; set; }
        public Definition<DateTime> CreatedAt { get; set; }
        public Definition<DateTime?> UpdatedAt { get; set; }
    }
}

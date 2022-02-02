using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DataModels.DTO_s.security
{
    public class RefreshTokenModel
    {
        public Guid Id { get; set; }
        public String refreshtoken { get; set; }

        public Guid UserId { get; set; }
    }
}

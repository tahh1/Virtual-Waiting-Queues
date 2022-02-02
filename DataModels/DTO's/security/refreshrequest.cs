using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.DataModels.DTO_s.security
{
    public class refreshrequest
    {

        [Required]
        public String refreshtoken { get; set; }
    }
}

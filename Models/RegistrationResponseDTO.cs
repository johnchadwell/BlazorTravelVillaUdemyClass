﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RegistrationResponseDTO
    {
        public bool IsRegSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}

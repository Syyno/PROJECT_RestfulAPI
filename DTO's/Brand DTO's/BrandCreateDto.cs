﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyProjectApi.DTO_s.Brand_DTO_s
{
    public class BrandCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

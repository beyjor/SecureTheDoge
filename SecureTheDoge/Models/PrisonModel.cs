﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureTheDoge.Models
{
    public class PrisonModel
    {
        public string Url { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Moniker { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime SentenceStart { get; set; }
        public DateTime EligibleForParole { get; set; }
        [Required]
        [MinLength(25)]
        [MaxLength(4096)]
        public string Description { get; set; }

        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
    }
}

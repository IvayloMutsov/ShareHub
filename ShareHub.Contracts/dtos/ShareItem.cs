using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareHub.Contracts.dtos
{
    public class ShareItem : BaseEntity
    {
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public User Owner { get; set; }

        [Required]
        public Category Category { get; set; }
    }
}

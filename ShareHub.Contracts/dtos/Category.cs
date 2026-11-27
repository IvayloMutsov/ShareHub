using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareHub.Contracts.dtos
{
    public class Category : BaseEntity
    {
        [MaxLength(300)]
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareHub.Contracts.dtos
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required, DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required, DefaultValue(null)]
        public DateTime? DeletedAt { get; set; }

        public string? ImageUrl { get; set; }
    }
}

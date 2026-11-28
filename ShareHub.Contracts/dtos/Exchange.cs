using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareHub.Contracts.dtos
{
    public class Exchange
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ShareItem Item { get; set; }

        [Required]
        public User Receiver { get; set; }

        [Required]
        public DateOnly ExchangeDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        [DefaultValue(false)]
        public bool IsCancelled { get; set; }
    }
}

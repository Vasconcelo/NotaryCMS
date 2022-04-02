﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notary.Api.DataAccess.Models.DTOs
{
    public  class PageDTO
    {
        [Required(ErrorMessage = "The filed Identifier is required")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "The filed Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The filed Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The filed AppId is required")]
        public int AppId { get; set; }

        public DateTime CreatedDate
        {
            get { return DateTime.Now; }
        }

        public DateTime UpdatedDate
        {
            get { return DateTime.Now; }
        }
    }
}

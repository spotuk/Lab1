using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class AutoListViewModel
    {
        public IEnumerable<Auto> Autos { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentType { get; set; }
    }
}
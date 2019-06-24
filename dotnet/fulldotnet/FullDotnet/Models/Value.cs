using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullDotnet.Models
{
    public class ValueModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string CurrentUser { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace LostAndFoundPatras.Models
{
    public class PetModel
    {
        public string Key { get; set; }
        public string Lost { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
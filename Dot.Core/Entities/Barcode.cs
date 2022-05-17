using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class Barcode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
        public string StudentId { get; set; }
        public string BarcodeUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

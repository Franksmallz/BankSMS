using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Transaction
    {
       public double amounts { get; set; }
        
       public double balance { get; set; }
        
       public DateTime transactionDate { get; set; } 

       public string descrption { get; set; }

       public string transactionType { get; set; } 

    }
}

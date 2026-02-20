using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Validation
{
    public class MinValue :ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value is int val)
            {
                if (val > -1) return true;
            }
            return false;
        }
    }
}

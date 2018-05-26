using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class SAccount
    {
        public string CustomerName { get; set; }
        public string NRIC { get; set; }
        public SAccount(string customer_name, string icorssm)
        {
            CustomerName = customer_name;
            NRIC = icorssm;
        }

        public decimal getPaid()
        {
            var query = new Sales().GetList()
                .Where((x) => x.GetCustomerName() == CustomerName && x.GetCustomerIC() == NRIC);
            decimal total = 0;
            foreach (var item in query)
            {
                decimal t2 = 0;
                foreach (var kvp in item.Paid)
                {
                    t2 += kvp.Value;
                }
                total += t2;
            }
            return total;
        }
        public decimal getTotal()
        {
            var query = new Sales().GetList()
                .Where((x) => x.GetCustomerName() == CustomerName && x.GetCustomerIC() == NRIC);
            decimal total = 0;
            foreach (var item in query)
            {
                total += item.GetNeedToPay();
            }
            return total;
        }
        public decimal getOwe()
        {
            var query = new Sales().GetList()
                .Where((x) => x.GetCustomerName() == CustomerName && x.GetCustomerIC() == NRIC);
            decimal total = 0;
            foreach (var item in query)
            {
                total += item.GetUnpaid();
            }
            return total;
        }
    }
}

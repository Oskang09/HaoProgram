using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaoProgram
{
    public class AgentCommission : IJsonObject<AgentCommission>
    {
        public string CategoryName { get; set; }
        public List<Tuple<Product, decimal, CommissionType>> Product { get; set; }

        public AgentCommission() : base(AppDomain.CurrentDomain.BaseDirectory + "/data/commission")
        {

        }
    }
    public enum CommissionType
    {
        PERCENTAGE,
        INTEGER
    }
    public static class CommissionTypeExtension
    {
        public static CommissionType FromString(string obj)
        {
            switch (obj)
            {
                case "$":
                    return CommissionType.INTEGER;
                case "%":
                    return CommissionType.PERCENTAGE;
            }
            return CommissionType.PERCENTAGE;
        }
        public static string ToString(CommissionType ct)
        {
            switch (ct)
            {
                case CommissionType.INTEGER:
                    return "$";
                case CommissionType.PERCENTAGE:
                    return "%";
            }
            return "";
        }
    }
}

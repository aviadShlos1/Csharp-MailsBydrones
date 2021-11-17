using System;

namespace IBL.BO
{
    public class AssignCustomerToParcel
    {
        public int Id { get; set; }
        public int Name { get; set; }

        public override string ToString()
        {
            return $"AssignCustomerToParcel: CustomerId:{Id}, CustomerName:{Name}";
        }
    }
}

using System;

namespace IBL.BO
{
    public class AssignCustomerToParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"CustomerId: {Id}, CustomerName: {Name}";
        }
    }
}

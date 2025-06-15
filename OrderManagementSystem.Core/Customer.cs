using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Core
{
    /// <summary>
    /// class represent customer in order management system.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique identifier for the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the customer
        /// </summary>
        [Required]
        public required string Name { get; set; }

        /// <summary>
        /// what segment that customer falls in.
        /// </summary>
        [Required]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// represent orders done by this customer.
        /// </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
namespace ShoeStore_Group9.Models
{
    public class User
    {
       public int UserID { get; set; }
       public string Username { get; set; }
       public string PasswordHash { get; set; }
       public string Email { get; set; }
       public string Role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}

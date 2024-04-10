namespace IntexII.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product prod, int quantity)
        {
            CartLine? line = Lines
                .Where(x => x.Product.ProductId == prod.ProductId)
                .FirstOrDefault();
            //has this item already been added to our cart?
            if (line == null) //if it hasn't
            {
                Lines.Add(new CartLine()
                {
                    Product = prod,
                    Quantity = quantity,
                    
                });
            }
            else //if it has, don't add another line for the same product, just update the quantity
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product prod) => Lines.RemoveAll(x => x.Product.ProductId == prod.ProductId);

        public virtual void Clear() => Lines.Clear();

        public double CalculateTotal() => Lines.Sum(x => x.Product.Price * x.Quantity);


        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
            
        }
    }
}

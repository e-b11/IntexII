﻿using IntexII.Data;

namespace IntexII.Models
{
    public class EFIntexRepository : IIntexRepository
    {
        private ApplicationDbContext _context;

        public EFIntexRepository(ApplicationDbContext temp)
        {
            _context = temp;
        }

        public IQueryable<Customer> Customers => _context.Customers;
        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Single(x => x.CustomerId == id);
        }
        public IQueryable<Product> Products => _context.Products;
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public Product GetProductById(int id)
        {
            return _context.Products.Single(p => p.ProductId == id);
        }
        public void EditProduct(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
        }

        public IQueryable<Order> Orders => _context.Orders;
        public IQueryable<Order> GetCustomerOrders(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId);
        }
        public IQueryable<Order> GetFraudOrders()
        {
            return _context.Orders.Where(o => o.Fraud == 1);
        }
        public void AddOrder(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
        }
        public void EditOrder(Order order)
        {
            _context.Update(order);
            _context.SaveChanges();
        }
        public Order GetOrderById(int id)
        {
            return _context.Orders.Single(o => o.TransactionId == id);
        }
        public IQueryable<LineItem> LineItems => _context.LineItems;
        public void AddLineItem(LineItem lineItem)
        {
            _context.Add(lineItem);
            _context.SaveChanges();
        }

    }
}

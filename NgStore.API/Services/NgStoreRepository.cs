using Microsoft.EntityFrameworkCore;
using NgStore.API.Data;
using NgStore.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Services
{
    public interface INgStoreRepository
    {
        IEnumerable<Customer> getCustomers();
        Customer getCustomer(int customerId);
        IEnumerable<Order> getCustomerOrders(int customerId);
        Order getOrder(int orderId);
        void addNewOrder(Order order);
        IEnumerable<OrderItem> getOrderItems(int orderId);
        IEnumerable<Product> getProducts();
        bool Save();
    }

    public class NgStoreRepository : INgStoreRepository
    {
        private NgStoreDBContext _context;

        public NgStoreRepository(NgStoreDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> getCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer getCustomer(int customerId)
        {
            return _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
        }

        public IEnumerable<Order> getCustomerOrders(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }        

        public Order getOrder(int orderId)
        {
            return _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
        }

        public void addNewOrder(Order order)
        {
            _context.Orders.Add(order);
        }

        public IEnumerable<OrderItem> getOrderItems(int orderId)
        {
            return _context.OrderItems.Include(oi => oi.Product).Where(oi => oi.OrderId == orderId).ToList();
        }

        public IEnumerable<Product> getProducts()
        {
            return _context.Products.ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

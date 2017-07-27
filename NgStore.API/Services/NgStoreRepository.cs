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
        IEnumerable<OrderItem> getOrderItems(int orderId);
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
            return _context.Customer.ToList();
        }

        public Customer getCustomer(int customerId)
        {
            return _context.Customer.Where(c => c.Id == customerId).FirstOrDefault();
        }

        public IEnumerable<Order> getCustomerOrders(int customerId)
        {
            return _context.Order.Where(o => o.CustomerId == customerId).ToList();
        }        

        public Order getOrder(int orderId)
        {
            return _context.Order.Where(o => o.Id == orderId).FirstOrDefault();
        }

        public IEnumerable<OrderItem> getOrderItems(int orderId)
        {
            return _context.OrderItem.Include(oi => oi.Product).Where(oi => oi.OrderId == orderId).ToList();
        }
    }
}

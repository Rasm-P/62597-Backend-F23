﻿using Microsoft.EntityFrameworkCore;
using ShopBackend.Contexts;
using ShopBackend.Models;

namespace ShopBackend.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly DBContext _dbContext;

        public OrderRepository(DBContext dbContext){
            _dbContext = dbContext;        
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public async Task<Order?> Get(Guid orderId)
        {
            return await _dbContext.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(c => c.Id == orderId);
        }


        public async Task<int> Insert(Order order)
        {
            _dbContext.Add(order);
            return await _dbContext.SaveChangesAsync();

        }
        public async Task<int> Update(Order order)
        {
            _dbContext.Update(order);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> Delete(Guid orderId)
        {
            _dbContext.Orders.Remove(new Order { Id = orderId });
            return await _dbContext.SaveChangesAsync();
        }

    }
}

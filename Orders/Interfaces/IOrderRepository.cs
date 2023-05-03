using Orders.DTO;
using Orders.Entities;

namespace Orders.Interfaces;

public interface IOrderRepository
{
    public void CreateOrders(IEnumerable<Order> orders);
    public IQueryable<Order> GetAll();
    public IQueryable<Order> GetById(int id);
    public void Create(Order order);
    public void Update(int key, Order order);
    public void Delete(Order order);
}

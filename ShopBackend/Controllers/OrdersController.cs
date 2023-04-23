﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;

namespace ShopBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var orders = (await _orderRepository.GetAll()).Select(order => order.AsOrderDto());
            if (orders.Any())
            {
                return Ok(orders);
            }

            return NotFound("The specified orders does not exist!"); ;
        }


        // GET api/<OrdersController>/5
        [HttpGet("{orderId}", Name= "GetOrderById")]
        public async Task<ActionResult<OrderDto>> Get(Guid orderId)
        {
            var order = await _orderRepository.Get(orderId);
            if (order != default)
            {
                return Ok(order.AsOrderDto());
            }

            return NotFound("The specified order does not exist!");
        }


        // POST api/<OrdersController>
        [EnableCors("FrontendPolicy")]
        [HttpPost]
        public async Task<ActionResult<string>> Create([FromBody] CreateOrderDto order)
        {
            if (order.OrderDetails.Count == 0)
            {
                return BadRequest("ProductDetails is required to register the order!");
            }

            var result = await _orderRepository.Insert(order.CreateAsOrderModel());
            if (result != default && result > 0)
            {
                return Created("CreateOrder","Order is inserted successfully!");
            }

            return NotFound("Order could not be registered!");
        }


        // PUT api/<OrdersController>/5
        [HttpPut("{orderId}")]
        public async Task<ActionResult<string>> Update(Guid orderId, [FromBody] OrderDto order)   
        {
            var orderToUpdate = await _orderRepository.Get(orderId);
            if (orderToUpdate == default)
            {
                return NotFound("Order does not exsist!");
            }

            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.OrderStatus = order.OrderStatus;
            orderToUpdate.CheckMarketing = order.CheckMarketing;
            orderToUpdate.SubmitComment = order.SubmitComment;

            var result = await _orderRepository.Update(orderToUpdate);
            if (result != default && result > 0)
            {
                return Ok("Order has been updated!");
            }

            return NotFound("Order could not be updated!");
        }


        // DELETE api/<OrdersController>/5
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<string>> Delete(Guid orderId)
        {

            var result = await _orderRepository.Delete(orderId);

            if (result != default && result > 0)
            {
                return Ok("Order has been deleted!");
            }
            return NotFound("Order could not be deleted!");
        }
    }
}

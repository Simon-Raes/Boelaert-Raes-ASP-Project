﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LayeredBusinessModel.DAO;
using LayeredBusinessModel.Domain;

namespace LayeredBusinessModel.BLL
{
    public class OrderService
    {
        OrderDAO orderDAO;

        //public List<Order> getAll()
        //{
        //    orderDAO = new OrderDAO();
        //    return orderDAO.getAll();
        //}

        public Order getOrder(String id)
        {
            orderDAO = new OrderDAO();
            return orderDAO.getOrder(id);
        }

        public void updateOrder(Order order)
        {
            orderDAO = new OrderDAO();
            orderDAO.updateOrder(order);
        }

        public List<Order> getOrdersForCustomer(int customer_id)
        {
            orderDAO = new OrderDAO();
            return orderDAO.getOrdersForCustomer(customer_id);
        }

        public int addOrderForCustomer(int customer_id)
        {
            orderDAO = new OrderDAO();
            return orderDAO.addOrderForCustomer(customer_id);
        }

        public void clearTable()
        {
            orderDAO = new OrderDAO();
            orderDAO.clearTable();
        }
    }
}

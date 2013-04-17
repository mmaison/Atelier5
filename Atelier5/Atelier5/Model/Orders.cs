using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace Atelier5.Model
{
    public partial class Orders
    {
       private Model.EntrepriseEntities _context;

        public Decimal Amount
        {
           get{
               var SumMontant=  from orders in _context.Order_Details
                                select orders.Unit_Price;
               var quantité = from orders in _context.Order_Details
                              select orders.Quantity;
               var feight = from orders in _context.Order_Details
                            select orders.Orders.Freight;
          
               
               if (SumMontant.Sum() != 0)
               {

                   return ((SumMontant * quantité) + feight);
                   
               }else{
                   return 0;
               }     
           }
            
        }
    }
}

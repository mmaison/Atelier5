using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Atelier5.ViewModel
{
    public class FilteredListViewModel : INotifyPropertyChanged
    {
        private int _selectedFilter = 0;
        private readonly string[] _filters;
        private Model.EntrepriseEntities _context;
       
        public FilteredListViewModel(Model.EntrepriseEntities context)
        {
           _context = context;
            _filters = "Tout le staff,10$ et moins,Anniversaire Janvier, order by age, Commandes Francaises, Prix Moyen Par Catégories,Test Order Amount".Split(',');
        }
       
        public IEnumerable<object> FilteredList
        {
            get
            {
                switch (this._selectedFilter)
                {
                    case 0:
                        return from employee in _context.Employees
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name
                               };
                    case 1:
                        return from product in _context.Products
                               where product.Unit_Price < 10.0m
                               select new
                               {
                                   Produit = product.Product_Name,
                                   Prix = product.Unit_Price
                               };
                    case 2 :
                        return from employee in _context.Employees
                               where employee.Birth_Date.HasValue == true
                               where employee.Birth_Date.Value.Month == 01
                               let age = DateTime.Now.Year - employee.Birth_Date.Value.Year
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name,
                                   Jour = employee.Birth_Date,
                                   Age= age 
                               };
                    case 3 : 
                        return from employee in _context.Employees
                        let age = DateTime.Now.Year - employee.Birth_Date.Value.Year
                        orderby age
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name,
                                   Age = age
                               };
                    case 4:
                        return from employee in _context.Employees
                               where employee.Country.Equals("France")
                               let count = (
                                    from orders in _context.Orders
                                    where orders.Employee_ID == employee.Employee_ID
                                    select employee.Employee_ID
                               )
                               select new
                               {
                                   Prénom = employee.First_Name,
                                   Nom = employee.Last_Name,
                                   Country = employee.Country,
                                   Nbcommandes = count.Count(),
                               };
                    case 5:
                        return from categories in _context.Categories
                               let average = (
                                    from products in _context.Products
                                    where categories.Category_ID == products.Category_ID
                                    select products.Unit_Price
                               )
                               select new
                               {
                                   Categorie = categories.Category_Name,
                                   Prix_Moyen = average.Average()
                               };

                    case 6:
                        return from orders in _context.Orders
                               let amount = orders.Amount
                               select new
                               {
                                   Order = orders.Order_Date,
                                   amount= amount
                               };

                    default:
                        return new string[] {
                        "(Not implemented filter)"
                    };
                }
            }
        }
        public IEnumerable<String> Filters
        {
            get { return _filters; }
        }
        public int SelectedFilter
        {
            get { return this._selectedFilter; }
            set
            {
                this._selectedFilter = value;
                if (PropertyChanged != null)
                    PropertyChanged(this,
                    new PropertyChangedEventArgs("FilteredList")
                    );
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

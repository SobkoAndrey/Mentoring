using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task.Surrogates
{
    public class OrderSurrogate : IDataContractSurrogate
    {
        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public Type GetDataContractType(Type type)
        {
            if (typeof(Order).IsAssignableFrom(type))
                return typeof(Order);
            if (typeof(Customer).IsAssignableFrom(type))
                return typeof(Customer);
            if (typeof(Shipper).IsAssignableFrom(type))
                return typeof(Shipper);
            if (typeof(Employee).IsAssignableFrom(type))
                return typeof(Employee);
            if (typeof(HashSet<Order_Detail>).IsAssignableFrom(type))
                return typeof(HashSet<Order_Detail>);
            return type;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            Order order = obj as Order;

            if (order != null)
            {
                var entity = new Order()
                {
                    OrderID = order.OrderID,
                    Customer = order.Customer,
                    CustomerID = order.CustomerID,
                    Employee = order.Employee,
                    EmployeeID = order.EmployeeID,
                    Freight = order.Freight,
                    OrderDate = order.OrderDate,
                    Order_Details = order.Order_Details,
                    RequiredDate = order.RequiredDate,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipCountry = order.ShipCountry,
                    ShipName = order.ShipName,
                    ShippedDate = order.ShippedDate,
                    Shipper = order.Shipper,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipRegion = order.ShipRegion,
                    ShipVia = order.ShipVia
                };
                return entity;
            }

            Customer customer = obj as Customer;

            if (customer != null)
            {
                var entity = new Customer()
                {
                    Address = customer.Address,
                    City = customer.City,
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Country = customer.Country,
                    CustomerID = customer.CustomerID,
                    Fax = customer.Fax,
                    Phone = customer.Phone,
                    PostalCode = customer.PostalCode,
                    Region = customer.Region
                };
                return entity;
            }

            Shipper shipper = obj as Shipper;

            if (shipper != null)
            {
                var entity = new Shipper()
                {
                    Phone = shipper.Phone,
                    CompanyName = shipper.CompanyName,
                    ShipperID = shipper.ShipperID
                };
                return entity;
            }

            Employee employee = obj as Employee;

            if (employee != null)
            {
                var entity = new Employee()
                {
                    Region = employee.Region,
                    PostalCode = employee.PostalCode,
                    Address = employee.Address,
                    BirthDate = employee.BirthDate,
                    City = employee.City,
                    Country = employee.Country,
                    EmployeeID = employee.EmployeeID,
                    Extension = employee.Extension,
                    FirstName = employee.FirstName,
                    HireDate = employee.HireDate,
                    HomePhone = employee.HomePhone,
                    LastName = employee.LastName,
                    Notes = employee.Notes,
                    Photo = employee.Photo,
                    PhotoPath = employee.PhotoPath,
                    ReportsTo = employee.ReportsTo,
                    Title = employee.Title,
                    TitleOfCourtesy = employee.TitleOfCourtesy
                };
                return entity;
            }

            HashSet<Order_Detail> set = obj as HashSet<Order_Detail>;

            if (set != null)
            {
                var sett = new List<Order_Detail>();

                foreach(var item in set)
                {
                    var entity = new Order_Detail()
                    {
                        Discount = item.Discount,
                        OrderID = item.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };

                    sett.Add(entity);
                }

                return sett;
            }

            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
    }
}

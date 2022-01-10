using OfficeModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeModel.Data
{
    public class DbInitializer
    {
        public static void Initialize(OfficeContext context)
        {
            context.Database.EnsureCreated();
            if (context.Products.Any())
            {
                return; // BD a fost creata anterior
            }
            var products = new Product[]
            {
new Product{Name="Set 10 creioane",Price=Decimal.Parse("10")},
new Product{Name="Set 20 creioane colorate",Price=Decimal.Parse("18")},
new Product{Name="Set 10 markere",Price=Decimal.Parse("12")},
new Product{Name="Set 10 markere colorate",Price=Decimal.Parse("22")},
new Product{Name="Bax hartie alba",Price=Decimal.Parse("23")},
new Product{Name="Set 10 caiete studentesti",Price=Decimal.Parse("32")},
new Product{Name="Set 10 caiete dictando",Price=Decimal.Parse("11")},
new Product{Name="Set geometrie",Price=Decimal.Parse("24")},
new Product{Name="Set radiere",Price=Decimal.Parse("6")},
new Product{Name="Set post-it",Price=Decimal.Parse("2")},
new Product{Name="Set 10 pixuri",Price=Decimal.Parse("10")},


            };
            foreach (Product s in products)
            {
                context.Products.Add(s);
            }
            context.SaveChanges();
            var clients = new Client[]
            {
new Client{ClientID=1050,Name="Popescu Marcela"},
new Client{ClientID=1051,Name="Berari Paul"},
new Client{ClientID=1052,Name="Pop Mihai"},
new Client{ClientID=1053,Name="Sabau Mihai"},
new Client{ClientID=1054,Name="Uliu Pavel"},
new Client{ClientID=1055,Name="Niculae Margareta"},
            };
            foreach (Client c in clients)
            {
                context.Clients.Add(c);
            }
            context.SaveChanges();

            var orders = new Order[]
            {
new Order{ProductID=1,ClientID=1050,OrderDate=DateTime.Parse("11-12-2021")},
new Order{ProductID=3,ClientID=1051,OrderDate=DateTime.Parse("12-12-2021")},
new Order{ProductID=1,ClientID=1052,OrderDate=DateTime.Parse("13-12-2021")},
new Order{ProductID=2,ClientID=1053,OrderDate=DateTime.Parse("14-12-2021")},
new Order{ProductID=4,ClientID=1054,OrderDate=DateTime.Parse("15-12-2021")},
new Order{ProductID=6,ClientID=1055,OrderDate=DateTime.Parse("16-12-2021")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
            var suppliers = new Supplier[]
{
new Supplier{SupplierName="SC MultiMass SRL",Address="Str. Aviatorilor, nr. 40, Bucuresti"},
new Supplier{SupplierName="SC MaxOffice SRL",Address="Str. Plopilor, nr. 35, Bucuresti"},
new Supplier{SupplierName="SC BigDepot SRL",Address="Str. Cascadelor, nr. 22, Oradea"},
};
            foreach (Supplier s in suppliers)
            {
                context.Suppliers.Add(s);
            }
            context.SaveChanges();

            var suppliedproducts = new SuppliedProduct[]
            {
                new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Set 10 creioane" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC MultiMass SRL").SupplierID
                },
                new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Set 20 creioane colorate" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC MaxOffice SRL").SupplierID
                },
                new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Set 10 markere" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC BigDepot SRL").SupplierID
                },new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Set 10 markere colorate" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC MaxOffice SRL").SupplierID
                },new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Bax hartie alba" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC MultiMass SRL").SupplierID
                },
                new SuppliedProduct {
                ProductID = products.Single(c => c.Name == "Set 10 caiete studentesti" ).ProductID,
                SupplierID = suppliers.Single(i => i.SupplierName == "SC BigDepot SRL").SupplierID
                                },
};
            foreach (SuppliedProduct sp in suppliedproducts)
            {
                context.SuppliedProducts.Add(sp);
            }
            context.SaveChanges();
        }
    }

}


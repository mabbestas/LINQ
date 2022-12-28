using System;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            NorthwindEntities db = new NorthwindEntities();

            List<Categories> categories = (from c in db.Categories
                                           where c.CategoryID == 3
                                           select c).ToList();



            // ORDERBY AND DESCENDİNG
            List<Categories> categories = (from c in db.Categories
                                           orderby c.CategoryName descending
                                           select c).ToList();


            // CONTAINS
            List<Categories> categories = (from c in db.Categories
                                           orderby c.CategoryName.Contains("e")
                                           select c).ToList();




            // COUNT
            List<Categories> categories = (from c in db.Categories
                                           select c).ToList();





            // METHOD BASE

            // WHERE
            List<Categories> categories = db.Categories.Where(x => x.CategoryID == 3).ToList();


            // ORDERBY
            List<Categories> categories = db.Categories.OrderBy(x => x.CategoryName).ToList();


            // ORDERBYDESCENDING
            List<Categories> categories = db.Categories.OrderByDescending(x => x.CategoryName).ToList();
            foreach (var item in categories)
            {
                Console.WriteLine(item.CategoryName);
            }


            // JOIN
            var innerJoin = db.Products.Join(db.Categories,
                p => p.CategoryID,
                c => c.CategoryID,
                (p, c) => new
                {
                    c.CategoryName,
                    p.ProductName
                });


            // CONTAINS, FIRSTORDEFAULT
            Categories categories = db.Categories.FirstOrDefault(x => x.CategoryID == 5);

            bool areThereCategories = db.Categories.Contains(categories);


            // COUNT
            int adet = db.Categories.Count();

            decimal? fiyat = db.Products.Max(p => p.UnitPrice);


            // SUM
            int? urunAdet = db.Products.Sum(p => p.UnitsInStock);

            List<Products> products = db.Products.OrderBy(x => x.ProductID).Skip(20).Take(5).ToList();
            Console.WriteLine(products);


            // GROUPBY
            var liste = db.Products.GroupBy(p => p.Categories.CategoryName).Select(p => new { CategoryName = p.Key, Adet = p.Count() }).ToList();
            var siraliListe = liste.OrderByDescending(x => x.Adet).ToList();


            // 3 LÜ JOIN ORDER, OREDER_DETAİLS, PRODUCT

            var liste = from o in db.Orders
                        join od in db.Order_Details on o.OrderID equals od.OrderID
                        join p in db.Products on od.ProductID equals p.ProductID
                        select new
                        {
                            OrderId = o.OrderID,
                            OrderDate = o.OrderDate,
                            ProductName = p.ProductName
                        };


            // employess tablosundan ünvanı Mr. olan çalışanların adı soyadı v unvan bilgilerini listele


            var emp = (from e in db.Employees
                       where e.TitleOfCourtesy == "Mr."
                       select new
                       {
                           e.TitleOfCourtesy,
                           e.FirstName,
                           e.LastName
                       }
            ).ToList();



            // 1950 ile 1961 yılında doğan çalışanların adı, soyadı ve doğum tarihi bilgilerini listele.

            DateTime dogumTarihi1 = new DateTime(1951, 1, 1);
            DateTime dogumTarihi2 = new DateTime(1961, 12, 31);
            List<Employees> emp = db.Employees.Where(e => e.BirthDate >= dogumTarihi1 && e.BirthDate <= dogumTarihi2).ToList();


            var emp = (from e in db.Employees
                       where e.BirthDate.Value.Year >= 1951 && e.BirthDate.Value.Year <= 1961
                       select new
                       {
                           e.FirstName,
                           e.LastName,
                           e.BirthDate
                       }
                       ).ToList();


            // Employess tablosundaki çalışanların ad, soyad, doğum tarihi bilgilerini lastname e göre azalan sırada listele.
            var emp = (from e in db.Employees.OrderByDescending(e => e.LastName)
                       select new
                       {
                           e.FirstName,
                           e.LastName,
                           e.BirthDate
                       }
                       ).ToList();


            // Siparişlerin içinde bulunan her bir ürün için ödenecek toplam fiyatı artan sırada sıralayınız.  (yapılamadı)
            var emp = (from od in db.Order_Details
                       orderby od.ProductID
                       select new
                       {
                           od.UnitPrice,
                           od.Quantity,
                           od.Discount
                       }
                       );
            decimal total;
            decimal newItem = 1;

            foreach (var item in emp)
            {
                total = newItem * Convert.ToDecimal(item);
                newItem = total;
            }


            // çalışanların içerisinde adı A ile başlayanları listele?
            var emp = (from e in db.Employees
                       where e.FirstName.StartsWith("A")
                       select new
                       {
                           e.FirstName,
                           e.LastName
                       }
                        ).ToList();

            Console.ReadLine();
        }
    }
}

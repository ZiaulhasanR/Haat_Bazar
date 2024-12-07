using Haat_Bazar.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Haat_Bazar.Controllers
{
    public class CustomerController : Controller
    {
        HaatBazarEntities6 db = new HaatBazarEntities6();
        // GET: Customer
        public ActionResult Index(int ?page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.catagories.Where(x => x.catStatus == 1).OrderByDescending(x => x.catId).ToList();
            IPagedList<catagory> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);
        }
        public ActionResult SignUpPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUpPage(customer cvm, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be Uploaded....";
            }
            else
            {
                customer cs = new customer();
                cs.customerName = cvm.customerName;
                cs.customerEmail = cvm.customerEmail;
                cs.customerPhone = cvm.customerPhone;
                cs.customerPass = cvm.customerPass;
                cs.customerImage = path;
                db.customers.Add(cs);
                db.SaveChanges();
                return RedirectToAction("LoginPage");
            }
            return View();
        }


        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(customer cvm)
        {
            customer cs = db.customers.Where(x => x.customerEmail == cvm.customerEmail && x.customerPass == cvm.customerPass).SingleOrDefault();
            if (cs != null)
            {
                Session["customerId"] = cs.customerId.ToString();
               
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }

        public ActionResult ViewProduct(int ?id,int?page)
        {
            Session["CustomerId"] = 1;
            if (TempData["cart"] != null)
            {
                float x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    x += item.bill;

                }

                TempData["total"] = x;
            }
            TempData.Keep();
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.Where(x=>x.CatagorieId== id).OrderByDescending(x=>x.ProductId).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);

            
        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {

                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {

                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }
            return path;
        }

        public ActionResult Adtocart(int? Id)
        {
            
            Product p = db.Products.Where(x => x.ProductId == Id).SingleOrDefault();
            return View(p);
        }
        List<cart> li = new List<cart>();
        [HttpPost]
        public ActionResult Adtocart(Product pi, string qty, int Id)
        {
            Product p = db.Products.Where(x => x.ProductId == Id).SingleOrDefault();

            cart c = new cart();
            c.productid = p.ProductId;
            c.price = (float)p.ProductPrice;
            c.qty = Convert.ToInt32(qty);
            c.bill = (c.price *((100-(float)p.Discount)/100))* c.qty;

            c.productname = p.ProductName;
            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;

            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.productid == c.productid)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }

                TempData["cart"] = li2;
            }

            TempData.Keep();




            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            return View(db.Products.Where(x => x.ProductId == id).FirstOrDefault());
        }
        public ActionResult checkout()
        {
            TempData.Keep();


            return View();
        }

        [HttpPost]
        public ActionResult checkout(Order o)
        {
            List<cart> li = TempData["cart"] as List<cart>;

            Invoice iv = new Invoice();
            iv.CustomerId = Convert.ToInt32(Session["CustomerId"].ToString());

            iv.InvoiceDate = System.DateTime.Now;
            iv.TotalBill = (int)(float)TempData["total"];
            db.Invoices.Add(iv);
            db.SaveChanges();

            foreach(var item in li)
            {
                Order od = new Order();
                od.ProductId = item.productid;
                od.InvoiceId = iv.InvoiceId;
                od.OrderDate = System.DateTime.Now;
                od.Quantity = item.qty;
                od.UnitPrice = (int)item.price;
                od.Bill = (int)item.bill;
                db.Orders.Add(od);
                db.SaveChanges();
            }
            TempData.Remove("total");
            TempData.Remove("cart");
            TempData["msg"] = "Order Placed Successfully...";
            TempData.Keep();
            return RedirectToAction("ViewProduct");

        }
    }
}
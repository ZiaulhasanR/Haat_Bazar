using Haat_Bazar.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;

namespace Haat_Bazar.Controllers
{
    public class AdminController : Controller
    {
        HaatBazarEntities6 db = new HaatBazarEntities6();
        // GET: Admin
        [HttpGet]
        public ActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginPage(admin_table avm)
        {
            admin_table ad=db.admin_table.Where(x => x.admin_username == avm.admin_username && x.admin_pass == avm.admin_pass).SingleOrDefault();
            if(ad!=null)
            {
                Session["admin_id"] = ad.admin_id.ToString();
                return RedirectToAction("AdminPanel");
            }
            else
            {
                ViewBag.error = "Invalid username or password";

            }
            return View();
        }

        public ActionResult AdminPanel()
        {

            if (Session["admin_id"] == null)
            {
                return RedirectToAction("LoginPage");
            }
            return View();
        }

        public ActionResult HomePage()
        {

            
            return View();
        }

        public ActionResult Create()
        {

            if (Session["admin_id"] == null)
            {
                return RedirectToAction("LoginPage");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(catagory cvm,HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be Uploaded....";
            }
            else
            {
                catagory cat = new catagory();
                cat.catName = cvm.catName;
                cat.catImage = path;
                cat.catStatus = 1;
                cat.admin_id=Convert.ToInt32(Session["admin_id"].ToString());
                db.catagories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            
            return View();
        }
        public ActionResult ViewCategory(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.catagories.Where(x => x.catStatus == 1).OrderByDescending(x => x.catId).ToList();
            IPagedList<catagory> stu = list.ToPagedList(pageindex, pagesize);
   
            return View(stu);
        }

        public ActionResult ViewOrder()
        {


            return View(db.Orders.ToList());
        }

        

        


        public ActionResult AddProduct1()
        {
            
            
            List<catagory> ctli = db.catagories.ToList();
            ViewBag.catagorylist = new SelectList(ctli, "catId", "catName");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct1(Product pr, HttpPostedFileBase imgfile)
        {
            string path = uploadimgfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be Uploaded....";
            }
            else
            {
                Product prd = new Product();

                
                prd.CatagorieId = pr.CatagorieId;
                prd.ProductName = pr.ProductName;
                prd.ProductStock = pr.ProductStock;
                prd.ProductPrice = pr.ProductPrice;
                prd.ProductDes = pr.ProductDes;
                prd.Discount = pr.Discount;
                prd.ProductImage = path;
                
                if (Session["admin_id"] != null)
                {
                    prd.admin_id = Convert.ToInt32(Session["admin_id"].ToString());
                }
                else
                {
                    ViewBag.error = "Please Log in first....";
                }

                //prd.admin_id = Convert.ToInt32(Session["admin_id"].ToString());

                db.Products.Add(prd);
                db.SaveChanges();

                return RedirectToAction("AddProduct1");
            }

            return View();
        }

        public ActionResult ViewProduct(int? page)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Products.OrderByDescending(x => x.ProductId).ToList();
            IPagedList<Product> stu = list.ToPagedList(pageindex, pagesize);

            return View(stu);
        }
        public ActionResult EditCatagory(int id)
        {

            return View(db.catagories.Where(x => x.catId == id).FirstOrDefault());

        }
        [HttpPost]
        public ActionResult EditCatagory(int id, catagory Ct)
        {

            db.Entry(Ct).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewCategory");
        }
        public ActionResult EditProduct(int id)
        {
            
            return View(db.Products.Where(x => x.ProductId == id).FirstOrDefault());
            
        }
        [HttpPost]
        public ActionResult EditProduct(int id,Product product)
        {
            
            db.Entry(product).State = (System.Data.Entity.EntityState)System.Data.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewProduct");
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
                        //file.SaveAs(path);
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

    }
}
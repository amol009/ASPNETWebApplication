using ASPNETWebApplication.Context;
using ASPNETWebApplication.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ASPNETWebApplication.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        MVCTestDbEntities1 dbObj = new MVCTestDbEntities1();
        public ActionResult Staff(StaffDetail obj)
        {
            if(obj != null)
                return View(obj);
            else
                return View();
        }

        [HttpPost]
        public ActionResult AddStaff(StaffDetail model)
        {
            if (ModelState.IsValid)
            {
                StaffDetail obj = new StaffDetail();
                obj.id = model.id;
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.Phone = model.Phone;
                obj.ProfilePic = model.ProfilePic;
                obj.Country = model.Country;
                obj.Status = model.Status;

                if (model.id == 0)
                {
                    dbObj.StaffDetails.Add(obj);
                    dbObj.SaveChanges();
                    ViewBag.Message = "Record Save Successfully.!";
                }
                else
                {
                    dbObj.Entry(obj).State = EntityState.Modified;
                    dbObj.SaveChanges();
                    ViewBag.Message = "Record Updated Successfully.!";
                }
            }
            ModelState.Clear();
            return View("Staff");
        }

        //public ActionResult StaffList()
        //{
        //    var res = dbObj.StaffDetails.ToList();
        //    return View(res);
        //}

        int pageSize = 3;
        public ActionResult StaffList(int pg =1)
        {
            List<StaffDetail> staffDetails = dbObj.StaffDetails.ToList();

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = staffDetails.Count();

            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = staffDetails.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            return View(data);
        }
        public ActionResult Delete(int id)
        {
            var res=dbObj.StaffDetails.Where(x=>x.id==id).First();
            dbObj.StaffDetails.Remove(res);
            dbObj.SaveChanges();

            var list = dbObj.StaffDetails.ToList();
            return View("StaffList",list);
        }

        public ActionResult GetData()
        {
            using (MVCTestDbEntities1 db = new MVCTestDbEntities1())
            {
                List<StaffDetail> staffList = db.StaffDetails.ToList<StaffDetail>();
                return Json(new { data = staffList},JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult ListMyItems(int page)
        //{
        //    const int pageSize = 10;
        //    List<StaffDetail> list = dbObj.StaffDetails.ToList();
        //    var count = list.Count();
        //    var data = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    ViewData["ItemList"] = data;
        //    ViewData["PageNumber"] = page;
        //    ViewData["TotalPages"] = Math.Ceiling((double)count / pageSize);
        //    return View();
        //}

        
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;
using WorkFlowaMig.Models;

namespace WorkFlowaMig.Controllers
{
    public class ZoneController : Controller
    {
        private WorkflowEntities _context;

        //Constructeur 

        public ZoneController()
        {
            _context = new WorkflowEntities();
        }
        // GET: /Zone/
        public ActionResult Index(string search, string sort, int? page)
        {
            IQueryable<Zone> zones = _context.Zones;

            if (!String.IsNullOrWhiteSpace(sort))
            {
                if (sort == "ZoneId_ASC")
                    zones = zones.OrderBy(c => c.Id);
                else if (sort == "ZoneId_DESC")
                    zones = zones.OrderByDescending(c => c.Id);

                else if (sort == "ZoneName_ASC")
                    zones = zones.OrderBy(c => c.Name);
                else if (sort == "ZoneName_DESC")
                    zones = zones.OrderByDescending(c => c.Name);
            }
            else
            {
                zones = zones.OrderByDescending(c => c.Id);
            }
            ViewBag.CurrentSort = sort;

            if (!String.IsNullOrWhiteSpace(search))
            {
                ViewBag.CurrentSearch = search;
                zones = zones.Where(c => c.Name.Contains(search));
            }
            //zones = zones.OrderByDescending(c => c.Id);
            int pageSize = 6;
            int pageNumber = page ?? 1;
          return PartialView(zones.ToPagedList(pageNumber, pageSize));

        }

        //Get all zones in db 

        public ActionResult GetZones()
        {
            var _zoneList = _context.Zones.ToList();
            return Json(_zoneList, JsonRequestBehavior.AllowGet);
        }

        //Get Zone By Id

        public ActionResult GetZone(int id)
        {
            var _zone = _context.Zones.Where(z => z.Id == id).FirstOrDefault();
            return Json(_zone, JsonRequestBehavior.AllowGet);
        }

        // Add zone 
        [HttpPost]
        public ActionResult AddZone([Bind(Exclude="Id")]Zone zone)
        {
            if(ModelState.IsValid)
            {
                _context.Zones.Add(zone);
                _context.SaveChanges();
               
            }
            return Json(zone, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Zone zone)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(zone).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return Json(zone, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var zone = _context.Zones.ToList().Find(m => m.Id == id);
            if (zone != null)
            {
                _context.Zones.Remove(zone);
                _context.SaveChanges();
            }

            return Json(zone, JsonRequestBehavior.AllowGet);
        }

	}
}
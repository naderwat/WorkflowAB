using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;
using WorkFlowaMig.Models;

namespace WorkFlowaMig.Controllers
{




    public class AgenceController : Controller
    {
        private WorkflowEntities _context;
        public AgenceController()
        {
            _context = new WorkflowEntities();
        }

        //
        // GET: /Agence/
        public ActionResult Index(string search, string sort, int? page)
        {
            IQueryable<Agence> agences = _context.Agences;

            if (!String.IsNullOrWhiteSpace(sort))
            {
                if (sort == "AgenceId_ASC")
                    agences = agences.OrderBy(c => c.Id);
                else if (sort == "AgenceName_DESC")
                    agences = agences.OrderByDescending(c => c.Id);

                else if (sort == "AgenceName_ASC")
                    agences = agences.OrderBy(c => c.Name);
                else if (sort == "AgenceName_DESC")
                    agences = agences.OrderByDescending(c => c.Name);
            }
            else
            {
                agences = agences.OrderByDescending(c => c.Id);
            }
            ViewBag.CurrentSort = sort;

            if (!String.IsNullOrWhiteSpace(search))
            {
                ViewBag.CurrentSearch = search;
                agences = agences.Where(c => c.Name.Contains(search));
            }
            //zones = zones.OrderByDescending(c => c.Id);
            int pageSize = 6;
            int pageNumber = page ?? 1;
            
           
            //ViewBag.ZoneID = new SelectList(_context.Zones, "Id", "Name");
            ViewBag.ZoneId = new SelectList(_context.Zones, "Id", "Name");

            return PartialView(agences.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult GetAgences()
        {
            var _listAgences = _context.Agences.ToList();
            return Json(_listAgences, JsonRequestBehavior.AllowGet); 

        }
        public ActionResult GetAgence(int id)
        {
            var _agence = _context.Agences.Where(u => u.Id == id).FirstOrDefault();
            return Json(_agence, JsonRequestBehavior.AllowGet); 

        }
        public ActionResult AddAgence([Bind(Include = "Id,Code,Name,ZoneId")] Agence agence)
        {
            if(ModelState.IsValid)
            {
                _context.Agences.Add(agence);
                _context.SaveChanges();
            }
            ViewBag.ZoneId = new SelectList(_context.Zones, "Id", "Name", agence.ZoneId);
            return Json(agence, JsonRequestBehavior.AllowGet); 

        }
        public ActionResult UpdateAgence(Agence agence)
        {
            //comment 
            if(ModelState.IsValid)
            {
                _context.Entry(agence).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(agence, JsonRequestBehavior.AllowGet); 
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var agence = _context.Agences.ToList().Find(m => m.Id == id);
            if (agence != null)
            {
                _context.Agences.Remove(agence);
                _context.SaveChanges();
            }

            return Json(agence, JsonRequestBehavior.AllowGet);
        }
	}
}
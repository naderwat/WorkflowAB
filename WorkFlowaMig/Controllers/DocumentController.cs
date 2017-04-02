using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc;
using WorkFlowaMig.Models;
using System.Data.Entity;

namespace WorkFlowaMig.Controllers
{
    public class DocumentController : Controller
    {
       private WorkflowEntities _context;

        //Constructeur 

       public DocumentController()
        {
            _context = new WorkflowEntities();
        }
        // GET: /Zone/
        public ActionResult Index(string search, string sort, int? page)
        {
            IQueryable<Document> document = _context.Documents;

            if (!String.IsNullOrWhiteSpace(sort))
            {
                if (sort == "DocumentId_ASC")
                    document = document.OrderBy(c => c.Id);
                else if (sort == "DocumentId_DESC")
                    document = document.OrderByDescending(c => c.Id);

                else if (sort == "DocumentName_ASC")
                    document = document.OrderBy(c => c.Libelle);
                else if (sort == "DocumentName_DESC")
                    document = document.OrderByDescending(c => c.Libelle);
            }
            else
            {
                document = document.OrderByDescending(c => c.Id);
            }
            ViewBag.CurrentSort = sort;

            if (!String.IsNullOrWhiteSpace(search))
            {
                ViewBag.CurrentSearch = search;
                document = document.Where(c => c.Libelle.Contains(search));
            }
            //zones = zones.OrderByDescending(c => c.Id);
            int pageSize = 6;
            int pageNumber = page ?? 1;
          return PartialView(document.ToPagedList(pageNumber, pageSize));

        }

        //Get all zones in db 

        public ActionResult GetDocuments()
        {
            var _docList = _context.Documents.ToList();
            return Json(_docList, JsonRequestBehavior.AllowGet);
        }

        //Get Zone By Id

        public ActionResult GetDocument(int id)
        {
            var _document = _context.Documents.Where(z => z.Id == id).FirstOrDefault();
            return Json(_document, JsonRequestBehavior.AllowGet);
        }

        // Add zone 
        [HttpPost]
        public ActionResult AddDocument([Bind(Exclude="Id")]Document document)
        {
            if(ModelState.IsValid)
            {
                _context.Documents.Add(document);
                _context.SaveChanges();
               
            }
            return Json(document, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(document).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return Json(document, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var document = _context.Documents.ToList().Find(m => m.Id == id);
            if (document != null)
            {
                _context.Documents.Remove(document);
                _context.SaveChanges();
            }

            return Json(document, JsonRequestBehavior.AllowGet);
        }
	}
}
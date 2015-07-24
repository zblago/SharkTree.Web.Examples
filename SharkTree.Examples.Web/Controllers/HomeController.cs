using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SharkDev.Web.Controls.TreeView.Model;
using System.Web.Script.Serialization;

namespace SharkTree.Examples.Web
{
    public class HomeController : Controller
    {
        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Simple()
        {
            List<Node> _lstTreeNodes = new List<Node>();

            _lstTreeNodes.Add(new Node() { Id = "12", Term = "Horse", ParentId = "9" });
            _lstTreeNodes.Add(new Node() { Id = "9", Term = "Equine", ParentId = "2" });
            _lstTreeNodes.Add(new Node() { Id = "3", Term = "Lizard", ParentId = "1" });
            _lstTreeNodes.Add(new Node() { Id = "15", Term = "Bessie", ParentId = "14" });
            _lstTreeNodes.Add(new Node() { Id = "2", Term = "Mammal" });
            _lstTreeNodes.Add(new Node() { Id = "4", Term = "Snake", ParentId = "1" });
            _lstTreeNodes.Add(new Node() { Id = "5", Term = "Bird", ParentId = "1" });
            _lstTreeNodes.Add(new Node() { Id = "6", Term = "Salamander", ParentId = "3" });
            _lstTreeNodes.Add(new Node() { Id = "7", Term = "Canary", ParentId = "5" });
            _lstTreeNodes.Add(new Node() { Id = "8", Term = "Tweetie", ParentId = "7" });
            _lstTreeNodes.Add(new Node() { Id = "10", Term = "Bovine", ParentId = "2" });
            _lstTreeNodes.Add(new Node() { Id = "13", Term = "Zebra", ParentId = "9" });
            _lstTreeNodes.Add(new Node() { Id = "14", Term = "Cow", ParentId = "10" });
            _lstTreeNodes.Add(new Node() { Id = "1", Term = "Reptile" });
            _lstTreeNodes.Add(new Node() { Id = "11", Term = "Canine", ParentId = "2" });

            ViewBag.SharkTreeData = _lstTreeNodes;

            return View();
        }

        public ActionResult BigTree()
        {
            List<Node> _lstTreeNodes;
            using (EFContext.SharkTreeTestEntities context = new EFContext.SharkTreeTestEntities())
            {
                _lstTreeNodes = context.TreeData.Select(x => new Node() { Id = x.ID.ToString(), Term = x.Term, ParentId = x.ParentId == 0 ? string.Empty : x.ParentId.ToString() }).ToList();
            }

            ViewBag.SharkTreeData = _lstTreeNodes;
            return View();
        }

        public ActionResult BigTreeWithAjaxAutoComplete()
        {
            List<Node> _lstTreeNodes;
            using (EFContext.SharkTreeTestEntities context = new EFContext.SharkTreeTestEntities())
            {
                _lstTreeNodes = context.TreeData.Select(x => new Node() { Id = x.ID.ToString(), Term = x.Term, ParentId = x.ParentId == 0 ? string.Empty : x.ParentId.ToString() }).ToList();
            }

            ViewBag.SharkTreeData = _lstTreeNodes;
            return View();
        }

        #endregion

        #region Ajax

        public string GetBySample(string q)
        {
            List<Node> listTree = new List<Node>();            
            using (EFContext.SharkTreeTestEntities context = new EFContext.SharkTreeTestEntities())
            {
                listTree = context
                        .TreeData.Where(x => x.Term.ToLower().StartsWith(q))
                        .Select(x => new Node() { Id = x.ID.ToString(), Term = x.Term, ParentId = x.ParentId == 0 ? string.Empty : x.ParentId.ToString() }).ToList();
            }

            string resultJson = new JavaScriptSerializer().Serialize(listTree);            

            return resultJson;
        }

        #endregion
    }
}

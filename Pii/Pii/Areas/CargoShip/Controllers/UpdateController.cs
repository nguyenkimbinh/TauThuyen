using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pii.Areas.CargoShip.ViewModel;
using Pii.Models;
using Pii.Areas.CargoShip.Models;
using System.Globalization;
using System.Data;
using System.Data.Entity;

namespace Pii.Areas.CargoShip.Controllers
{
    [Authorize(Roles = "manager, administrators")]
    public class UpdateController : Pii.Controllers.BaseController
    {
        //
        // GET: /CargoShip/Update/
        UpdateViewModel UViewModel = new UpdateViewModel();
        CargoShipRepository repository = new CargoShipRepository();
        public UpdateController()
        {
            repository.InitialUpdateViewModel(UViewModel);
        }
        public ActionResult Index(int shipId, string tripId, string IsRefresh)
        {
            ViewBag.isRefresh = IsRefresh;
            repository.SetUpdateViewModelShip(UViewModel, shipId, tripId);
            ViewBag.nations = DbContext.Nations.Select(p=>p.NationName).ToList();
            return View(UViewModel);
        }
        [HttpPost]
        //[Authorize(Roles = "manager, administrators")]
        public ActionResult Update(UpdateViewModel model)
        {
            // model.CurrentStartTime = DateTime.ParseExact(_CurrentStartTime, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            repository.UpdateGoodsAndJourney(model);
            repository.UpdateModels(model, User.Identity.Name);
            if (model.IsLate > 0)
            {
                return RedirectToAction("UpdateReson", model);
            }
            //repository.SaveChange();
            return RedirectToAction("Index", new { shipId = model.Ship.ShipId, tripId = model.CurrentTripId, IsRefresh = "true" });
        }
        public ActionResult UpdateReson(UpdateViewModel model)
        {
            var result = repository.GetResult(model);
            ViewBag.availableResons = repository.GetResons();
            return View(result);
        }
        [HttpPost]
        public ActionResult UpdateReson(IEnumerable<Result> model, string modify)
        {
            repository.UpdateResult(model);
            if (string.IsNullOrEmpty(modify))
            {
                return RedirectToAction("Detail", "Home", new { shipId = model.FirstOrDefault().ShipId, tripId = model.FirstOrDefault().TripId, all = true, currentTrip = true });
            }
            return RedirectToAction("Index", new { tripId = model.FirstOrDefault().TripId, shipId = model.FirstOrDefault().ShipId });
        }
        public ActionResult Modify(int jnId)
        {
            var model = DbContext.JourneySuppervision.Find(jnId);
            ViewBag.availablePorts = DbContext.Ports.ToList();
            ViewBag.availableTasks = DbContext.Tasks.ToList();
            ViewBag.availableStates = DbContext.States.ToList();
            ViewBag.availableCategories = DbContext.Categories.ToList();
            if (model != null)
            {
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Modify(JourneySupervision model, DateTime? oldTime)
        {
            if (ModelState.IsValid)
            {
                model.Updator = User.Identity.Name;
                model.UpdateTime = DateTime.Now;
                DbContext.Entry(model).State = EntityState.Modified;
                if (model.CreatedDate.Value.Date != oldTime.Value.Date)
                {
                    var list = new List<Result>();
                    var result =repository.UpdateResult(model.ShipId, model.TripId, model.StateId,model.PortId);
                    if (result !=null)
                    {
                        foreach (var item in result)
                        {

                            item.JourneyDate = model.CreatedDate;
                            if (item.PlanDate != null)
                            {
                                var days = (item.PlanDate.Value.Date - model.CreatedDate.Value.Date).Days;
                                if (days > 0)
                                {
                                    var mode = DbContext.Results.Find(item.ResultId);
                                    mode.Remark = "Sớm hơn " + days + " ngày so với dự kiến";
                                    mode.JourneyDate = model.CreatedDate;
                                    list.Add(mode);
                                    DbContext.SaveChanges();
                                }
                                if (days < 0)
                                {
                                    var mode = DbContext.Results.Find(item.ResultId);
                                    mode.Remark = "Trễ hơn " + -days + " ngày so với dự kiến";
                                    mode.JourneyDate = model.CreatedDate;
                                    list.Add(mode);
                                    DbContext.SaveChanges();
                                }
                                if (days == 0)
                                {
                                    var mode = DbContext.Results.Find(item.ResultId);
                                    mode.Remark = "Đúng theo dự kiến";
                                    mode.JourneyDate = model.CreatedDate;
                                    mode.ResonId = null;
                                    DbContext.SaveChanges();
                                }
                            }
                        }
                        if (list.Count>0)
                        {
                            ViewBag.availableResons = repository.GetResons();
                            return View("UpdateReson",list);
                        }
                    }
                }
                DbContext.SaveChanges();
            }
            return RedirectToAction("Detail", "Home", new { shipId = model.ShipId, tripId = model.TripId, all = true, currentTrip = true });
        }
        [HttpPost]
        public ActionResult Delete(int id, int key, int shipId, string tripId)
        {
            switch (key)
            {
                case 1: var jn = DbContext.JourneySuppervision.Find(id);
                    DbContext.JourneySuppervision.Remove(jn);
                    break;
                case 2: var pl = DbContext.journeyPlans.Find(id);
                    DbContext.journeyPlans.Remove(pl);
                    break;
                case 3: var note = DbContext.Notes.Find(id);
                    DbContext.Notes.Remove(note);
                    break;
                default:
                    break;
            }
            DbContext.SaveChanges();
            return RedirectToAction("Detail", "Home", new { shipId = shipId, tripId = tripId, all = true, currentTrip = true});
        }
        public ActionResult PModify(int plId)
        {
            JourneyPlan pl = DbContext.journeyPlans.Find(plId);
            ViewBag.availablePlans = DbContext.Plans.ToList();
            ViewBag.availablePorts = DbContext.Ports.ToList();
            ViewBag.availableStates = DbContext.States.ToList();
            return View(pl);
        }
        [HttpPost]
        public ActionResult PModify(JourneyPlan model)
        {
            if (ModelState.IsValid)
            {
                model.Updator = User.Identity.Name;
                model.UpdateTime = DateTime.Now;
                DbContext.Entry(model).State = EntityState.Modified;
                var result = DbContext.Results.FirstOrDefault(p => p.ShipId == model.ShipId && p.TripId == model.TripId && p.PlanId == model.PlanId && p.PortId == model.PortId);
                if (result!=null &&result.JourneyDate!=null)
                {
                    result.PlanDate = model.PlanTime;
                    var days = (result.PlanDate.Value - result.JourneyDate.Value).Days;
                    if (days > 0)
                    {
                        result.Remark = "Sớm hơn " + days + " ngày so với dự kiến";
                        ViewBag.availableResons = repository.GetResons();
                        DbContext.Entry(result).State = EntityState.Modified;
                        DbContext.SaveChanges();
                        return View("UpdateReson", new List<Result> { result });
                    }
                    if (days < 0)
                    {
                        result.Remark = "Trễ hơn " + -days + " ngày so với dự kiến";
                        ViewBag.availableResons = repository.GetResons();
                        DbContext.Entry(result).State = EntityState.Modified;
                        DbContext.SaveChanges();
                        return View("UpdateReson", new List<Result> { result });
                    }
                    if (days == 0)
                    {
                        result.Remark = "Đúng theo dự kiến";
                    }
                   
                }
                DbContext.SaveChanges();
            }
            return RedirectToAction("Detail", "Home", new { shipId = model.ShipId, tripId = model.TripId, all = false, currentTrip = true });
        }
        public ActionResult NModify(int noteid)
        {
            var note = DbContext.Notes.Find(noteid);
            return View(note);
        }
        [HttpPost]
        public ActionResult NModify(Note model)
        {
           if (ModelState.IsValid)
            {
                model.Updator = User.Identity.Name;
                model.UpdateTime = DateTime.Now;
                model.CreatedDate = DateTime.Now;
                DbContext.Entry(model).State = EntityState.Modified;
                DbContext.SaveChanges();
            }
            return RedirectToAction("Detail", "Home", new { shipId = model.ShipId, tripId = model.TripId, all = true, currentTrip = true });
        }
        public ActionResult NewPort(int shipId, string tripId)
        {
            ViewBag.shipId = shipId;
            ViewBag.tripId = tripId;
            ViewBag.PossibleNations = DbContext.Nations;
            return View();
        }
        [HttpPost]
        public ActionResult NewPort(Port port, int shipId, string tripId)
        {
            if (port.PortName!=string.Empty)
            {
                DbContext.Ports.Add(port);
                DbContext.SaveChanges();
                return RedirectToAction("Index", new {shipId = shipId, tripId= tripId, IsRefresh= false });
            }
            ViewBag.shipId = shipId;
            ViewBag.tripId = tripId;
            ViewBag.PossibleNations = DbContext.Nations;
            return View(port);
        }
        public ActionResult NewCategory(int shipId, string tripId)
        {
            ViewBag.shipId = shipId;
            ViewBag.tripId = tripId;
            return View();
        }
        [HttpPost]
        public ActionResult NewCategory(Category cate, int shipId, string tripId)
        {
            if (cate.CategoryName!=string.Empty)
            {
                DbContext.Categories.Add(cate);
                DbContext.SaveChanges();
                return RedirectToAction("Index", new { shipId = shipId, tripId = tripId, IsRefresh = false });
            }
            ViewBag.shipId = shipId;
            ViewBag.tripId = tripId;
            return View(cate);
        }
      
      
      
    }
}

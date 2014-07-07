using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pii.Areas.CargoShip.ViewModel;
using Pii.Areas.CargoShip.Models;
using Pii.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;

namespace Pii.Areas.CargoShip.Controllers
{
    [Authorize]
    public class HomeController : Pii.Controllers.BaseController
    {
        CargoShipRepository repository = new CargoShipRepository();
        PiiContext db = new PiiContext();
        // GET: /CargoShip/Home/

        public ActionResult Index()
        {
            //var list = db.JourneySuppervision.Where(p=>p.UpdateTime!=null).ToList();
            ViewBag.pickedDate = DateTime.Today.ToString("dd/MM/yyyy");
            return View(repository.CreateHomeViewModel(string.Empty));
        }
        [HttpPost]
        public ActionResult Index(string pickedDate)
        {
            ViewBag.pickedDate = pickedDate;
            return View(repository.CreateHomeViewModel(pickedDate));
        }

        public ActionResult Detail(int shipId, string tripId, bool all, bool currentTrip)
        {
            ViewBag.currentTrip = currentTrip;
            return View(repository.CreateDetailViewModel(shipId, tripId, all));
        }
        [HttpPost]
        public ActionResult Detail(int shipId, string tripId, bool all, bool currentTrip, string starttime, string stoptime)
        {
            ViewBag.currentTrip = currentTrip;
            try
            {
                ViewBag.fromDate = DateTime.ParseExact(starttime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
            }
            try
            {
                ViewBag.toDate = DateTime.ParseExact(stoptime, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
            }
            return View(repository.CreateDetailViewModel(shipId, tripId, all));
        }

        public JsonResult ChangeTrip(string selectedTripId, int shipId)
        {
            var nation = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == selectedTripId);
            var result = new
            {
                startNation = nation.StartNationName,
                stopNation = nation.StopNationName,
                startTime = nation.StartDate.HasValue ? nation.StartDate.Value.ToString("dd/MM/yyyy") : "",
                tripId = selectedTripId
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RouteDetailFilter(DetailViewModel model, string tripId, string starttime, string stoptime)
        {
            var result = repository.GetJourneySupervisionById(model, tripId, starttime, stoptime);
            var times = repository.GetNoteByTime(model.Ship.ShipId, tripId, starttime, stoptime);
            var plans = repository.GetJourneyPlanByTime(model.Ship.ShipId, tripId, starttime, stoptime);
            var b = new List<object>();
            var ttime = new List<object>();
            var tplan = new List<object>();
            foreach (var item in result)
            {
                var a = new
                {
                    TaskName = item.Task != null ? item.Task.TaskName : "",
                    ArrangedWeight = item.ArrangedWeight,
                    Date = item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("dd/MM/yyyy") : "",
                    PortName = item.Port != null ? item.Port.PortName : "",
                    Position = item.Position ?? "",
                    RestWeight = item.RestWeight,
                    CategoryName = item.CategoryName ?? "",
                    StateName = item.State != null ? item.State.StateName : "",
                    Time = item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("HH:mm") : "",
                    id = item.Id,
                    nationName = item.NationName,
                    result = (item.StateId == ConstId.LoadingStateId || item.StateId == ConstId.LoadedStateId || item.StateId == ConstId.UnloadedStateId || item.StateId == ConstId.UnloadingStateId || item.StateId == ConstId.RefueledId) ? "Làm:" + item.ArrangedWeight + "      <span>Còn:" + (item.RestWeight) + "<span>" : ""
                };
                b.Add(a);
            }
            foreach (var item in times)
            {
                var a = new { createdDate = item.UpdateTime.HasValue ? item.UpdateTime.Value.ToString("dd/MM/yyyy") : "", note = item.NoteContent ?? "" };
                if (a.createdDate != "" || a.note != "")
                {
                    ttime.Add(a);
                }
            }
            foreach (var item in plans)
            {
                string updateResult = "";
                string updateReson = "";
                string journeytime = "";
                var end = db.Results.FirstOrDefault(p => p.TripId == item.TripId && p.ShipId == item.ShipId && p.PlanId == item.PlanId);
                if (end != null)
                {
                    updateResult = end.Remark;
                    updateReson = end.Reson == null ? "" : end.Reson.ResonName;
                    journeytime = end.JourneyDate.HasValue ? end.JourneyDate.Value.ToString("dd/MM/yyyy") : "";
                }
                var a = new
                {
                    planId = item.PlanId ?? "",
                    portName = item.Port == null ? "" : item.Port.PortName,
                    createdDate = item.PlanTime.HasValue ? item.PlanTime.Value.ToString("dd/MM/yyyy") : "",
                    planName = item.Plan.PlanName,
                    ssresult = updateResult,
                    reson = updateReson,
                    journeyTime = journeytime,
                    id = item.JourneyPlanId
                };
                if (a.createdDate != "" || a.planId != "" || a.portName != "")
                {
                    tplan.Add(a);
                }
            }
            return Json(new { ar1 = b, ar2 = ttime, ar3 = tplan });
        }
        public ActionResult Graph(int shipId, string tripId)
        {
            var result = repository.GetGraph(shipId, tripId);
            ViewBag.shipName = repository.GetShipName(shipId);
            ViewBag.tripId = tripId;
            return View(result);
        }
        public ActionResult HomeRoute(string pickedDate, int? shipId)
        {
            ViewBag.ship = db.Ships.Find(shipId) != null ? db.Ships.Find(shipId).ShipName : db.Ships.Where(p => p.State).First().ShipName;
            //ViewBag.homeRoute = repository.SetHomeRoute(date, shipId);
            var list = db.JourneySuppervision.Where(p => p.UpdateTime != null).ToList();
            ViewBag.dates = list.GroupBy(p => p.UpdateTime.Value.Date).Select(p => p.FirstOrDefault().UpdateTime.Value.Date.ToString("dd/MM/yyyy")).ToList();
            ViewBag.pickedDate = pickedDate ?? DateTime.Today.ToString("dd/MM/yyyy");
            return View("Index", repository.CreateHomeViewModel(pickedDate));
        }
        public ActionResult Report(int shipId, string tripId)
        {
            var model = new ReportViewModel();
            model.OnLoadReport = repository.GetLoadingReport(shipId, tripId, false);
            model.UnloadReport = repository.GetLoadingReport(shipId, tripId, true);
            model.UnsualReport = repository.GetUnusualReport(shipId, tripId);
            ViewBag.shipId = shipId;
            ViewBag.tripId = tripId;
            return View(model);
        }
        [HttpPost]
        public ActionResult ExportReport(ReportViewModel report, int reportType, int shipId, string tripId)
        {
            string path = repository.WriteToExcel(report, reportType);
            var fileStream = new FileStream(path, FileMode.Open);
            var mimeType = "application/xls";
            var fileDownloadName = path.Substring(path.LastIndexOf(@"\") + 1);
            return File(fileStream, mimeType, fileDownloadName);
            //return RedirectToAction("Report", new { shipId = shipId, tripId = tripId });
        }
        //public ActionResult Download()
        //{
        //    var fileStream = new FileStream(@"c:\PATH\TO\PDF\ON\THE\SERVER.pdf", FileMode.Open);
        //    var mimeType = "application/pdf";
        //    var fileDownloadName = "download.pdf";
        //    return File(fileStream, mimeType, fileDownloadName);
        //}

    }
}

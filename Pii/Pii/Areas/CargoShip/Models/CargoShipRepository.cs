using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pii.Areas.CargoShip.Models;
using Pii.Areas.CargoShip.ViewModel;
using System.Data;
using Pii.Models;
using System.Web.Mvc;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;



namespace Pii.Areas.CargoShip.Models
{
    public class CargoShipRepository : Controller
    {
        Pii.Models.PiiContext db = new Pii.Models.PiiContext();
        public int CreateJourney(int ShipId, string TripId, string ContractNumber, int StartPortId, DateTime StartedDate, Single Weight, DateTime FinishedDate)
        {
            try
            {
                Journey journey = new Journey { ContractNumber = ContractNumber, EndDate = FinishedDate, ShipId = ShipId, StartPortId = StartPortId, StartDate = StartedDate, Weight = Weight, TripId = TripId };
                db.Journeys.Add(journey);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public List<HomeViewModel> GetHomeViewModelByTime(string start, string stop)
        {

            DateTime startTime = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime stopTime = string.IsNullOrEmpty(stop) ? DateTime.Now : DateTime.ParseExact(stop, "dd/MM/yyyy", null);
            var list = db.Journeys.Where(p => (p.StartDate >= startTime && (p.StartDate <= stopTime || p.EndDate == null)) || (p.StartDate <= startTime && (p.EndDate >= startTime || p.EndDate == null))).ToList();
            var items = new List<HomeViewModel>();
            ICollection<Ship> ships = ships = db.Ships.ToList();
            foreach (var item in list)
            {
                var name = ships.FirstOrDefault(p => p.ShipId == item.ShipId).ShipName;
                var a = new HomeViewModel { ShipName = name, StartDate = item.StartDate, TripId = item.TripId, ShipId = item.ShipId, FromTo = item.StartNationName + " / " + item.StopNationName };
                items.Add(a);
            }
            return items;
        }

        public List<HomeViewModel> CreateHomeViewModel(string pickedDate)
        {
            var items = new List<HomeViewModel>();
            ICollection<Ship> ships = ships = db.Ships.ToList();
            var list = db.Journeys.Where(p => p.Ship.State)
                .GroupBy(p => p.ShipId).ToList().Select(m => new { journey = m.OrderByDescending(g => g.JourneyId).FirstOrDefault() });
            if (pickedDate != null && pickedDate != string.Empty)
            {
                DateTime pickedTimecv = DateTime.ParseExact(pickedDate, "dd/MM/yyyy", null);
                foreach (var item in list)
                {
                    try
                    {
                        string pickedDateS = pickedTimecv.Date.ToString();
                        var jns = db.JourneySuppervision.ToList().OrderByDescending(p=>p.Id)
                            .FirstOrDefault(p => p.ShipId == item.journey.ShipId && p.Date==pickedDateS);
                        var a = new HomeViewModel
                        {
                            ShipName = item.journey.Ship.ShipName,
                            StartDate = item.journey.StartDate,
                            TripId = item.journey.TripId,
                            ShipId = item.journey.ShipId,
                            FromTo = item.journey.StartNationName + " / " + item.journey.StopNationName,
                            CurPosAndState = jns==null?"": jns.StateId != null ? (jns.Position + " | " + jns.State.StateName) : (jns.Position + " | ")
                        };
                        items.Add(a);
                    }
                    catch (Exception)
                    {
                    }
                }
                return items.OrderBy(p=>p.ShipName).ToList();
            }
            else
            {
                foreach (var item in list)
                {
                    try
                    {
                        var jns = db.JourneySuppervision.OrderByDescending(p => p.Id).FirstOrDefault(p => p.ShipId == item.journey.ShipId && p.TripId == item.journey.TripId);
                        var a = new HomeViewModel
                        {
                            ShipName = item.journey.Ship.ShipName,
                            StartDate = item.journey.StartDate,
                            TripId = item.journey.TripId,
                            ShipId = item.journey.ShipId,
                            FromTo = item.journey.StartNationName + " / " + item.journey.StopNationName,
                            CurPosAndState = jns == null ? "" : jns.StateId != null ? (jns.Position + " | " + jns.State.StateName) : (jns.Position + " | ")
                        };
                        items.Add(a);
                    }
                    catch (Exception)
                    {
                    }
                }
                return items.OrderBy(p => p.ShipName).ToList();
            }

            //var list = db.Journeys.Join(db.JourneySuppervision, p => p.ShipId, m => m.ShipId, (m, p) => p);
            //foreach (var item in list)
            //{

            //}
        }

        public DetailViewModel CreateDetailViewModel(int shipId, string tripId, bool all)
        {
            var model = new DetailViewModel();
            model.Ship = db.Ships.Find(shipId);
            model.Journeys = db.Journeys.Where(p => p.ShipId == shipId).ToList();
            model.JourneySupervisions = db.JourneySuppervision.Where(p => p.ShipId == shipId && p.TripId == tripId).OrderByDescending(p => p.Id).ToList();
            model.Notes = db.Notes.Where(p => p.ShipId == shipId && p.TripId == tripId).OrderByDescending(p => p.NoteId).ToList();
            model.JourneyPlans = db.journeyPlans.Where(p => p.ShipId == shipId && p.TripId == tripId).OrderByDescending(p => p.JourneyPlanId).ToList();
            model.SelectedTripId = tripId;
            model.Ports = db.Ports.OrderBy(p => p.PortName).ToList();
            model.Tasks = db.Tasks.OrderBy(p => p.TaskName).ToList();
            model.States = db.States.OrderBy(p => p.StateName).ToList();
            model.Results = db.Results.Where(p => p.TripId == tripId && p.ShipId == shipId).ToList();
            foreach (var item in model.JourneySupervisions)
            {
                model.CreatedDates.Add(item.CreatedDate);
            }
            model.Ports.RemoveAll(p => p == null);
            model.Tasks.RemoveAll(p => p == null);
            model.CreatedDates.RemoveAll(p => p == null);
            model.States.RemoveAll(p => p == null);

            if (!all)
            {
                model.JourneySupervisions = model.JourneySupervisions.OrderByDescending(p => p.CreatedDate).Take(10).ToList();
            }
            return model;
        }
        
      
        public List<JourneySupervision> GetJourneySupervisionById(DetailViewModel model, string tripId, string start, string stop)
        {
            DateTime startTime = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime stopTime = string.IsNullOrEmpty(stop) ? DateTime.Now : DateTime.ParseExact(stop, "dd/MM/yyyy", null);
            var result = new List<JourneySupervision>();
            var temp = db.JourneySuppervision.Where(p => p.ShipId == model.Ship.ShipId && p.TripId == tripId &&
                (model.SelectedStateId == null || p.StateId == model.SelectedStateId) && (model.SelectedTaskId == null || p.TaskId == model.SelectedTaskId) &&
                (model.SelectedPortId == null || p.PortId == model.SelectedPortId) && (p.CreatedDate >= startTime && p.CreatedDate <= stopTime)).OrderByDescending(p => p.Id).ToList();
            if (model.SelectedTime != null && temp != null)
            {
                foreach (var item in temp)
                {
                    if (item.CreatedDate.Value.Date == model.SelectedTime.Value.Date)
                    {
                        result.Add(item);
                    }
                }
            }
            else { result = temp; }
            return result;
        }
        public List<Note> GetNoteByTime(int shipId, string tripId, string start, string stop)
        {
            DateTime startTime = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime stopTime = string.IsNullOrEmpty(stop) ? DateTime.Now : DateTime.ParseExact(stop, "dd/MM/yyyy", null);
            var result = db.Notes.Where(p => (p.CreatedDate >= startTime && p.CreatedDate <= stopTime) && p.ShipId == shipId && p.TripId == tripId).OrderByDescending(p => p.NoteId).ToList();
            return result;
        }
        public List<JourneyPlan> GetJourneyPlanByTime(int shipId, string tripId, string start, string stop)
        {
            DateTime startTime = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime stopTime = string.IsNullOrEmpty(stop) ? DateTime.Now : DateTime.ParseExact(stop, "dd/MM/yyyy", null);
            var result = db.journeyPlans.Where(p => p.PlanTime >= startTime && p.PlanTime <= stopTime && p.ShipId == shipId && p.TripId == tripId).OrderByDescending(p => p.JourneyPlanId).ToList();
            return result;
        }

        public void InitialUpdateViewModel(UpdateViewModel UViewModel)
        {
            UViewModel.AvailableCategories = db.Categories.OrderBy(p => p.CategoryName).ToList();
            UViewModel.AvailableNations = db.Nations.OrderBy(p => p.NationName).ToList();
            UViewModel.AvailablePorts = db.Ports.OrderBy(p => p.PortName).ToList();
            UViewModel.AvailableStates = db.States.OrderBy(p => p.StateName).ToList();
            UViewModel.AvailablePlans = db.Plans.OrderBy(p => p.PlanName).ToList();
            UViewModel.InsertOrUpdate = 1;
            UViewModel.AvailableTripIds = new List<string>();
        }
        public void SetUpdateViewModelShip(UpdateViewModel model, int shipId, string tripId)
        {
            model.CurrentStartTime = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId).StartDate;

            model.AvailableTripIds.Add(tripId);
            model.Ship = db.Ships.Find(shipId);
            model.CurrentTripId = tripId;
            model.LoadGoodsJourney = new JourneySupervision() { ShipId = shipId, TripId = tripId, TaskId = 1 };
            model.UnloadGoodsJourney = new JourneySupervision() { ShipId = shipId, TripId = tripId, TaskId = 2 };
            model.JourneyInProgress = new JourneySupervision() { ShipId = shipId, TripId = tripId, TaskId = 3 };
            model.UnusualJourney = new JourneySupervision() { ShipId = shipId, TripId = tripId, TaskId = 4 };
            model.Note = new Note() { ShipId = shipId, TripId = tripId };
            model.JourneyPlan = new JourneyPlan() { ShipId = shipId, TripId = tripId };
            var journey = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId);
            if (journey.ContractNumber != null)
            {
                model.InsertOrUpdate = 2;
                model.StartNationId = journey.StartNationName != null ? db.Nations.FirstOrDefault(p => p.NationName == journey.StartNationName).NationId : null;
                model.StopNationId = journey.StopNationName != null ? db.Nations.FirstOrDefault(p => p.NationName == journey.StopNationName).NationId : null;
                model.ContractNumber = journey.ContractNumber;
                var shipment = db.Shipments.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId && p.ContractNumber == journey.ContractNumber);
                var log = db.UpdateLogs.Where(p => p.ShipId == shipId && p.TripId == tripId).ToList();
                model.Logs = log;
            }
            else
            {
                model.InsertOrUpdate = 1;
            }
        }

        public string GenerateTripId(string tripId)
        {
            int a = int.Parse(tripId.Substring(1, 2));
            a++;
            string result = a < 10 ? "V0" + a + "/" + DateTime.Now.Year.ToString().Substring(2, 2) : "V" + a + "/" + DateTime.Now.Year.ToString().Substring(2, 2);
            return result;
        }
        public void UpdateJourneyStopDate(int shipId, string tripId)
        {
            var result = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId);
            result.EndDate = DateTime.Now;
        }
        public void GetJourney(UpdateViewModel model)
        {
            var journey = db.Journeys.FirstOrDefault(p => p.TripId == model.CurrentTripId && p.ShipId == model.Ship.ShipId);
        }

        public void UpdateGoodsAndJourney(UpdateViewModel model)
        {
            if (model.InsertOrUpdate == 1)
            {
                Single totalWeight = 0;
                for (int i = 0; i < model.CategoryIds.Count; i++)
                {
                    if (model.CategoryIds[i] != null || model.Weights[i] != null || model.StartPortIds[i] != null || model.StopPortIds[i] != null)
                    {
                        var a = new GoodsMakingPlace { ContractNumber = model.ContractNumber, PortId = model.StartPortIds[i] == null ? model.StartPortIds[0] : model.StartPortIds[i], Kind = false, NationId = model.StartNationId };
                        var b = new GoodsMakingPlace { ContractNumber = model.ContractNumber, PortId = model.StopPortIds[i] == null ? model.StopPortIds[0] : model.StopPortIds[i], Kind = true, NationId = model.StopNationId };
                        if (model.Weights[i] != null || model.CategoryIds[i] != null)
                        {
                            var shipment = new Shipment { CategoryId = model.CategoryIds[i], ShipId = model.Ship.ShipId, ContractNumber = model.ContractNumber, TripId = model.CurrentTripId, Weight = model.Weights[i] ?? 0 };
                            db.Shipments.Add(shipment);
                        }
                        totalWeight += model.Weights[i] != null ? model.Weights[i].Value : 0;
                        try
                        {
                            db.GoodMakingPlaces.Add(a);
                            db.GoodMakingPlaces.Add(b);
                        }
                        catch (Exception)
                        {
                        }
                        var log = new UpdateLog
                        {
                            ContractNumber = model.ContractNumber,
                            TripId = model.CurrentTripId,
                            ShipId = model.Ship.ShipId,
                            Category = db.Categories.Find(model.CategoryIds[i]) == null ? "" : db.Categories.Find(model.CategoryIds[i]).CategoryName,
                            StartPort = db.Ports.Find(model.StartPortIds[i]) == null ? "" : db.Ports.Find(model.StartPortIds[i]).PortName,
                            StopPort = db.Ports.Find(model.StopPortIds[i]) == null ? "" : db.Ports.Find(model.StopPortIds[i]).PortName,
                            Weight = model.Weights[i].ToString()
                        };
                        db.UpdateLogs.Add(log);
                    }
                }
                var jouney = db.Journeys.FirstOrDefault(p => p.ShipId == model.Ship.ShipId && p.TripId == model.CurrentTripId);
                jouney.ContractNumber = model.ContractNumber;
                jouney.Weight += totalWeight;
                jouney.StartNationName = model.StartNationId != null ? db.Nations.Find(model.StartNationId).NationName : null;
                jouney.StopNationName = model.StopNationId == null ? null : db.Nations.Find(model.StopNationId).NationName;
                db.Entry(jouney).State = System.Data.EntityState.Modified;

            }
            //var jouney2 = db.Journeys.FirstOrDefault(p => p.ShipId == model.Ship.ShipId && p.TripId == model.CurrentTripId);
            //jouney2.StartNationName = db.Nations.Find(model.StartNationId).NationName;
            //jouney2.StopNationName = db.Nations.Find(model.StopNationId).NationName;
            //db.Entry(jouney2).State = System.Data.EntityState.Modified;
        }

        private int? GetPreviousPortId(string tripId, int shipId)
        {
            var a = db.JourneySuppervision.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId && p.Task.TaskName == "Dỡ hàng");
            if (a != null)
            {
                return a.PortId.Value;
            }
            return null;
        }
        public void UpdateModels(UpdateViewModel model, string userName)
        {
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.LoadGoodsJourney, 0, userName);
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.JourneyInProgress, 0, userName);
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.UnloadGoodsJourney, 0, userName);
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.UnusualJourney, 0, userName);
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.JourneyPlan, 1, userName);
            UpdateModels(model, model.CurrentTripId, model.Ship.ShipId, model.Note, 2, userName);
        }
        //public Single SetRestWeight(int shipId, string tripId, int taskId, int? stateId, Single arrangedWeight)
        //{
        //    var jns = db.Journeys.FirstOrDefault(p => p.TripId == tripId && p.ShipId == shipId);
        //    Single result = (jns.Weight - arrangedWeight);
        //    return result;
        //}

        private void UpdateModels(UpdateViewModel upv, string tripId, int shipId, dynamic model, int kind, string userName)
        {
            model.ShipId = shipId;
            model.TripId = tripId;
            switch (kind)
            {
                case 0:
                    int taskId = model.TaskId;
                    int? stateId = model.StateId;
                    int? portId = model.PortId;
                    //if ((stateId == ConstId.LoadedStateId || stateId == ConstId.UnloadedStateId || stateId == ConstId.UnloadingStateId || stateId == ConstId.LoadingStateId) && (taskId == ConstId.LoadTaskId || taskId == ConstId.UnloadTaskId))
                    //{
                    //    model.RestWeight = SetRestWeight(shipId, tripId, taskId, stateId, model.ArrangedWeight);
                    //}
                    //JourneySupervision a = db.JourneySuppervision.Where(p => p.TripId == tripId && p.ShipId == shipId && p.TaskId == taskId && p.PortId == portId && p.StateId == stateId).FirstOrDefault();
                    //if (a == null)
                    //{
                    if (model.PortId != null || model.CreatedDate != null || model.Position != null || model.ArrangedWeight != 0)
                    {
                        var js = new JourneySupervision
                        {
                            ShipId = shipId,
                            TripId = tripId,
                            StateId = stateId,
                            PortId = model.PortId,
                            ArrangedWeight = model.ArrangedWeight,
                            CreatedDate = model.CreatedDate,
                            RestWeight = model.RestWeight,
                            Position = model.Position,
                            TaskId = taskId,
                            CategoryName = model.CategoryName,
                            Updator = userName,
                            UpdateTime = DateTime.Now,
                            NationName = model.NationName
                        };
                        db.JourneySuppervision.Add(js);
                    }
                    //}
                    //else
                    //{
                    //    if (model.StateId == ConstId.LoadingStateId || model.StateId == ConstId.UnloadingStateId)
                    //    {
                    //        var js = new JourneySupervision
                    //        {
                    //            ShipId = shipId,
                    //            TripId = tripId,
                    //            StateId = stateId,
                    //            PortId = model.PortId,
                    //            ArrangedWeight = model.ArrangedWeight,
                    //            CreatedDate = model.CreatedDate,
                    //            RestWeight = model.RestWeight,
                    //            Position = model.Position,
                    //            TaskId = taskId,
                    //            CategoryName = model.CategoryName,
                    //            Updator = userName,
                    //            UpdateTime = DateTime.Now
                    //        };
                    //        db.JourneySuppervision.Add(js);
                    //    }
                    //    else
                    //    {
                    //        a.ArrangedWeight = model.ArrangedWeight ?? 0;
                    //        a.RestWeight = model.RestWeight ?? 0;
                    //        a.CreatedDate = model.CreatedDate;
                    //        a.Note = model.Note;
                    //        a.PortId = model.PortId;
                    //        a.Position = model.Position;
                    //        a.StateId = stateId;
                    //        a.CategoryName = model.CategoryName;
                    //        a.UpdateTime = DateTime.Now;
                    //        a.Updator = userName;
                    //        db.Entry(a).State = EntityState.Modified;
                    //    }
                    //}
                    if (model.StateId != null)
                    {
                        var plan = db.Compares.FirstOrDefault(p => p.StateId == stateId);
                        if (plan != null)
                        {
                            string plid = plan.PlanId;

                            var jourpl = db.journeyPlans.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId && p.PlanId == plid && p.PortId == portId);
                            if (jourpl != null)
                            {
                                var plTime = jourpl.PlanTime;
                                DateTime? jnTime = model.CreatedDate;
                                if (plTime != null && jnTime != null)
                                {
                                    var rs1 = db.Results.Where(p => p.TripId == tripId && p.ShipId == shipId && p.PlanId == plid && p.PortId == portId);
                                    if (rs1 != null)
                                    {
                                        foreach (var item in rs1)
                                        {
                                            item.StateId = stateId;
                                            item.JourneyDate = jnTime;
                                            var days = (item.PlanDate.Value - jnTime.Value).Days;
                                            if (days > 0)
                                            {
                                                upv.IsLate = 1;
                                                item.Remark = "Sớm hơn " + days + " ngày so với dự kiến";
                                            }
                                            if (days < 0)
                                            {
                                                item.Remark = "Trễ hơn " + -days + " ngày so với dự kiến";
                                                upv.IsLate = 2;
                                            }
                                            if (days == 0)
                                            {
                                                upv.IsLate = 0;
                                                item.Remark = "Đúng theo dự kiến";
                                            }
                                        }
                                        upv.rStateId = stateId;
                                        upv.rPlanId = plid;
                                        upv.rShipId = shipId;
                                    }
                                }
                            }
                        }

                    }
                    string ctripId = GenerateTripId(tripId);
                    var jn = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == ctripId);
                    if (model.TaskId == ConstId.UnloadTaskId && model.StateId == ConstId.UnloadedStateId && jn == null)
                    {
                        var now = model.CreatedDate??DateTime.Now;
                        var updateJourney = db.Journeys.FirstOrDefault(p => p.ShipId == shipId && p.TripId == tripId);
                        updateJourney.EndDate = now;
                       // db.Entry(updateJourney).State = System.Data.EntityState.Modified;
                       // db.SaveChanges();
                        var journey = new Journey { ShipId = model.ShipId, TripId = ctripId, StartPortId = model.PortId, StartDate = now };
                        db.Journeys.Add(journey);
                    }
                    db.SaveChanges();

                    break;
                case 1:
                    string planId = model.PlanId;
                    //var b = db.journeyPlans.FirstOrDefault(p => p.TripId == tripId && p.ShipId == shipId && p.PlanId == planId);
                    //if (b != null)
                    //{
                    //    b.PlanTime = model.PlanTime;
                    //    b.PortId = model.PortId;
                    //    b.UpdateTime = DateTime.Now;
                    //    b.Updator = userName;
                    //}
                    //else
                    //{
                    if (model.PlanId != null || model.PlanTime != null && model.PortId)
                    {
                        var jp = new JourneyPlan
                        {
                            ShipId = shipId,
                            TripId = tripId,
                            PlanId = planId,
                            PlanTime = model.PlanTime,
                            PortId = model.PortId,
                            Updator = userName,
                            UpdateTime = DateTime.Now
                        };
                        var rs = new Result { PlanDate = model.PlanTime, PlanId = planId, ShipId = shipId, TripId = tripId, PortId = model.PortId };
                        db.journeyPlans.Add(jp);
                        db.Results.Add(rs);
                    }
                    //}
                    db.SaveChanges();
                    break;
                case 2:
                    bool isnull = string.IsNullOrEmpty(model.NoteContent);
                    if (!isnull)
                    {
                        var note = new Note { CreatedDate = model.CreatedDate, ShipId = shipId, TripId = tripId, NoteContent = model.NoteContent, Updator = userName, UpdateTime = DateTime.Now };
                        db.Notes.Add(note);
                        db.SaveChanges();
                    }
                    break;
                default:
                    break;
            }
        }
        public ICollection<JourneySupervision> GetGraph(int shipId, string tripId)
        {
            var result = db.JourneySuppervision.Where(p => p.ShipId == shipId && p.TripId == tripId).ToList();
            return result;
        }
        public string GetShipName(int shipId)
        {
            return db.Ships.Find(shipId).ShipName;
        }
        public ICollection<Result> GetResult(UpdateViewModel model)
        {
            var result = db.Results.Where(p => p.ShipId == model.rShipId && p.StateId == model.rStateId && p.TripId == model.CurrentTripId && p.PlanId == model.rPlanId).ToList();
            return result;
        }
        public List<Reson> GetResons()
        {
            return db.Resons.ToList();
        }
        public void UpdateResult(IEnumerable<Result> model)
        {
            //var result = db.Results.FirstOrDefault(p => p.ShipId == model.ShipId && p.StateId == model.StateId && p.TripId == model.TripId && p.PlanId == model.PlanId);
            //result.ResonId = model.ResonId;
            foreach (var item in model)
            {
                var rs = db.Results.Find(item.ResultId);
                rs.ResonId = item.ResonId;
            }
            SaveChange();
        }
        public RouteModel SetHomeRoute(string date, int? shipId)
        {
            var model = new RouteModel();
            DateTime time;
            try
            {
                time = DateTime.ParseExact(date, "dd/MM/yyyy", null);
            }
            catch (Exception)
            {
                time = DateTime.Now;
            }
            int ShipId;

            if (shipId == null)
            {
                ShipId = db.Ships.Where(p => p.State).First().ShipId;

            }
            else ShipId = shipId.Value;
            foreach (var item in db.JourneySuppervision.Where(p => p.ShipId == ShipId).ToList())
            {
                if (item.UpdateTime.HasValue && item.UpdateTime.Value.Date == time.Date)
                {
                    model.JourneySupervisions.Add(item);
                }
            }
            foreach (var item in db.journeyPlans.Where(p => p.ShipId == ShipId).ToList())
            {
                if (item.UpdateTime.HasValue && item.UpdateTime.Value.Date == time.Date)
                {
                    model.JourneyPlans.Add(item);
                }
            }
            foreach (var item in db.Notes.Where(p => p.ShipId == ShipId).ToList())
            {
                if (item.UpdateTime.HasValue && item.UpdateTime.Value.Date == time.Date)
                {
                    model.Notes.Add(item);
                }
            }
            model.Results = db.Results.ToList();
            return model;
        }
        public IEnumerable<Result> UpdateResult(int shipId, string tripId, int? stateId, int? portId)
        {
            var result = db.Results.Where(p => p.ShipId == shipId && p.TripId == tripId && p.StateId == stateId && p.PortId == portId);
            return result;
        }

        public LoadingReport GetLoadingReport(int shipId, string tripId, bool UnLoad)
        {
            var result = new LoadingReport();
            var temp = new JourneySupervision();
            var list = new List<JourneySupervision>();
            if (!UnLoad)
            {
                list = db.JourneySuppervision.Where(p => p.ShipId == shipId && p.TripId == tripId && p.TaskId == 1).ToList();
            }
            else
            {
                list = db.JourneySuppervision.Where(p => p.ShipId == shipId && p.TripId == tripId && p.TaskId == 2).ToList();

            }
            temp = list.FirstOrDefault(p => p.StateId == 1);
            if (temp != null && temp.CreatedDate.HasValue)
            {
                result.PortComeDate = temp.CreatedDate.Value;
                result.Goods = temp.CategoryName;
                result.PortName = temp.Port.PortName;
            }
            temp = list.FirstOrDefault(p => p.StateId == 9);
            if (temp != null && temp.CreatedDate.HasValue)
            {
                result.PortComeTime = temp.CreatedDate.Value;
            }
            temp = list.FirstOrDefault(p => p.StateId == 4);
            if (temp != null && temp.CreatedDate.HasValue)
            {
                result.LoadingTime = temp.CreatedDate.Value;
            }
            temp = list.FirstOrDefault(p => p.StateId == 11);
            if (temp != null && temp.CreatedDate.HasValue)
            {
                result.LoadingComplTime = temp.CreatedDate.Value;
            }
            temp = list.FirstOrDefault(p => p.StateId == 10);
            if (temp != null && temp.CreatedDate.HasValue)
            {
                result.LeavingTime = temp.CreatedDate.Value;
            }
            return result;
        }
        public UnusualReport GetUnusualReport(int shipId, string tripId)
        {
            var result = new UnusualReport();
            var list = db.JourneySuppervision.Where(p => p.ShipId == shipId && p.TripId == tripId && p.StateId == 4).OrderByDescending(p => p.Id);
            var temp = list.FirstOrDefault(p => p.StateId == 1);
            if (temp != null)
            {
                result.ComingDate = temp.CreatedDate.Value;
                result.PortName = temp.Port.PortName;
            }
            return result;
        }


        public string WriteToExcel(ReportViewModel report, int reportType)
        {
            string fileName = "";
            Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            switch (reportType)
            {
                case 1:
                    xlWorkSheet.Cells[1, 1] = "CÔNG TY CỔ PHẦN VẬN TẢI VÀ THUÊ TÀU BIỂN VIỆT NAM";
                    xlWorkSheet.Cells[3, 1] = "Tên tàu";
                    xlWorkSheet.Cells[5, 2] = "TÓM LƯỢC TÌNH HÌNH CẢNG XẾP";
                    xlWorkSheet.Cells[7, 1] = "Cảng:";
                    xlWorkSheet.Cells[8, 1] = "Agent/Đại lý:";
                    xlWorkSheet.Cells[9, 1] = "Hàng hóa:";
                    xlWorkSheet.Cells[10, 1] = "Mớn tàu đến:";
                    xlWorkSheet.Cells[11, 1] = "Thời gian tàu đến:";
                    xlWorkSheet.Cells[12, 1] = "Thời gian cập cầu:";
                    xlWorkSheet.Cells[13, 1] = "Thời gian bắt đầu xếp hàng:";
                    xlWorkSheet.Cells[14, 1] = "Thời gian hoàn thành xếp hàng:";
                    xlWorkSheet.Cells[15, 1] = "Thời gian tàu rời cảng:";
                    xlWorkSheet.Cells[16, 1] = "Mớn tàu rời đi";
                    xlWorkSheet.Cells[18, 1] = "Ghi chú: Các thông tin cập nhật dựa trên các báo cáo từ tàu/ đại lý … có thể mang tính đơn phương, dự đoán chưa được công nhận bởi các bên liên quan. Để biết thêm thông tin vui lòng liên hệ P.KTTV để có được Statement of Fleets của tàu tại các cảng có chữ ký/ được công nhận bởi các bên liên quan";
                    xlWorkSheet.Cells[7, 2] = report.OnLoadReport.PortName;
                    xlWorkSheet.Cells[9, 2] = report.OnLoadReport.Goods;
                    xlWorkSheet.Cells[10, 2] = report.OnLoadReport.ComingCube;
                    xlWorkSheet.Cells[11, 2] = report.OnLoadReport.PortComeDate;
                    xlWorkSheet.Cells[12, 2] = report.OnLoadReport.PortComeTime;
                    xlWorkSheet.Cells[13, 2] = report.OnLoadReport.LoadingTime;
                    xlWorkSheet.Cells[14, 2] = report.OnLoadReport.LoadingComplTime;
                    xlWorkSheet.Cells[15, 2] = report.OnLoadReport.LeavingTime;
                    xlWorkSheet.Cells[16, 2] = report.OnLoadReport.LeavingCube;
                    fileName = " Xep_hang_";
                    break;
                case 2:
                    xlWorkSheet.Cells[1, 1] = "CÔNG TY CỔ PHẦN VẬN TẢI VÀ THUÊ TÀU BIỂN VIỆT NAM";
                    xlWorkSheet.Cells[3, 1] = "Tên tàu";
                    xlWorkSheet.Cells[5, 2] = "TÓM LƯỢC TÌNH HÌNH CẢNG XẾP";
                    xlWorkSheet.Cells[7, 1] = "Cảng:";
                    xlWorkSheet.Cells[8, 1] = "Agent/Đại lý:";
                    xlWorkSheet.Cells[9, 1] = "Hàng hóa:";
                    xlWorkSheet.Cells[10, 1] = "Mớn tàu đến:";
                    xlWorkSheet.Cells[11, 1] = "Thời gian tàu đến:";
                    xlWorkSheet.Cells[12, 1] = "Thời gian cập cầu:";
                    xlWorkSheet.Cells[13, 1] = "Thời gian bắt đầu dỡ hàng:";
                    xlWorkSheet.Cells[14, 1] = "Thời gian hoàn thành dỡ hàng:";
                    xlWorkSheet.Cells[15, 1] = "Thời gian tàu rời cảng:";
                    xlWorkSheet.Cells[16, 1] = "Mớn tàu rời đi";
                    xlWorkSheet.Cells[18, 1] = "Ghi chú: Các thông tin cập nhật dựa trên các báo cáo từ tàu/ đại lý … có thể mang tính đơn phương, dự đoán chưa được công nhận bởi các bên liên quan. Để biết thêm thông tin vui lòng liên hệ P.KTTV để có được Statement of Fleets của tàu tại các cảng có chữ ký/ được công nhận bởi các bên liên quan";
                    xlWorkSheet.Cells[7, 2] = report.UnloadReport.PortName;
                    xlWorkSheet.Cells[9, 2] = report.UnloadReport.Goods;
                    xlWorkSheet.Cells[10, 2] = report.UnloadReport.ComingCube;
                    xlWorkSheet.Cells[11, 2] = report.UnloadReport.PortComeDate;
                    xlWorkSheet.Cells[12, 2] = report.UnloadReport.PortComeTime;
                    xlWorkSheet.Cells[13, 2] = report.UnloadReport.LoadingTime;
                    xlWorkSheet.Cells[14, 2] = report.UnloadReport.LoadingComplTime;
                    xlWorkSheet.Cells[15, 2] = report.UnloadReport.LeavingTime;
                    xlWorkSheet.Cells[16, 2] = report.UnloadReport.LeavingCube;
                    fileName = " Do_hang_";
                    break;
                case 3:
                    xlWorkSheet.Cells[1, 1] = "CÔNG TY CỔ PHẦN VẬN TẢI VÀ THUÊ TÀU BIỂN VIỆT NAM";
                    xlWorkSheet.Cells[3, 1] = "Tên tàu";
                    xlWorkSheet.Cells[5, 2] = "TÓM LƯỢC TÌNH HÌNH TRANSIT/ BUNKERING/ DEVIATION";
                    xlWorkSheet.Cells[7, 1] = "Tên cảng/ kênh:";
                    xlWorkSheet.Cells[8, 1] = "Mục đích:";
                    xlWorkSheet.Cells[9, 1] = "Thời gian tàu đến:";
                    xlWorkSheet.Cells[10, 1] = "Thời gian bắt đầu:";
                    xlWorkSheet.Cells[11, 1] = "Thời gian kết thúc:";
                    xlWorkSheet.Cells[12, 1] = "Thời gian tàu chạy:";
                    xlWorkSheet.Cells[14, 1] = "Ghi chú: Các thông tin cập nhật dựa trên các báo cáo từ tàu/ đại lý … có thể mang tính đơn phương, dự đoán chưa được công nhận bởi các bên liên quan. Để biết thêm thông tin vui lòng liên hệ P.KTTV để có được Statement of Fleets của tàu tại các cảng có chữ ký/ được công nhận bởi các bên liên quan";
                    xlWorkSheet.Cells[7, 2] = report.UnsualReport.PortName;
                    xlWorkSheet.Cells[8, 2] = report.UnsualReport.Purpose;
                    xlWorkSheet.Cells[9, 2] = report.UnsualReport.ComingDate;
                    xlWorkSheet.Cells[10, 2] = report.UnsualReport.StartTime;
                    xlWorkSheet.Cells[11, 2] = report.UnsualReport.FinishTime;
                    xlWorkSheet.Cells[12, 2] = report.UnsualReport.RunTime;
                    fileName = " TRANSIT_BUNKERING_DEVIATION";
                    break;
                default:
                    break;
            }
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/") + "Report" + fileName + DateTime.Today.ToString("dd-MM-yyyy") + ".xls";
            xlWorkBook.SaveAs(path,
                XlFileFormat.xlWorkbookNormal,
                misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            return path;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }


        public void SaveChange()
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

            }

        }

    }
}
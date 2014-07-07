using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Pii.Areas.CargoShip.Models;

namespace Pii.Models
{
    public partial class PiiContext:DbContext
    {
        public virtual IDbSet<Plan> Plans { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Compare> Compares { get; set; }
        public virtual IDbSet<GoodsMakingPlace> GoodMakingPlaces { get; set; }
        public virtual IDbSet<Journey> Journeys { get; set; }
        public virtual IDbSet<JourneyPlan> journeyPlans { get; set; }
        public virtual IDbSet<JourneySupervision> JourneySuppervision { get; set; }
        public virtual IDbSet<Nation> Nations { get; set; }
        public virtual IDbSet<Note> Notes { get; set; }
        public virtual IDbSet<Port> Ports { get; set; }
        public virtual IDbSet<Reson> Resons { get; set; }
        public virtual IDbSet<Result> Results { get; set; }
        public virtual IDbSet<Ship> Ships { get; set; }
        public virtual IDbSet<Shipment> Shipments { get; set; }
        public virtual IDbSet<State> States { get; set; }
        public virtual IDbSet<Task> Tasks { get; set; }
        public virtual IDbSet<UpdateLog> UpdateLogs { get; set; }
        public virtual IDbSet<Variable> Variables { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GoodsMakingPlace>()
                  .HasRequired(p => p.Nation)
                  .WithMany()
                  .HasForeignKey(p => new { p.NationId}).WillCascadeOnDelete(false);
            modelBuilder.Entity<GoodsMakingPlace>().HasOptional(p => p.Port).WithMany().HasForeignKey(p => p.PortId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Journey>().HasRequired(p => p.GoodMakingPlace).WithRequiredDependent().Map(p=>p.MapKey("ContractNumber")) .WillCascadeOnDelete(false);
            modelBuilder.Entity<Journey>().HasRequired(p => p.Ship).WithMany().HasForeignKey(p => p.ShipId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Journey>().HasOptional(p => p.Port).WithMany().HasForeignKey(p=>p.StartPortId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneyPlan>().HasRequired(p => p.Ship).WithMany().HasForeignKey(p => p.ShipId).WillCascadeOnDelete(false);
           // modelBuilder.Entity<JourneyPlan>().HasRequired(p => p.Journey).WithMany().HasForeignKey(p => p.TripId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneyPlan>().HasOptional(p => p.Plan).WithMany().HasForeignKey(p => p.PlanId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneyPlan>().HasOptional(p => p.Port).WithMany().HasForeignKey(p => p.PortId).WillCascadeOnDelete(false);
           // modelBuilder.Entity<JourneySupervision>().HasRequired(p => p.Journey).WithMany().HasForeignKey(p => p.TripId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneySupervision>().HasRequired(p => p.Task).WithMany().HasForeignKey(p => p.TaskId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneySupervision>().HasOptional(p => p.Port).WithMany().HasForeignKey(p => p.PortId).WillCascadeOnDelete(false);
            modelBuilder.Entity<JourneySupervision>().HasOptional(p => p.State).WithMany().HasForeignKey(p => p.StateId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Note>().HasRequired(p => p.Ship).WithMany().HasForeignKey(p => p.ShipId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Note>().HasRequired(p => p.Journey).WithMany().HasForeignKey(p => p.TripId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Shipment>().HasRequired(p => p.Ship).WithMany().HasForeignKey(p => p.ShipId).WillCascadeOnDelete(false);
           // modelBuilder.Entity<Shipment>().HasRequired(p => p.Journey).WithMany().HasForeignKey(p => p.TripId).WillCascadeOnDelete(false);
           // modelBuilder.Entity<Shipment>().HasRequired(p => p.GoodsMakingPlace).WithMany().HasForeignKey(p => p.ContractNumber).WillCascadeOnDelete(false);
            modelBuilder.Entity<Shipment>().HasRequired(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Compare>().HasRequired(p => p.Plan).WithMany().HasForeignKey(p => p.PlanId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Compare>().HasRequired(p => p.State).WithMany().HasForeignKey(p => p.StateId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Result>().HasRequired(p => p.Ship).WithMany().HasForeignKey(p => p.ShipId).WillCascadeOnDelete(false);
           // modelBuilder.Entity<Result>().HasRequired(p => p.Journey).WithMany().HasForeignKey(p => p.TripId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Result>().HasRequired(p => p.Plan).WithMany().HasForeignKey(p => p.PlanId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Result>().HasRequired(p => p.State).WithMany().HasForeignKey(p => p.StateId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Result>().HasRequired(p => p.Reson).WithMany().HasForeignKey(p => p.ResonId).WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
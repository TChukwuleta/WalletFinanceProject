using Dot.Core.Enums;
using Dot.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Core.Entities
{
    public class Saving
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public string Duration { get; set; }
        public int Amount { get; set; }
        public SavingStatus SavingStatus { get; set; }
        public string SavingStatusDesc { get; set; }
        public FundingSource FundingSource { get; set; }
        public string FundingSourceDesc { get; set; }
        public SavingFrequency SavingFrequency { get; set; }
        public string SavingFrequencyDesc { get; set; }
        public SavingsType SavingsType { get; set; }
        public string SavingsTypeDesc { get; set; }
        public string Purpose { get; set; }
        public List<RequestBuddy>? SaveBuddies { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}

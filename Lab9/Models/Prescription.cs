﻿using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace APBD9.Models
{
    public class Prescription
    {
        [Key]
        public int IdPrescription { get; set; }

        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        public Patient Patient { get; set; }
        
        public Doctor Doctor { get; set; }
        
        public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    }
}

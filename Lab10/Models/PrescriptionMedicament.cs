﻿using System.ComponentModel.DataAnnotations;

namespace APBD9.Models
{
    public class PrescriptionMedicament
    {
        public int IdPrescription { get; set; }
        
        public int IdMedicament { get; set; }
        
        public Prescription Prescription { get; set; }
        
        public Medicament Medicament { get; set; }
        
        public int Dose { get; set; }
        
        [MaxLength(100)]
        [Required]
        public String Details { get; set; }
    }
}

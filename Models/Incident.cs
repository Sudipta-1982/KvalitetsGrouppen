using System;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcStarter.Models
{
  public class Incident
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime DateReported { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public DateTime Created { get; set; }

    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    // New properties
    [StringLength(200)]
    public string Customer { get; set; } = string.Empty;

    [StringLength(200)]
    public string Supplier { get; set; } = string.Empty;

    [StringLength(500)]
    public string AdditionalDescription { get; set; } = string.Empty;

    public DateTime? DateClosed { get; set; }

    public DateTime? DateFollowUp { get; set; }

    [StringLength(100)]
    public string OperatedBy { get; set; } = string.Empty;


    [StringLength(100)]
    public string SubmittedBy { get; set; } = string.Empty;

    public int? CustomerID { get; set; }

    public int? SupplierID { get; set; }

    public int? DocID { get; set; }

    [StringLength(100)]
    public string DocName { get; set; } = string.Empty;
    [StringLength(100)]
    public string SecretLabel { get; set; } = string.Empty;

    [StringLength(100)]
    public string Reference { get; set; } = string.Empty;
    [StringLength(500)]
    public string PreventingMeasures { get; set; } = string.Empty;

    public int? DecidedByUID { get; set; }

    [StringLength(500)]
    public string FollowUp { get; set; } = string.Empty;

    public int? FollowUID { get; set; }

    public bool Secret { get; set; }

    [StringLength(1000)]
    public string Notes { get; set; } = string.Empty;


    [StringLength(500)]
    public string CorrectiveMeasures { get; set; } = string.Empty;

    [StringLength(500)]
    public string Investigation { get; set; } = string.Empty;
  }
}

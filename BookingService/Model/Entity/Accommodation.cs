using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookingService.Model.Entity;

public class Accommodation
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ExternalId { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Owner { get; set; } = null!;

    [Required]
    public PriceType PriceType { get; set; }

    public ICollection<AvailabilityPeriod> AvailabilityPeriods { get; set; } = new List<AvailabilityPeriod>();
}
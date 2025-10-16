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
    
    [Range(1, int.MaxValue)]
    [Column("min_number_of_guests")]
    public int? MinNumberOfGuests { get; set; }

    [Range(0, int.MaxValue)]
    [Column("max_number_of_guests")]
    public int? MaxNumberOfGuests { get; set; }

    public ICollection<AvailabilityPeriod> AvailabilityPeriods { get; set; } = new List<AvailabilityPeriod>();
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
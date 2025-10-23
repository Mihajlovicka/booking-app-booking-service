using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Model.Entity;

public class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    
    [Column]
    public string GuestUsername { get; set; }
    
    [Column]
    public int GuestNumber { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalPrice { get; set; }
    
    [ForeignKey(nameof(Accommodation))]
    public int AccommodationId { get; set; }
    
    public Accommodation Accommodation { get; set; }
    
    [Required]
    [MaxLength(36)]
    [Column("external_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid ExternalId { get; set; }
    
    [Required]
    [Column]
    public bool Active { get; set; } = true;
}
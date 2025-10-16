using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Model.Entity;

public class ReservationRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }
    
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User Guest { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal FinalPrice { get; set; }
    
    [Column]
    public int GuestNumber { get; set; }
    
    [Required]
    [MaxLength(36)]
    [Column("external_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid ExternalId { get; set; }
    
    [ForeignKey(nameof(Accommodation))]
    public int AccommodationId { get; set; }

    public Accommodation Accommodation { get; set; }
}
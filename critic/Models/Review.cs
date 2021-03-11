using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Critic.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Review
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Key]
        [DataMember(Name = "id")]
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets UserID
        /// </summary>
        [ForeignKey("user")]
        [Column("user")]
        [DataMember(Name = "user")]
        [Required]
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or Sets User
        /// </summary>
        public AppUser User { get; set; }

        /// <summary>
        /// Gets or Sets RestaurantID
        /// </summary>
        [Column("restaurant")]
        [ForeignKey("restaurant")]
        [Required]
        public int? RestaurantID { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        [DataMember(Name="date")]
        [Column("date")]
        [Required]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name="comment")]
        [Column("comment")]
        [StringLength(4000)]
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets Rating
        /// </summary>
        [DataMember(Name="rating")]
        [Column("rating")]
        [Required]
        public decimal? Rating { get; set; }

        /// <summary>
        /// Gets or Sets ReplyID
        /// </summary>
        [Column("reply")]
        [ForeignKey("reply")]
        public int? ReplyID { get; set; }

        /// <summary>
        /// Gets or Sets User
        /// </summary>
        [NotMapped]
        [DataMember(Name = "reply")]
        public Reply Reply { get; set; }

        /// <summary>
        /// Gets or Sets User Image URL
        /// </summary>
        [NotMapped]
        [DataMember(Name = "userImage")]
        public string UserImage { get; set; }
    }
}

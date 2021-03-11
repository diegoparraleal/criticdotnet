
using System;
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
    public partial class Reply 
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
        /// Gets or Sets User Image URL
        /// </summary>
        [NotMapped]
        [DataMember(Name = "userImage")]
        public string UserImage { get; set; }

        /// <summary>
        /// Gets or Sets Date
        /// </summary>
        [DataMember(Name = "date")]
        [Column("date")]
        [Required]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name = "comment")]
        [Column("comment")]
        [StringLength(4000)]
        [Required]
        public string Comment { get; set; }


        /// <summary>
        /// Gets or Sets ReviewID
        /// </summary>
        [ForeignKey("review")]
        [Column("review")]
        public int ReviewID { get; set; }
    }
}

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
    public partial class Restaurant 
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
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name")]
        [Column("name")]
        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name="city")]
        [Column("city")]
        [StringLength(100)]
        [Required]
        public string City { get; set; }
        
        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name= "address")]
        [Column("address")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or Sets Price
        /// </summary>
        [DataMember(Name="price")]
        [Column("price")]
        [Required]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or Sets Image
        /// </summary>
        [DataMember(Name="image")]
        [Column("image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets Rating
        /// </summary>
        [DataMember(Name="rating")]
        [Column("rating")]
        public decimal? Rating { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name = "description")]
        [Column("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets OwnerID
        /// </summary>
        [Column("owner")]
        [ForeignKey("owner")]
        [DataMember(Name = "owner")]
        [Required]
        public int? OwnerID { get; set; }
    }
}


using System;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Critic.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Table("User")]
    public partial class AppUser
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Key]
        [DataMember(Name="id")]
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name")]
        [Column("name")]
        [StringLength(200)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [DataMember(Name="email")]
        [Column("email")]
        [StringLength(100)]
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets Image
        /// </summary>
        [DataMember(Name="image")]
        [Column("image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets Role
        /// </summary>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum Roles
        {
            /// <summary>
            /// Enum UserEnum for user
            /// </summary>
            [EnumMember(Value = "none")]
            None = 0,

            /// <summary>
            /// Enum UserEnum for user
            /// </summary>
            [EnumMember(Value = "user")]
            User = 1,
            /// <summary>
            /// Enum OwnerEnum for owner
            /// </summary>
            [EnumMember(Value = "owner")]
            Owner = 2,
            /// <summary>
            /// Enum AdminEnum for admin
            /// </summary>
            [EnumMember(Value = "admin")]
            Admin = 3 
        }

        /// <summary>
        /// Gets or Sets Role
        /// </summary>
        [DataMember(Name="role")]
        [Column("role")]
        [Required]
        public Roles? Role { get; set; }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}

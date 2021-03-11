
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Critic.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ReviewWithRestaurant 
    {
        /// <summary>
        /// Gets or Sets Review
        /// </summary>
        [DataMember(Name = "review")]
        public Review Review { get; set; }

        /// <summary>
        /// Gets or Sets Restaurant
        /// </summary>
        [DataMember(Name="restaurant")]
        public Restaurant Restaurant { get; set; }
    }
}

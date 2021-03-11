
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
    public partial class RestaurantWithReviews 
    { 
        /// <summary>
        /// Gets or Sets Restaurant
        /// </summary>
        [DataMember(Name="restaurant")]
        public Restaurant Restaurant { get; set; }

        /// <summary>
        /// Gets or Sets BestReview
        /// </summary>
        [DataMember(Name="bestReview")]
        public Review BestReview { get; set; }

        /// <summary>
        /// Gets or Sets WorstReview
        /// </summary>
        [DataMember(Name="worstReview")]
        public Review WorstReview { get; set; }

        /// <summary>
        /// Gets or Sets Reviews
        /// </summary>
        [DataMember(Name="reviews")]
        public List<Review> Reviews { get; set; }

    }
}

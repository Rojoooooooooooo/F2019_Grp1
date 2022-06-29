using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    public class PostModel
    {
        public string Id{ get; set; }
        public string ProfileId { get; set; }
        public DateTime CreatedAt{ get; set; }
        public string Content { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}